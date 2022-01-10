using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Toolset
{
    public class DebugSettingsWindow : EditorWindow, IHasCustomMenu
    {
        private const string WindowsName = "Debug Settings Editor";

        [SerializeField]
        private MonoScript _script;

        [SerializeField]
        private List<string> _settingNames = new List<string>();
        public IList<string> SettingNames
        {
            get
            {
                if (_settingNames == null)
                {
                    _settingNames = new List<string>();
                }
                if (_settingNames.Count == 0)
                {
                    _settingNames.Add("None");
                }
                return _settingNames;
            }
        }
        [SerializeReference]
        [SerializeField]
        private List<DebugSetting> _debugSettings = new List<DebugSetting>();
        public IList<DebugSetting> DebugSettings
        {
            get
            {
                if (_debugSettings == null)
                {
                    _debugSettings = new List<DebugSetting>();
                }
                return _debugSettings;
            }
        }

        [SerializeField]
        private int _popupIndex = 0;

        private SerializedObject _serializedObject;
        private SerializedProperty _selectedProperty;

        public DebugSetting SelectedSetting
        {
            get
            {
                if (_popupIndex >= 1)
                {
                    return DebugSettings[_popupIndex - 1];
                }
                return null;
            }
        }

        [MenuItem("Window/Debug Settings Editor")]
        static void Init()
        {
            Open();
        }

        public static void Open()
        {
            DebugSettingsWindow window = GetWindow<DebugSettingsWindow>(WindowsName);

            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("Debug Settings Editor", EditorStyles.boldLabel);

            _serializedObject = new SerializedObject(this);

            MonoScript newScript = EditorGUILayout.ObjectField("Debug Script", _script, typeof(MonoScript), false) as MonoScript;

            if (newScript != _script)
            {
                if (newScript == null || newScript.GetClass().IsSubclassOf(typeof(DebugSetting)))
                {
                    ClearAll();
                    _script = newScript;

                    _serializedObject.ApplyModifiedProperties();
                }
            }

            if (_script != null)
            {
                EditorGUILayout.Space();

                string[] array = new string[SettingNames.Count];
                SettingNames.CopyTo(array, 0);
                int index = EditorGUILayout.Popup("Setting", _popupIndex, array);

                SelectSetting(index);

                if (_popupIndex > 0)
                {
                    RenameSetting(_popupIndex, EditorGUILayout.TextField("Name", SettingNames[_popupIndex]));

                    EditorGUILayout.Space();

                    GUILayout.Label("Setting Parameters", EditorStyles.boldLabel);
                    if (_selectedProperty != null)
                    {
                        EditorGUI.indentLevel++;
                        DrawProperties(_selectedProperty, true);
                        EditorGUI.indentLevel--;
                    }

                    EditorGUILayout.Space();

                    if (GUILayout.Button("(-) Remove Setting"))
                    {
                        GUI.FocusControl(null);
                        _popupIndex = RemoveSetting(_popupIndex);
                    }
                }

                if (GUILayout.Button("(+) Add Setting"))
                {
                    GUI.FocusControl(null);
                    _popupIndex = AddSetting();
                }

                _serializedObject.ApplyModifiedProperties();
            }
        }

        protected void DrawProperties(SerializedProperty prop, bool drawChildren)
        {
            string lastPropPath = string.Empty;
            foreach (SerializedProperty p in prop)
            {
                if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUILayout.BeginHorizontal();
                    p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (p.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        DrawProperties(p, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath))
                    {
                        continue;
                    }
                    lastPropPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        }

        protected void SelectSetting(int popupIndex)
        {
            DebugSetting.Current = popupIndex == 0 ? null : DebugSettings[popupIndex - 1];
            _popupIndex = popupIndex;

            if (popupIndex > 0)
            {
                SerializedProperty property = _serializedObject.FindProperty("_debugSettings");
                if (property != null)
                {
                    _selectedProperty = property.GetArrayElementAtIndex(popupIndex - 1);
                }
            }
        }

        protected void RenameSetting(int popupIndex, string settingName)
        {
            SettingNames[popupIndex] = settingName;
        }

        protected int RemoveSetting(int popupIndex)
        {
            SettingNames.RemoveAt(popupIndex);
            DebugSettings.RemoveAt(popupIndex - 1);
            _serializedObject.ApplyModifiedProperties();
            if (popupIndex >= SettingNames.Count)
            {
                return popupIndex - 1;
            }
            return popupIndex;
        }

        protected int AddSetting()
        {
            SettingNames.Add("New Setting " + SettingNames.Count);
            DebugSetting setting = System.Activator.CreateInstance(_script.GetClass()) as DebugSetting;
            DebugSettings.Add(setting);
            return DebugSettings.Count;
        }

        public void ClearAll()
        {
            _script = null;
            _settingNames.Clear();
            _debugSettings.Clear();
            _popupIndex = 0;
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Clear All"), false, ClearAll);
        }
    }
}
