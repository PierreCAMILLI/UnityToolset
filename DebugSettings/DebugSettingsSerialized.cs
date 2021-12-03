using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[CreateAssetMenu(fileName = "Debug Settings", menuName = "Debug/Settings")]
public class DebugSettingsSerialized : ScriptableObject
{
    [SerializeField]
    private List<string> _debugSettingNames;
    public IList<string> DebugSettingNames { get { return _debugSettingNames; } }

    [SerializeField]
    private List<DebugSettings> _debugSettings;
    public IList<DebugSettings> DebugSettings { get { return _debugSettings; } }

    public int popupIndex = 0;

    public DebugSettings SelectedSetting
    {
        get => popupIndex > 0 ? _debugSettings[popupIndex - 1] : null;
    }

    public void Init()
    {
        _debugSettingNames = new List<string>();
        _debugSettings = new List<DebugSettings>();
    }

    public void RemoveSetting(int index)
    {
        _debugSettingNames.RemoveAt(index);
        _debugSettings.RemoveAt(index);
    }

    public void AddSetting(string settingName)
    {
        if (_debugSettingNames == null)
        {
            _debugSettingNames = new List<string>();
        }
        _debugSettingNames.Add(settingName);

        if (_debugSettings == null)
        {
            _debugSettings = new List<DebugSettings>();
        }
        _debugSettings.Add(new DebugSettings());
    }
}
#endif
