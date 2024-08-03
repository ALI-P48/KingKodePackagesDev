using UnityEngine;

namespace KingKode.CustomBuildTool {
    
    public class BuildCondition : ScriptableObject {
        public virtual bool IsEligible() {
            return false;
        }
    }
}