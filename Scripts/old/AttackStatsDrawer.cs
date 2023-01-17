using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

//All names are important. This draws GUI on inspector.
[CustomPropertyDrawer(typeof(AttackStats))]
public class AttackStatsDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();

        // Create property fields.
        var nameField = new PropertyField(property.FindPropertyRelative("attackName"), "Attack name");
        var minDisField = new PropertyField(property.FindPropertyRelative("minDistance"), "Testi nimi");
        var maxDisField = new PropertyField(property.FindPropertyRelative("maxDistance"));
        var maxAngleField = new PropertyField(property.FindPropertyRelative("maxAngle"));
        var angleIsPositiveField = new PropertyField(property.FindPropertyRelative("angleIsPositive"));
        var timeStartField = new PropertyField(property.FindPropertyRelative("timeStart"));
        var lenghtField = new PropertyField(property.FindPropertyRelative("attackTimeLenght"));
        // var nameField = new PropertyField(property.FindPropertyRelative("Lenght"), "Fancy Name");

        // Add fields to the container.
        container.Add(nameField);
        container.Add(minDisField);
        container.Add(maxDisField);
        container.Add(maxAngleField);
        container.Add(angleIsPositiveField);
        container.Add(timeStartField);
        container.Add(lenghtField);

        return container;
    }
}

[CustomPropertyDrawer(typeof(AttackStats))]
public class AttackStatsDrawer2 : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects: pos.x, pos.y, width, height. These dont push others out of way so x/y positions must increse
        var nameRect = new Rect(position.x, position.y, 60, position.height);
        var minDisRect = new Rect(position.x + 65, position.y, 33, position.height);
        var maxDisRect = new Rect(position.x + 100, position.y, 33, position.height);
        var angleRect = new Rect(position.x + 135, position.y, 33, position.height);
        var anglePosRect = new Rect(position.x + 175, position.y, 25, position.height);
        var timeRect = new Rect(position.x + 200, position.y, 33, position.height);
        var lenghtRect = new Rect(position.x + 230, position.y, 33, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("attackName"), GUIContent.none);
        EditorGUI.PropertyField(minDisRect, property.FindPropertyRelative("minDistance"), GUIContent.none);
        EditorGUI.PropertyField(maxDisRect, property.FindPropertyRelative("maxDistance"), GUIContent.none);
        EditorGUI.PropertyField(angleRect, property.FindPropertyRelative("maxAngle"), GUIContent.none);
        EditorGUI.PropertyField(anglePosRect, property.FindPropertyRelative("angleIsPositive"), GUIContent.none);
        EditorGUI.PropertyField(timeRect, property.FindPropertyRelative("timeStart"), GUIContent.none);
        EditorGUI.PropertyField(lenghtRect, property.FindPropertyRelative("attackTimeLenght"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}

