using System.IO;
using UnityEditor;

namespace KingKode.CustomBuildTool {
    public class BuildWindowEditor : Editor {
        
        [MenuItem("Editor Tools/CustomBuildTool/Init CustomBuildTool")]
        public static void Init()
        {
            var templateGuid = AssetDatabase.FindAssets("t:" + nameof(BuildConfig) + " BuildConfig_TEMPLATE")[0];
            var templatePath = AssetDatabase.GUIDToAssetPath(templateGuid);
            BuildConfig buildConfigTemplate = AssetDatabase.LoadAssetAtPath<BuildConfig>(templatePath);
            
            CreateFolderAndConfig(buildConfigTemplate);
            CreateConditions();
            CreateActions();
            CreateKeyStorePassFile(buildConfigTemplate);
            SetResolverSettings();
                
            EditorUtility.DisplayDialog("Alert",
                "CustomBuildTool initialized.\n\nMake sure you enable \"Custom Main Gradle Template\" and \"Custom AndroidManifest\"in Android player settings.\n\n" + 
                "Also make sure you save key store pass in " + buildConfigTemplate.ReletiveKeyStorePassPath +".",
                "OK");
        }

        private static void CreateFolderAndConfig(BuildConfig buildConfigTemplate)
        {
            string dir = Path.Combine("Assets", "Resources", "CustomBuildTool");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string fileDir = Path.Combine("Assets", "Resources", "CustomBuildTool", "BuildConfig.asset");
            if (!File.Exists(fileDir))
            {
                BuildConfig tempConfig = buildConfigTemplate.Clone();
                AssetDatabase.CreateAsset(tempConfig, fileDir);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateConditions()
        {
            string conditionsDir = Path.Combine("Assets", "Resources", "CustomBuildTool", "Conditions");
            if (!Directory.Exists(conditionsDir))
            {
                Directory.CreateDirectory(conditionsDir);
            }
            
            var conditionsGuid = AssetDatabase.FindAssets("t:" + nameof(BuildCondition) + " BuildCondition_TEMPLATE");
                
            foreach (string conditionGuid in conditionsGuid)
            {
                var conditionsPath = AssetDatabase.GUIDToAssetPath(conditionGuid);
                BuildCondition condition = AssetDatabase.LoadAssetAtPath<BuildCondition>(conditionsPath);
                BuildCondition clone = condition.Clone();
                string cloneName = clone.name.Replace("_TEMPLATE", "") + ".asset";
                string clonePath = Path.Combine(conditionsDir, cloneName);
                if (!File.Exists(clonePath))
                {
                    AssetDatabase.CreateAsset(clone, clonePath);
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private static void CreateActions()
        {
            string actionsDir = Path.Combine("Assets", "Resources", "CustomBuildTool", "Actions");
            if (!Directory.Exists(actionsDir))
            {
                Directory.CreateDirectory(actionsDir);
            }
            
            var actionsGuid = AssetDatabase.FindAssets("t:" + nameof(BuildAction) + " BuildAction_TEMPLATE");
                
            foreach (string actionGuid in actionsGuid)
            {
                var actionsPath = AssetDatabase.GUIDToAssetPath(actionGuid);
                BuildAction action = AssetDatabase.LoadAssetAtPath<BuildAction>(actionsPath);
                BuildAction clone = action.Clone();
                string cloneName = clone.name.Replace("_TEMPLATE", "") + ".asset";
                string clonePath = Path.Combine(actionsDir, cloneName);
                if (!File.Exists(clonePath))
                {
                    AssetDatabase.CreateAsset(clone, clonePath);
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateKeyStorePassFile(BuildConfig buildConfigTemplate)
        {
            string keyStorePassFilePath = Utils.GetAbsolutePath(buildConfigTemplate.ReletiveKeyStorePassPath);

            if (!Directory.Exists(Path.GetDirectoryName(keyStorePassFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(keyStorePassFilePath));
            }
            if (!File.Exists(keyStorePassFilePath))
            {
                File.Create(keyStorePassFilePath).Dispose();
            }
        }

        private static void SetResolverSettings()
        {
            var projectSettings = new Google.ProjectSettings("GooglePlayServices.");
            projectSettings.SetBool("GooglePlayServices.AutoResolveOnBuild", true);
            projectSettings.SetBool("GooglePlayServices.PatchMainTemplateGradle", true);
        }
    }
}
