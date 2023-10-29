using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Windows;

namespace KingKodePackages.CustomBuildTool {
    public class BuildWindow : EditorWindow {

        private SerializedObject _target;
        public static BuildConfig config;

        private SerializedProperty RelativeBuildFolderPath;
        private SerializedProperty ReletiveKeyStorePassPath;
        
        private SerializedProperty BuildInfos;

        private Vector2 _scrollPos;
        private int _tabIndex;
        
        private GUIStyle _horizontalLine;
        private string _version;
        private int _versionCode;
        
        [MenuItem("Editor Tools/Init CustomBuildTool")]
        public static void Init()
        {
            var templateGuid = AssetDatabase.FindAssets("t:" + nameof(BuildConfig) + " BuildConfig_TEMPLATE")[0];
            var templatePath = AssetDatabase.GUIDToAssetPath(templateGuid);
            BuildConfig buildConfigTemplate = AssetDatabase.LoadAssetAtPath<BuildConfig>(templatePath);

            if (!Directory.Exists("Assets/CustomBuildTool"))
            {
                AssetDatabase.CreateFolder("Assets", "CustomBuildTool");
            }

            if (!File.Exists("Assets/CustomBuildTool/BuildConfig.asset"))
            {
                //BuildConfig tempConfig = ScriptableObject.CreateInstance<BuildConfig>();
                //tempConfig.CopyFrom(buildConfigTemplate);
                BuildConfig tempConfig = buildConfigTemplate.Clone();
                AssetDatabase.CreateAsset(tempConfig, "Assets/CustomBuildTool/BuildConfig.asset");
            }
            
            if (!Directory.Exists("Assets/CustomBuildTool/Conditions"))
            {
                string conditionsFolderGuid = AssetDatabase.CreateFolder("Assets/CustomBuildTool", "Conditions");
                string conditionsFolderPath = AssetDatabase.GUIDToAssetPath(conditionsFolderGuid);
            }
            
            var conditionsGuid = AssetDatabase.FindAssets("t:" + nameof(BuildCondition) + " BuildCondition_TEMPLATE");
                
            foreach (string conditionGuid in conditionsGuid)
            {
                var conditionsPath = AssetDatabase.GUIDToAssetPath(conditionGuid);
                BuildCondition condition = AssetDatabase.LoadAssetAtPath<BuildCondition>(conditionsPath);
                BuildCondition clone = condition.Clone();
                string cloneName = clone.name.Replace("_TEMPLATE", "") + ".asset";
                string clonePath = "Assets/CustomBuildTool/Conditions/" + cloneName;
                if (!File.Exists(clonePath))
                {
                    AssetDatabase.CreateAsset(clone, clonePath);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("Editor Tools/Show Custom Build Window")]
        static void ShowWindow() {
            var window = GetWindow<BuildWindow>("Custom Build Window");
            window.Show();
        }

        private void OnEnable() {
            var buildConfig = AssetDatabase.FindAssets("t:" + nameof(BuildConfig));
            var path = AssetDatabase.GUIDToAssetPath(buildConfig[0]);
            _target = new SerializedObject(AssetDatabase.LoadAssetAtPath<BuildConfig>(path));
            config = (BuildConfig) _target.targetObject;
            RelativeBuildFolderPath = _target.FindProperty("RelativeBuildFolderPath");
            ReletiveKeyStorePassPath = _target.FindProperty("ReletiveKeyStorePassPath");
            BuildInfos = _target.FindProperty("BuildInfos");
        }
        
        private void OnFocus()
        {
            _version = PlayerSettings.bundleVersion;
            #if UNITY_ANDROID
                _versionCode = PlayerSettings.Android.bundleVersionCode;
            #elif UNITY_IOS
                _versionCode = int.Parse(PlayerSettings.iOS.buildNumber);
            #endif
        }

        private void OnGUI() {
            _target.Update();
            _tabIndex = GUILayout.Toolbar(_tabIndex, new string[] {"Build", "Build Editor"},
                GUILayout.ExpandWidth(true));
            _scrollPos = GUILayout.BeginScrollView(_scrollPos);

            EditorGUILayout.Space(10);

            if (_tabIndex == 0)
            {
                AppVersionSetting();
                DrawSpace(10f);
                DrawLine();
                DrawSpace(10f);
                for (var i = 0; i < config.BuildInfos.Count; i++) {
                    var info = config.BuildInfos[i];
                    if(info.Target != EditorUserBuildSettings.activeBuildTarget) continue;
                    
                    var style = GetStyleWithBackgroundColor(info.Color);
                    EditorGUILayout.BeginHorizontal(style);
                    if (GUILayout.Button($"{info.Name}\n{info.GetFileName()}")) {
                        if (EditorUtility.DisplayDialog("Warning!", $"Build For {info.Name}", "Yes", "No No No")) {
                            Builder.Build(i);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            if (_tabIndex == 1) {
                EditorGUILayout.PropertyField(RelativeBuildFolderPath);
                EditorGUILayout.PropertyField(ReletiveKeyStorePassPath);
                EditorGUILayout.PropertyField(BuildInfos);
            }
            
            GUILayout.EndScrollView();
            _target.ApplyModifiedProperties();
        }
        
        private void AppVersionSetting()
        {
            EditorGUILayout.Separator();
            GUILayout.BeginHorizontal();
            _version = EditorGUILayout.TextField(_version);
            _versionCode = EditorGUILayout.IntField(_versionCode);
            if (GUILayout.Button("Edit"))
            {
                if (PlayerSettings.bundleVersion != _version
                    #if UNITY_ANDROID
                        || PlayerSettings.Android.bundleVersionCode != _versionCode
                    #elif UNITY_IOS
                        || PlayerSettings.iOS.buildNumber != _versionCode.ToString()
                    #endif
                )
                {
                    PlayerSettings.bundleVersion = _version;
                    #if UNITY_IOS
                        PlayerSettings.iOS.buildNumber = _versionCode.ToString();
                    #else
                        PlayerSettings.Android.bundleVersionCode = _versionCode;
                    #endif
                    OnFocus();
                }
            }

            EditorGUILayout.Separator();
            GUILayout.EndHorizontal();
        }
        
        private void DrawLine()
        {
            _horizontalLine = new GUIStyle();
            GUI.color = Color.black;
            _horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
            _horizontalLine.fixedHeight = 1;
            GUILayout.Box(EditorGUIUtility.whiteTexture, _horizontalLine);
            GUI.color = Color.white;
        }
        
        private void DrawSpace(float space)
        {
            GUILayout.Space(space);
        }
        
        private GUIStyle GetStyleWithBackgroundColor(Color color)
        {
            GUIStyle style = new GUIStyle
            {
                normal =
                {
                    background = MakeTex(4, 4, color),
                }
            };
            return style;
        }
        
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
