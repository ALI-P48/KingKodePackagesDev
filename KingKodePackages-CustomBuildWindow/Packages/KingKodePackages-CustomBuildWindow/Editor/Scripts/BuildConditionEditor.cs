using UnityEditor;
using UnityEngine;

namespace KingKodePackages.CustomBuildTool {
    
    [CustomEditor(typeof(BuildCondition))]
    public class BuildConditionSOEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Test Condition")) {
                ((BuildCondition) target).IsEligible();
            }
        }
    }
}