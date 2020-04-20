using UnityEngine;
using UnityEditor;

namespace Tools.Utility {
    public class NamespaceEncapsulatorWindow : EditorWindow {

    private string namespaceName;
    private string path;
    private bool verbose;

    [MenuItem("Utility/Namespace Encapsulator/Encapsulate Scripts")]
    private static void ShowWindow() {
        var window = GetWindow<NamespaceEncapsulatorWindow>();
        window.titleContent = new GUIContent("NamespaceEncapsulatorWindow");
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        NamespaceField();
        DirectoryField();
        VerboseLogToggle();
        EncapsulateButton();
        EditorGUILayout.EndVertical();
    }

    private void NamespaceField()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Namespace: ", GUILayout.Width(100));
        GUILayout.Space(10);
        namespaceName = GUILayout.TextField(namespaceName);
        GUILayout.EndHorizontal();
    }

    private void DirectoryField() {
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Directory: ", GUILayout.Width(100));
        GUILayout.Space(10);
        if(string.IsNullOrEmpty(path)) {
            if(GUILayout.Button("Browse")) {
                path = EditorUtility.OpenFolderPanel("Directory", Application.dataPath, "");
            }
        } else {
            GUILayout.Label(path);
        }
        GUILayout.EndHorizontal();
    }

    private void VerboseLogToggle() {
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Verbose Log: ", GUILayout.Width(100));
        GUILayout.Space(10);
        verbose = EditorGUILayout.Toggle(verbose);
        GUILayout.EndHorizontal();
    }

    private void EncapsulateButton() {
        GUILayout.Space(10);
        if(GUILayout.Button("Encapsulate")) {
            // var directory = System.IO.Path.GetFullPath("Assets/Namespace Encapsulator/Test/");
            var templatePath = System.IO.Path.GetFullPath("Assets/Namespace Encapsulator/Templates/Script.txt");
            var nsEncapsulator = new NamespaceEncapsulator(namespaceName, templatePath, verbose);
            nsEncapsulator.ScanFiles(path);
            nsEncapsulator.CheckNamespace();
            nsEncapsulator.EncapsulateScript();
            AssetDatabase.Refresh();
        }
    }
}
}
