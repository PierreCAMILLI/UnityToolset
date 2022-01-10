using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Toolset
{
    namespace Persistent
    {
        [CustomEditor(typeof(Location), true)]
        public class PersistentLocationEditor : Editor
        {
            SerializedProperty _fileNameProp;
            SerializedProperty _fileExtensionProp;
            SerializedProperty _pathProp;

            private void OnEnable()
            {
                _fileNameProp = serializedObject.FindProperty("_fileName");
                _fileExtensionProp = serializedObject.FindProperty("_fileExtension");
                _pathProp = serializedObject.FindProperty("_path");
            }

            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                GUILayout.Space(10f);

                Location persistent = (Location)target;

                GUIStyle labelStyle = GUI.skin.label;
                labelStyle.fontStyle = FontStyle.Bold;
                GUILayout.Label("Full Path: ");
                labelStyle.fontStyle = FontStyle.Normal;

                labelStyle.wordWrap = true;
                if (_fileNameProp.stringValue.Length > 0 && _fileExtensionProp.stringValue.Length > 0)
                {
                    EditorGUILayout.SelectableLabel(persistent.DefaultFullPath, labelStyle);
                }
                else
                {
                    Color baseColor = labelStyle.normal.textColor;

                    labelStyle.fontStyle = FontStyle.Bold;
                    labelStyle.normal.textColor = Color.red;
                    GUILayout.Label("Path isn't valid! Filename and file extention need to be filled.");
                    labelStyle.fontStyle = FontStyle.Normal;
                    labelStyle.normal.textColor = baseColor;
                }

                if (GUILayout.Button("Delete Save File"))
                {
                    if (persistent.Delete())
                    {
                        Debug.Log(persistent.FullFilename + " file successfully deleted.");
                    }
                    else
                    {
                        Debug.LogWarning("No save file named " + persistent.FullFilename + " exists.");
                    }
                }
            }
        }
    }
}
