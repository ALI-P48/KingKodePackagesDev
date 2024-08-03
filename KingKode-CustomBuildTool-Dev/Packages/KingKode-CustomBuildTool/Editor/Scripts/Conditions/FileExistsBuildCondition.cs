using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KingKode.CustomBuildTool {
    
    [CreateAssetMenu(menuName = "ScriptableObjects/CustomBuildTool/Conditions/FileExists BuildCondition", fileName = "FileExists BuildCondition")]
    public class FileExistsBuildCondition : BuildCondition {
        [SerializeField] private List<string> _relativePaths;

        public override bool IsEligible() {
            foreach (var path in _relativePaths) {
                if (!File.Exists(Path.Combine(Application.dataPath, path))) {
                    Debug.LogError($"The file does not exist in path: {path}");
                    return false;
                }
            }
            return true;
        }
    }
}