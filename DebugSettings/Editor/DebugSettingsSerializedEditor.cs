using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetHandler
{
    [UnityEditor.Callbacks.OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line)
    {
        DebugSettingsSerialized debugSettings = EditorUtility.InstanceIDToObject(instanceId) as DebugSettingsSerialized;
        if (debugSettings != null)
        {
            DebugSettingsWindow.Open(debugSettings);
            return true;
        }
        return false;
    }
}

[CustomEditor(typeof(DebugSettingsSerialized))]
public class DebugSettingsSerializedEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Debug Settings Editor"))
        {
            DebugSettingsWindow.Open((DebugSettingsSerialized)target);
        }
    }
}
