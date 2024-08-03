using System.Collections.Generic;
using UnityEngine;

namespace KingKode.CustomBuildTool {
    
    public class BuildConfig : ScriptableObject {

        public string RelativeBuildFolderPath = "Builds";
        public string ReletiveKeyStorePassPath = "Builds/KeyStorePass.txt";

        public List<BuildInfo> BuildInfos;
    }
}