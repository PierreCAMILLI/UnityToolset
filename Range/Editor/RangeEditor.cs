using UnityEngine;
using UnityEditor;
using System.Collections;

// Author: Pierre CAMILLI
[CanEditMultipleObjects]
public abstract class RangeEditor<T> : PropertyDrawer where T : System.IComparable<T> {

    protected SerializedProperty Min, Max;
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
            T value = PropertyEditorField(contentPosition, new GUIContent("Min"), GetPropertyValue(Min));
            if (EditorGUI.EndChangeCheck())
            {
                SetPropertyValue(Min, SafeMinValue(value));
            }
        }
        EditorGUI.EndProperty();

        contentPosition.x += partWidth + spaceWidth;

        // MAX
        EditorGUI.BeginProperty(contentPosition, label, Max);
        {
            EditorGUI.BeginChangeCheck();
            T value = PropertyEditorField(contentPosition, new GUIContent("Max"), GetPropertyValue(Max));
            if (EditorGUI.EndChangeCheck())
            {
                SetPropertyValue(Max, SafeMaxValue(value));
            }
        }
        EditorGUI.EndProperty();
    }

    T SafeMinValue(T value)
    {
        T propertyValue = GetPropertyValue(Max);
        if (propertyValue.CompareTo(value) <= 0)
        {
            return propertyValue;
        }
        return value;
    }

    T SafeMaxValue(T value)
    {
        T propertyValue = GetPropertyValue(Min);
        if (propertyValue.CompareTo(value) >= 0)
        {
            return propertyValue;
        }
        return value;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return Screen.width < 333 ? (16f + 18f) : 16f;
    }

    protected abstract T GetPropertyValue(SerializedProperty property);

    protected abstract void SetPropertyValue(SerializedProperty property, T value);

    protected abstract T PropertyEditorField(Rect position, GUIContent content, T value);
}

[CustomPropertyDrawer(typeof(Range))]
public class RangeEditorFloat : RangeEditor<float>
{
    protected override float GetPropertyValue(SerializedProperty property)
    {
        return property.floatValue;
    }

    protected override float PropertyEditorField(Rect position, GUIContent content, float value)
    {
        return EditorGUI.FloatField(position, content, value);
    }

    protected override void SetPropertyValue(SerializedProperty property, float value)
    {
        property.floatValue = value;
    }
}

[CustomPropertyDrawer(typeof(RangeInt))]
public class RangeEditorInt : RangeEditor<int>
{
    protected override int GetPropertyValue(SerializedProperty property)
    {
        return property.intValue;
    }

    protected override int PropertyEditorField(Rect position, GUIContent content, int value)
    {
        return EditorGUI.IntField(position, content, value);
    }

    protected override void SetPropertyValue(SerializedProperty property, int value)
    {
        property.intValue = value;
    }
}
