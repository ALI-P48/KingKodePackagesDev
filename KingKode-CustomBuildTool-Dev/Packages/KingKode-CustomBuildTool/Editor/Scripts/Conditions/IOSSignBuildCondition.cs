using Google;
using UnityEditor;
using UnityEngine;

namespace KingKode.CustomBuildTool
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CustomBuildTool/Conditions/IOSSign BuildCondition", fileName = "IOSSign BuildCondition")]
    public class IOSSignBuildCondition : BuildCondition
    {
        [SerializeField] private string teamID;
        
        public override bool IsEligible()
        {
            return PlayerSettings.iOS.appleEnableAutomaticSigning && PlayerSettings.iOS.appleDeveloperTeamID == teamID;
        }
    }
}
