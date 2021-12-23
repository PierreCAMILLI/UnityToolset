using UnityEngine;
using UnityEditor;
using System.Collections;

// Author: Pierre CAMILLI
[CanEditMultipleObjects]
public abstract class RangeEditor<T> : PropertyDrawer where T : System.IComparable<T> {

    protected SerializedProperty _min, _max;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        const uint nbParts = 2;
        const float spaceWidth = 5f;

        // get the Min and Max values
        property.NextVisible(true);
        _min = property.Copy();
        property.NextVisible(true);
        _max = property.Copy();

        Rect contentPosition = EditorGUI.PrefixLabel(position, label, EditorStyles.label);

        if (position.height > 16f)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.indentLevel += 1;
            contentPosition = EditorGUI.IndentedRect(position);
            contentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
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
         GUIContent minContent = EditorGUI.BeginProperty(contentPosition, new GUIContent("Min"), _min);
        {
            EditorGUI.BeginChangeCheck();
            T value = PropertyEditorField(contentPosition, minContent, GetPropertyValue(_min));
            if (EditorGUI.EndChangeCheck())
            {
                SetPropertyValue(_min, SafeMinValue(value));
            }
        }
        EditorGUI.EndProperty();

        contentPosition.x += partWidth + spaceWidth;

        // MAX
        GUIContent maxContent = EditorGUI.BeginProperty(contentPosition, new GUIContent("Max"), _max);
        {
            EditorGUI.BeginChangeCheck();
            T value = PropertyEditorField(contentPosition, maxContent, GetPropertyValue(_max));
            if (EditorGUI.EndChangeCheck())
            {
                SetPropertyValue(_max, SafeMaxValue(value));
            }
        }
        EditorGUI.EndProperty();
    }

    T SafeMinValue(T value)
    {
        T propertyValue = GetPropertyValue(_max);
        if (propertyValue.CompareTo(value) <= 0)
        {
            return propertyValue;
        }
        return value;
    }

    T SafeMaxValue(T value)
    {
        T propertyValue = GetPropertyValue(_min);
        if (propertyValue.CompareTo(value) >= 0)
        {
            return propertyValue;
        }
        return value;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return  Screen.width < 333 ? EditorGUIUtility.singleLineHeight : 16f; // EditorGUIUtility.singleLineHeight
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

[CustomPropertyDrawer(typeof(RangeByte))]
public class RangeEditorByte : RangeEditor<byte>
{
    protected override byte GetPropertyValue(SerializedProperty property)
    {
        return (byte)property.intValue;
    }

    protected override byte PropertyEditorField(Rect position, GUIContent content, byte value)
    {
        return (byte)EditorGUI.IntField(position, content, value);
    }

    protected override void SetPropertyValue(SerializedProperty property, byte value)
    {
        property.intValue = value;
    }
}
