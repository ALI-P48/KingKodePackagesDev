using Google;
using UnityEditor;
using UnityEngine;

namespace KingKodePackages.CustomBuildTool
{
    [CreateAssetMenu(menuName = "CustomBuildTool/Conditions/IOSSign BuildCondition", fileName = "IOSSign BuildCondition")]
    public class IOSSignBuildCondition : BuildCondition
    {
        [SerializeField] private string teamID;
        
        public override bool IsEligible()
        {
            return PlayerSettings.iOS.appleEnableAutomaticSigning && PlayerSettings.iOS.appleDeveloperTeamID == teamID;
        }
    }
}
