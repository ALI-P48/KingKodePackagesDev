using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Events;

namespace KingKodePackages.CustomBuildTool {

    [Serializable]
    public class BuildInfo {

        public string Name;
        public string FileName;

        public List<BuildCondition> Conditions;

        public UnityEvent PrebuildActions;

        public List<SceneAsset> Scenes;

        public BuildOptions BuildOptions;
        public ScriptingImplementation ScriptingImplementation;
        public AndroidArchitecture AndroidArchitecture;
        public AndroidCreateSymbols AndroidCreateSymbols;
        public bool BuildAppBundle;
        public bool Google;
        public BuildTarget Target = BuildTarget.Android;
        public BuildTargetGroup TargetGroup = BuildTargetGroup.Android;
        public UnityEngine.Color Color;

        public string GetFileName() {
            #if UNITY_ANDROID
                return $"{FileName}-{PlayerSettings.bundleVersion}-{PlayerSettings.Android.bundleVersionCode}{(BuildAppBundle?".aab":".apk")}";
            #elif UNITY_IOS
                return $"{FileName}-{PlayerSettings.bundleVersion}-{PlayerSettings.iOS.buildNumber}";
            #else
            return $"{FileName}-{PlayerSettings.bundleVersion}-{PlayerSettings.Android.bundleVersionCode}{(BuildAppBundle?".aab":".apk")}";
            #endif
        }
    }
}