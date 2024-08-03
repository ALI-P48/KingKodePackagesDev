using UnityEditor;
using UnityEngine;

namespace KingKode.CustomBuildTool {
    
    [CustomEditor(typeof(BuildAction), true)]
    public class BuildActionEditor : Editor {

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            BuildAction config = (BuildAction)target;

            if (GUILayout.Button("Act"))
            {
                config.Act();
            }
        }
    }
}