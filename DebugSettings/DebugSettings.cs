using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[System.Serializable]
public class DebugSettings
{
    private static DebugSettings _current;
    /// <summary>
    /// Returns current Debug Settings used for debugging
    /// </summary>
    /// <remarks>Use <b>#if UNITY_EDITOR</b> directive when using this variable</remarks>
    public static DebugSettings Current
    {
        get
        {
            if (_current == null)
            {
                string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(DebugSettingsSerialized)}", new[] { "Assets" });
                if (guids.Length > 0)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                    _current = UnityEditor.AssetDatabase.LoadAssetAtPath<DebugSettingsSerialized>(path).SelectedSetting;
                }
            }
            return _current;
        }
        set => _current = value;
    }

    [Header("Example Parameters")]

    [SerializeField]
    private int _intValue;
    public int IntValue { get => _intValue; }

    [SerializeField]
    private float _floatValue;
    public float FloatValue { get => _floatValue; }

    [SerializeField]
    private string _stringValue;
    public string StringValue { get => _stringValue; }
}
#endif
