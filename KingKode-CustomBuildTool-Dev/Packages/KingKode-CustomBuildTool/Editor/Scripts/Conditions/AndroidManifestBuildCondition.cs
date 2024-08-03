using System.IO;
using System.Xml;
using Google;
using UnityEditor;
using UnityEngine;

namespace KingKode.CustomBuildTool
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CustomBuildTool/Conditions/AndroidManifest BuildCondition", fileName = "AndroidManifest BuildCondition")]
    public class AndroidManifestBuildCondition : BuildCondition
    {
        [Space(10)]
        [Header("com.google.android.gms.permission.AD_ID")]
        [Tooltip("Ensures that com.google.android.gms.permission.AD_ID permission is present in the manifest in order for ADID collection to continue to take place as of API 31 and beyond.")]
        public bool CheckADIDCollectionPermission = true;
        
        [Space(10)]
        [Header("android:debuggable=\"false\"")]
        [Tooltip("You can not publish the build if there is a android:debuggable=\"true\" attribute in any AndroidManifest file.")]
        public bool CheckDebuggableAttribute = true;
        
        [Space(10)]
        [Header("android:usesCleartextTraffic=\"true\"")]
        [Tooltip("If any http url is going to be called in the project, usesCleartextTraffic=\"true\" attribute must exist on every <application> element in all AndroidManifest files.")]
        public bool CheckUseClearTrafficTextAttribute= true;
        
        [Space(10)]
        [Header("android:exported=\"true\"")]
        [Tooltip("On android API level 31 and above, exported=\"true\" Attribute must be present and equal to true on any activity element in any AndroidManifest file that has an \"intent-filter\" element in the activity, Or else the build is not going to work on devices with Android 12.")]
        public bool CheckManifestExportedAttribute= true;
        public bool CheckFreshchatReceiverExportedAttribute= true;

        
        [Space(10)]
        [Header("android:allowNativeHeapPointerTagging=\"false\"")]
        public bool CheckAllowNativeHeapPointerAttribute = true;
        
        [Space(10)]
        [Header("android:extractNativeLibs=\"true\"")]
        public bool CheckExtractNativeLibsAttribute = true;
        
        [Space(10)]
        [Header("\"uses-sdk\" element")]
        [Tooltip("Makes sure that \"uses-sdk\" element is not present in any of the AndroidManifest.xml files in the project. If this element exist in a manifest, there are going to be errors on build.")]
        public bool CheckUsesSDKElement = true;
        
        [Space(10)]
        [Header("GoogleAds metadata elements")]
        [Tooltip("Makes sure \"com.google.android.gms.ads.flag.OPTIMIZE_INITIALIZATION\" & \"com.google.android.gms.ads.flag.OPTIMIZE_AD_LOADING\" metadata elements are present in the main AndroidManifest file and both are set to true.")]
        public bool CheckGoogleAdsMetaDataElement = true;
        
        [Space(10)]
        [Header("\"metrix_appId\" metadata")]
        [Tooltip("This metadata element has to be present in the main AndroidManifest file in order for the Metrix plugin to work properly.")]
        public bool CheckMetrixMetaDataElement = true;

        private string mainAndroidManifestPath => Application.dataPath + "\\Plugins\\Android\\AndroidManifest.xml";
        
        public override bool IsEligible()
        {
            #if UNITY_ANDROID
                return checkDebuggableAttribute() &&
                       checkUseClearTrafficTextAttribute() &&
                       checkManifestExportedAttribute() &&
                       checkFreshchatReceiverExportedAttribute() &&
                       checkAllowNativeHeapPointerAttribute() &&
                       checkExtractNativeLibsAttribute() &&
                       checkUsesSDKElement() &&
                       checkGoogleAdsMetaDataElement() &&
                       checkMetrixMetaDataElement() &&
                       checkADIDCollectionPermission();
            #else
                return true;
            #endif
        }
        
        private bool checkADIDCollectionPermission()
        {
            if (!CheckADIDCollectionPermission)
                return true;

            using (StreamReader sr = new StreamReader(mainAndroidManifestPath))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(sr, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLower() == "uses-permission")
                        {
                            if (reader.GetAttribute("android:name") == "com.google.android.gms.permission.AD_ID")
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            
            Debug.LogError("Add <uses-permission android:name=\"com.google.android.gms.permission.AD_ID\"/> to main manifest file in order for ADID collection to continue to take place as of API 31 and beyond.");

            return false;
        }

        private bool checkDebuggableAttribute()
        {
            if (!CheckDebuggableAttribute)
                return true;

            string[] files = System.IO.Directory.GetFiles(Application.dataPath, "AndroidManifest.xml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    using (XmlReader reader = XmlReader.Create(sr, settings))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLower() == "application")
                            {
                                bool att = false;
                                bool exists = bool.TryParse(reader.GetAttribute("android:debuggable"), out att);
                                if (exists && att)
                                {
                                    Debug.LogError("android:debuggable=\"true\" attribute detected in " + file + ". Make sure to set this attribute to false in AndroidManifest files.");
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        
        private bool checkUseClearTrafficTextAttribute()
        {
            if (!CheckUseClearTrafficTextAttribute)
                return true;
            
            using (StreamReader sr = new StreamReader(mainAndroidManifestPath))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(sr, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLower() == "application")
                        {
                            bool cleartextValue = true;
                            bool cleartextExists = bool.TryParse(reader.GetAttribute("android:usesCleartextTraffic"), out cleartextValue);
                            string networkConfigValue = reader.GetAttribute("android:networkSecurityConfig");
                            if (!cleartextExists || !cleartextValue || networkConfigValue!="@xml/network_security_config")
                            {
                                Debug.LogError("\"usesCleartextTraffic=\"true\"\" is no present in android manifest file. Make sure to add this tag to Android Manifest file.");
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        
        
        private bool checkManifestExportedAttribute()
        {
            if (!CheckManifestExportedAttribute)
                return true;

            string[] files = System.IO.Directory.GetFiles(Application.dataPath, "AndroidManifest.xml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    using (XmlReader reader = XmlReader.Create(sr, settings))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element
                                && reader.Name.ToLower() == "activity")
                            {
                                bool att = true;
                                bool exists = bool.TryParse(reader.GetAttribute("android:exported"), out att);
                                XmlReader inner = reader.ReadSubtree();
                                while (inner.Read())
                                {
                                    if (inner.NodeType == XmlNodeType.Element
                                        && inner.Name.ToLower() == "intent-filter")
                                    {
                                        if (!exists || !att)
                                        {
                                            Debug.LogError( "On android API level 31 and above, exported=\"true\" Attribute must be present and equal to true on any activity element in any AndroidManifest file that has an \"intent-filter\" element in the activity, Or else the build is not going to work on devices with Android 12.");
                                            Debug.LogError("Make sure you add \"exported=\"true\"\" to activity element in " + file);
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        
        private bool checkFreshchatReceiverExportedAttribute()
        {
            if (!CheckFreshchatReceiverExportedAttribute)
                return true;

            bool existFlag1 = false;
            bool existFlag2 = false;
            using (StreamReader sr = new StreamReader(mainAndroidManifestPath))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(sr, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLower() == "receiver")
                        {
                            string name = reader.GetAttribute("android:name");
                            if (name == "com.freshchat.consumer.sdk.receiver.FreshchatReceiver")
                            {
                                existFlag1 = true;
                                bool exported = true;
                                bool exportedExists = bool.TryParse(reader.GetAttribute("android:exported"), out exported);
                                string merge = reader.GetAttribute("tools:node");
                                if (!exportedExists || !exported || merge!="merge")
                                {
                                    Debug.LogError("You have to include freshchat receiver element in man AndroidManifest and set exported attribute of it to true as well as setting node value of it to \"merge\".");
                                    return false;
                                }
                            }
                            if (name == "com.freshchat.consumer.sdk.receiver.FreshchatNetworkChangeReceiver")
                            {
                                existFlag2 = true;
                                bool exported = true;
                                bool exportedExists = bool.TryParse(reader.GetAttribute("android:exported"), out exported);
                                string merge = reader.GetAttribute("tools:node");
                                if (!exportedExists || !exported || merge!="merge")
                                {
                                    Debug.LogError("You have to include FreshchatNetworkChangeReceiver receiver element in man AndroidManifest and set exported attribute of it to true as well as setting node value of it to \"merge\".");
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            
            if (!existFlag1)
            {
                Debug.LogError("You have to include freshchat receiver element in man AndroidManifest and set exported attribute of it to true as well as setting node value of it to \"merge\".");
                return false;
            }

            if (!existFlag2)
            {
                Debug.LogError("You have to include FreshchatNetworkChangeReceiver receiver element in man AndroidManifest and set exported attribute of it to true as well as setting node value of it to \"merge\".");
                return false;
            }
            
            return true;
        }

        private bool checkAllowNativeHeapPointerAttribute()
        {
            if (!CheckAllowNativeHeapPointerAttribute)
                return true;

            using (StreamReader sr = new StreamReader(mainAndroidManifestPath))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(sr, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLower() == "application")
                        {
                            bool att = false;
                            bool exists = bool.TryParse(reader.GetAttribute("android:allowNativeHeapPointerTagging"), out att);
                            if (!exists || att)
                            {
                                Debug.LogError("\"android:allowNativeHeapPointerTagging=\"false\"\" is not present in android manifest file. Make sure to add this tag to Android Manifest file.");
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool checkExtractNativeLibsAttribute()
        {
            if (!CheckExtractNativeLibsAttribute)
                return true;

            using (StreamReader sr = new StreamReader(mainAndroidManifestPath))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(sr, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLower() == "application")
                        {
                            bool att = true;
                            bool exists = bool.TryParse(reader.GetAttribute("android:extractNativeLibs"), out att);
                            if (!exists || !att)
                            {
                                Debug.LogError("\"extractNativeLibs=\"true\"\" is no present in android manifest file. Make sure to add this tag to Android Manifest file.");
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool checkUsesSDKElement()
        {
            if (!CheckUsesSDKElement)
                return true;

            string[] files = System.IO.Directory.GetFiles(Application.dataPath, "AndroidManifest.xml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    using (XmlReader reader = XmlReader.Create(sr, settings))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLower() == "uses-sdk")
                            {
                                Debug.LogError("\"uses-sdk\" tag detected in " + file + ". Make sure to remove this tag in AndroidManifest files or else you will have errors on build.");
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        
        private bool checkGoogleAdsMetaDataElement()
        {
            if (!CheckGoogleAdsMetaDataElement)
                return true;
            
            bool existFlag1 = false;
            bool existFlag2 = false;
            using (StreamReader sr = new StreamReader(mainAndroidManifestPath))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(sr, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLower() == "meta-data")
                        {
                            string name = reader.GetAttribute("android:name");
                            if (name == "com.google.android.gms.ads.flag.OPTIMIZE_INITIALIZATION")
                            {
                                existFlag1 = true;
                                bool value = true;
                                bool valueExists = bool.TryParse(reader.GetAttribute("android:value"), out value);
                                if (!valueExists || !value)
                                {
                                    Debug.LogError("GoogleAds metadata is no present in the main android manifest file. Make sure to add below tags to Android Manifest file:");
                                    Debug.LogError("<meta-data android:name=\"com.google.android.gms.ads.flag.OPTIMIZE_INITIALIZATION\" android:value=\"true\" />");
                                    return false;
                                }
                            }
                            if (name == "com.google.android.gms.ads.flag.OPTIMIZE_AD_LOADING")
                            {
                                existFlag2 = true;
                                bool value = true;
                                bool valueExists = bool.TryParse(reader.GetAttribute("android:value"), out value);
                                if (!valueExists || !value)
                                {
                                    Debug.LogError("GoogleAds metadata is no present in the main android manifest file. Make sure to add below tags to Android Manifest file:");
                                    Debug.LogError("<meta-data android:name=\"com.google.android.gms.ads.flag.OPTIMIZE_AD_LOADING\" android:value=\"true\" />");
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            if (!existFlag1)
            {
                Debug.LogError("GoogleAds metadata is no present in the main android manifest file. Make sure to add below tags to Android Manifest file:");
                Debug.LogError("<meta-data android:name=\"com.google.android.gms.ads.flag.OPTIMIZE_INITIALIZATION\" android:value=\"true\" />");
                return false;
            }

            if (!existFlag2)
            {
                Debug.LogError("GoogleAds metadata is no present in the main android manifest file. Make sure to add below tags to Android Manifest file:");
                Debug.LogError("<meta-data android:name=\"com.google.android.gms.ads.flag.OPTIMIZE_AD_LOADING\" android:value=\"true\" />");
                return false;
            }
            
            return true;
        }
        
        
        private bool checkMetrixMetaDataElement()
        {
            if (!CheckMetrixMetaDataElement)
                return true;
            
            using (StreamReader sr = new StreamReader(mainAndroidManifestPath))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                using (XmlReader reader = XmlReader.Create(sr, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name.ToLower() == "meta-data")
                        {
                            string name = reader.GetAttribute("android:name");
                            if (name == "metrix_appId")
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            Debug.LogError("\"metrix_appId\" metadata is no present in the main android manifest file. Make sure to add this tag to Android Manifest file.");
            Debug.LogError("Add this line to application tag in android manifest file: <meta-data android:name=\"metrix_appId\" android:value=\"jzbrsoaengvmeju\" />");
            return false;
        }
    }
}