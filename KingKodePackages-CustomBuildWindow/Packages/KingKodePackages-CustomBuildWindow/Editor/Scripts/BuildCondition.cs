using UnityEngine;

namespace KingKodePackages.CustomBuildTool {
    
    [CreateAssetMenu(menuName = "CustomBuildTool/Conditions/BuildCondition", fileName = "BuildCondition")]
    public class BuildCondition : ScriptableObject {
        [ContextMenu("Test Condition")]
        public virtual bool IsEligible() {
            return false;
        }
    }
}