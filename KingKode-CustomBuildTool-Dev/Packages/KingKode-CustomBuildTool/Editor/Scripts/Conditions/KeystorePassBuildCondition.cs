using UnityEditor;
using UnityEngine;

namespace KingKode.CustomBuildTool {
    [CreateAssetMenu(menuName = "ScriptableObjects/CustomBuildTool/Conditions/KeystorePass BuildCondition", fileName = "KeystorePass BuildCondition")]
    public class KeystorePassBuildCondition : BuildCondition {
        [SerializeField] private bool _full;

        public override bool IsEligible() {
            if (string.IsNullOrEmpty(PlayerSettings.Android.keystorePass) && _full ||
                !string.IsNullOrEmpty(PlayerSettings.Android.keystorePass) && !_full ||
                string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass) && _full ||
                !string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass) && !_full) {
                var f = _full ? "full" : "Empty";
                Debug.LogError($"the keystore pass must be {f}");
                return false;
            }

            return true;
        }
    }
}