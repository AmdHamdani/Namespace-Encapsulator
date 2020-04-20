using UnityEditor;

namespace Tools.Utility {

    public class Runner {
        
        // [MenuItem("Utility/Namespace Encapsulator/Encapsulate Scripts")]
        public static void Run() {
            var directory = System.IO.Path.GetFullPath("Assets/Namespace Encapsulator/Test/");
            var templatePath = System.IO.Path.GetFullPath("Assets/Namespace Encapsulator/Templates/Script.txt");
            var nsEncapsulator = new NamespaceEncapsulator("MyNameSpace", templatePath, false);
            nsEncapsulator.ScanFiles(directory);
            nsEncapsulator.CheckNamespace();
            nsEncapsulator.EncapsulateScript();
            AssetDatabase.Refresh();
        }

    }
}