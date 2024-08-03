using UnityEditor;
using UnityEngine;

namespace KingKode.CustomBuildTool {
    
    [CustomEditor(typeof(BuildCondition), true)]
    public class BuildConditionEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Test Condition")) {
                ((BuildCondition) target).IsEligible();
            }
        }
    }
}