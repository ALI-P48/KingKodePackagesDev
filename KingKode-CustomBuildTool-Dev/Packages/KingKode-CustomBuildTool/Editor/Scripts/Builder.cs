using System.IO;
using System.Linq;
using Google.Android.AppBundle.Editor;
using Google.Android.AppBundle.Editor.Internal;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Rendering;

namespace KingKode.CustomBuildTool {
    
    public static class Builder {

        public static void Build(int buildInfoIndex) {
            var globalInfo = BuildWindow.config;
            var localInfo = globalInfo.BuildInfos[buildInfoIndex];

            if (!SetKeyStorePass())
            {
                EditorUtility.DisplayDialog("Warning", "the keystore pass file not found.", "Ok");
                return;
            }

            var levels = localInfo.Scenes.Select(AssetDatabase.GetAssetPath);
            if (!levels.Any()) {
                EditorUtility.DisplayDialog("Warning", "there are no scenes!", "Ok");
                return;
            }

            var buildPath = Path.Combine($"{Application.dataPath}/..", globalInfo.RelativeBuildFolderPath,
                localInfo.GetFileName());

            //check path
            if (!IsPathValid(buildPath)) return;

            //check conditions
            if ( /*globalInfo.Conditions.Any(condition => !condition.IsEligible()) ||*/
                localInfo.Conditions.Any(condition => !condition.IsEligible()))
                return;

            //globalInfo.PreBuildActions?.Invoke();
            localInfo.PrebuildActions?.Invoke();
            AssetDatabase.Refresh();

            var options = new BuildPlayerOptions() {
                options = localInfo.BuildOptions,
                scenes = levels.ToArray(),
                locationPathName = buildPath,
                target = localInfo.Target,
                targetGroup = localInfo.TargetGroup
            };

            EditorUserBuildSettings.waitForManagedDebugger =
                localInfo.BuildOptions.HasFlag(BuildOptions.AllowDebugging);

            if (localInfo.Target == BuildTarget.iOS) {
                PlayerSettings.SetGraphicsAPIs(BuildTarget.iOS, new[] {
                    GraphicsDeviceType.Metal,
                    GraphicsDeviceType.OpenGLES3,
                    GraphicsDeviceType.OpenGLES2
                });
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            }
            else if (localInfo.Target == BuildTarget.Android) {
                PlayerSettings.Android.targetArchitectures = localInfo.AndroidArchitecture;
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, localInfo.ScriptingImplementation);
                EditorUserBuildSettings.androidCreateSymbols = localInfo.AndroidCreateSymbols;
                EditorUserBuildSettings.buildAppBundle = localInfo.BuildAppBundle;
            }

            if (localInfo.ResolveOnBuild)
            {
                GooglePlayServices.PlayServicesResolver.Resolve(null, true, (bool success) =>
                {
                    if (success)
                    {
                        BuildPlayer(localInfo, options);
                    }
                    else
                    {
                        Debug.LogError("Force resolve failed.");
                    }
                });
            }
            else
            {
                BuildPlayer(localInfo, options);
            }
        }

        private static void BuildPlayer(BuildInfo info, BuildPlayerOptions options)
        {
            if (info.Target == BuildTarget.Android && info.BuildAppBundle && info.Google) {
                AppBundlePublisher.Build(options, new AssetPackConfig() {SplitBaseModuleAssets = false}, false);
            }
            else {
                BuildPipeline.BuildPlayer(options);
            }
        }

        private static bool IsPathValid(string path) {
            if (Application.isBatchMode)
                return true;
            
            if (File.Exists(path)) {
                return EditorUtility.DisplayDialog("Overwrite!", "This file already exists!\nOverwrite?", "Ok", "No");
            }
            return true;
        }
        
        private static bool SetKeyStorePass()
        {
            try
            {
                var pass = GetKeyStorePass();
                PlayerSettings.keystorePass = pass;
                PlayerSettings.keyaliasPass = pass;
                AssetDatabase.Refresh();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        
        private static string GetKeyStorePass()
        {
            var pass = File.ReadAllText(Utils.GetAbsolutePath(BuildWindow.config.ReletiveKeyStorePassPath));
            return pass;
        }
    }
}
