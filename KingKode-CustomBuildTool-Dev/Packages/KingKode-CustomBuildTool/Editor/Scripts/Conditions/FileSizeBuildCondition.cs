using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KingKode.CustomBuildTool {
    [CreateAssetMenu(menuName = "ScriptableObjects/CustomBuildTool/Conditions/FileSize BuildCondition", fileName = "FileSize BuildCondition")]
    public class FileSizeBuildCondition : BuildCondition {

        [SerializeField] private List<Item> _items;

        public override bool IsEligible() {
            foreach (var item in _items) {
                var file = new FileInfo($"{Application.dataPath}/{item.relativePath}");
                if (file.Length < item.size) {
                    Debug.LogError($"{item.relativePath} file size is less than {item.size}");
                    return false;
                }
            }
            return true;
        }

        [Serializable]
        private class Item {
            public string relativePath;
            public long size;
        }
    }
}