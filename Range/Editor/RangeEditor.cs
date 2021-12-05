using UnityEngine;
using UnityEditor;
using System.Collections;

// Author: Pierre CAMILLI

[CanEditMultipleObjects]
[CustomPropertyDrawer(typeof(Range))]
public class RangeEditor : PropertyDrawer {

    SerializedProperty Min, Max, Restrict;
    string name;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        const uint nbParts = 2;
        const float spaceWidth = 5f;

        // get the name before it's gone
        name = property.displayName;

        // get the Min and Max values
        property.NextVisible(true);
        Min = property.Copy();
        property.NextVisible(true);
        Max = property.Copy();

        Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(name));

        if(position.height > 16f)
        {
            position.height = 16f;
            EditorGUI.indentLevel += 1;
            contentPosition = EditorGUI.IndentedRect(position);
            contentPosition.y += 18f;
        }

        float partWidth = (contentPosition.width / nbParts) - (spaceWidth * 0.5f * (nbParts - 1)) ;
        GUI.skin.label.padding = new RectOffset(6, 3, 6, 6);

        // show the Min and Max
        EditorGUIUtility.labelWidth = 28f;
        contentPosition.width = partWidth;
        EditorGUI.indentLevel = 0;

        // Begin/end property & change check make each field
        // Behave correctly when multi-object editing.
        // MIN
        EditorGUI.BeginProperty(contentPosition, label, Min);
        {
            EditorGUI.BeginChangeCheck();
            float value = EditorGUI.FloatField(contentPosition, new GUIContent("Min"), Min.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                Min.floatValue = MinValue(value);
            }
        }
        EditorGUI.EndProperty();

        contentPosition.x += partWidth + spaceWidth;

        // MAX
        EditorGUI.BeginProperty(contentPosition, label, Max);
        {
            EditorGUI.BeginChangeCheck();
            float value = EditorGUI.FloatField(contentPosition, new GUIContent("Max"), Max.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                Max.floatValue = MaxValue(value);
            }
        }
        EditorGUI.EndProperty();
    }

    float MinValue(float value)
    {
        if (Max.floatValue < value)
        {
            return Max.floatValue;
        }
        else
        {
            return value;
        }
    }

    float MaxValue(float value)
    {
        if (Min.floatValue > value)
        {
            return Min.floatValue;
        }
        else
        {
            return value;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return Screen.width < 333 ? (16f + 18f) : 16f;
    }

}
