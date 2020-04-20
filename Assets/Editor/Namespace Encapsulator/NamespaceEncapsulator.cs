using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tools.Utility {
    public class NamespaceEncapsulator {

        private string _namespaceName;
        private string _scriptTemplate;
        private bool _verbose;
        private List<string> _files;
        private List<string> _noNamespace = new List<string>();

        public NamespaceEncapsulator(string namespaceName, string templatePath, bool verbose = true) {
            _namespaceName = namespaceName;
            _scriptTemplate = File.ReadAllText(templatePath);
            _verbose = verbose;
        }

        public void ScanFiles(string directory, string extension = "*.cs") {
            Debug.Log("Scan files with extension " + extension);
            _files = Directory
                .EnumerateFiles(directory, extension, SearchOption.AllDirectories)
                .ToList();

            if(!_verbose)
                return;

            foreach(var file in _files) {
                Debug.Log("Find file: " + file);
            }
        }

        public void CheckNamespace() {
            Debug.Log("Check files without namespace.");
            foreach(var file in _files) {
                var lines = File.ReadAllLines(file).ToList();

                var hasNamespace = false;
                foreach(var line in lines) {
                    if(line.Contains("namespace") && !line.Contains("//")) {
                        hasNamespace = true;
                        break;
                    }
                }

                if(!hasNamespace) {
                    _noNamespace.Add(file);
                }
            }

            if(_verbose)
                _noNamespace.ForEach((path) => Debug.Log("File without namespace: " + path));
        }

        public void EncapsulateScript() {
            Debug.Log("Encapsulate script with namepace");
            foreach (var file in _noNamespace) {
                var lines = File.ReadAllLines(file);

                var libraries = new StringBuilder();
                var script = new StringBuilder();

                for(int i = 0; i < lines.Length; i++) {
                    if(lines[i].Contains("using")) {
                        libraries.AppendLine(lines[i]);
                    } else if(lines[i].Length > 0) {
                        script.AppendLine(lines[i]);
                    }
                }

                var formattedScript = FormatScript(libraries.ToString(), script.ToString());
                
                OverwriteScript(file, formattedScript);

                if(_verbose)
                    Debug.Log(formattedScript);
            }

            Debug.Log("Finish encapsulate script with namespace");
            
            if(!_verbose)
                return;

            foreach(var file in _noNamespace) {
                Debug.Log("Updated script: " + file);
            }
        }

        private string FormatScript(string libraries, string script) {
            var text = _scriptTemplate;
            text = text.Replace("__USING__", libraries);
            text = text.Replace("__NAMESPACE__", _namespaceName);
            text = text.Replace("__SCRIPT__", script);

            return text;
        }

        private void OverwriteScript(string file, string script) {
            File.WriteAllText(file, script);
        }
    }
}
