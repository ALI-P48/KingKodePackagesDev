using UnityEditor;
using UnityEngine;

namespace KingKode.CustomBuildTool {

    [CreateAssetMenu(menuName = "ScriptableObjects/CustomBuildTool/Conditions/TargetAPI BuildCondition", fileName = "TargetAPI BuildCondition")]
    public class TargetAPIBuildCondition : BuildCondition {

        [SerializeField] private int _targetSdkVersion;

        public override bool IsEligible() {
            if ((int) PlayerSettings.Android.targetSdkVersion == _targetSdkVersion) {
                return true;
            }
            Debug.LogError($"targetSdkVersion is not {_targetSdkVersion}");
            return false;
        }
    }
}