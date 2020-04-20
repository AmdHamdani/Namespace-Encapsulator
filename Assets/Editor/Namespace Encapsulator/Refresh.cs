using UnityEditor;

namespace Tools.Utility {

    public class Refresh {

        public static void Recompile(string reimportedAsset) {
            AssetDatabase.ImportAsset(reimportedAsset);
        }

    }

}