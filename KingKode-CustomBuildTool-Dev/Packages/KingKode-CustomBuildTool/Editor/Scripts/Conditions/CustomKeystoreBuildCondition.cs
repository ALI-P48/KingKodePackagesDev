using UnityEditor;
using UnityEngine;

namespace KingKode.CustomBuildTool {

    [CreateAssetMenu(menuName = "ScriptableObjects/CustomBuildTool/Conditions/CustomKeystore BuildCondition", fileName = "CustomKeystore BuildCondition")]
    public class CustomKeystoreBuildCondition : BuildCondition {
        [SerializeField] private bool _activate;

        public override bool IsEligible() {
            if (PlayerSettings.Android.useCustomKeystore && !_activate ||
                !PlayerSettings.Android.useCustomKeystore && _activate) {
                Debug.LogError($"the keystore must be {_activate}");
                return false;
            }
            return true;
        }
    }
}