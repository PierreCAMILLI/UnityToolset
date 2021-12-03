using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugSettingsWindow : EditorWindow
{
    private static DebugSettingsSerialized _settings;
    private static List<string> _settingNames;

    private SerializedObject _serializedObject;
    private SerializedProperty _selectedProperty;

    [MenuItem("Window/Debug Settings Editor")]
    static void Init()
    {
        Open(null);
    }

    public static void Open(DebugSettingsSerialized settings)
    {
        // DebugSettingsWindow window = (DebugSettingsWindow)EditorWindow.GetWindow(typeof(DebugSettingsWindow));
        DebugSettingsWindow window = GetWindow<DebugSettingsWindow>("Debug Settings Editor");

        if (settings != null)
        {
            _settings = settings;
        }
        else
        {
            FetchSettings();
        }

        if (_settings != null)
        {
            if (_settingNames == null)
            {
                _settingNames = new List<string>();
            }
            else
            {
                _settingNames.Clear();
            }

            _settingNames.Add("None");
            if (_settings != null)
            {
                if (_settings.DebugSettingNames == null)
                {
                    _settings.Init();
                }
                _settingNames.AddRange(_settings.DebugSettingNames);
            }
        }

        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Debug Settings Editor", EditorStyles.boldLabel);

        if (!FetchSettings())
        {
            GUILayout.Label("Create a Debug Settings Serialized object before using the Debug Settings Editor.");
        }
        else
        {
            _serializedObject = new SerializedObject(_settings);

            int index = EditorGUILayout.Popup("Setting", _settings.popupIndex, _settingNames.ToArray());

            SerializedProperty properties = _serializedObject.FindProperty("_debugSettings");
            SelectSetting(properties, index);

            if (_settings.popupIndex != 0)
            {
                RenameSetting(_settings.popupIndex, EditorGUILayout.TextField("Name", _settingNames[_settings.popupIndex]));

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
                    _settings.popupIndex = RemoveSetting(_settings.popupIndex);
                }
            }

            if (GUILayout.Button("(+) Add Setting"))
            {
                GUI.FocusControl(null);
                _settings.popupIndex = AddSetting();
            }
            _serializedObject.ApplyModifiedProperties();
        }
    }

    private static bool FetchSettings()
    {
        if (_settings == null)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(DebugSettingsSerialized)}" , new[] { "Assets" } );
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                _settings = AssetDatabase.LoadAssetAtPath<DebugSettingsSerialized>(path);
            }
            if (_settings != null)
            {
                Open(_settings);
            }
        }

        return _settings != null;
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

    protected void SelectSetting(SerializedProperty properties, int popupIndex)
    {
        DebugSettings.Current = popupIndex == 0 ? null : _settings.DebugSettings[popupIndex - 1];
        _settings.popupIndex = popupIndex;

        if (popupIndex > 0)
        {
            _selectedProperty = properties.GetArrayElementAtIndex(popupIndex - 1);
        }
        _serializedObject.ApplyModifiedProperties();
    }

    protected void RenameSetting(int popupIndex, string settingName)
    {
        _settingNames[popupIndex] = settingName;
        _settings.DebugSettingNames[popupIndex - 1] = settingName;
    }

    protected int RemoveSetting(int popupIndex)
    {
        _settingNames.RemoveAt(popupIndex);
        _settings.RemoveSetting(popupIndex - 1);
        _serializedObject.ApplyModifiedProperties();
        if (popupIndex >= _settingNames.Count)
        {
            return popupIndex - 1;
        }
        _serializedObject.ApplyModifiedProperties();
        return popupIndex;
    }

    protected int AddSetting()
    {
        _settingNames.Add("New Setting " + _settingNames.Count);
        _settings.AddSetting("New Setting " + _settingNames.Count);
        _serializedObject.ApplyModifiedProperties();
        return _settings.DebugSettings.Count;
    }
}
