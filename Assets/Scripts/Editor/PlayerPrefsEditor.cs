#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;

// provide the component type for which this inspector UI is required
[CustomEditor(typeof(Toolbox))]
public class PlayerPrefsEditor : Editor
{
    string property;
    int value;
    public override void OnInspectorGUI()
    {
        // will enable the default inpector UI 
        base.OnInspectorGUI();

        // implement your UI code here
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Settings", style, GUILayout.ExpandWidth(true));

        GUILayout.BeginHorizontal();
        property = EditorGUILayout.TextField("Property:", property, GUILayout.ExpandWidth(true));
        value = EditorGUILayout.IntField("Value:", value, GUIStyle.none, GUILayout.ExpandWidth(true));

        GUILayout.EndHorizontal();
        //Button
        if (GUILayout.Button("Player Prefs Update"))
        {
            PlayerPrefs.SetInt(property, value);
        }
    }
}
#endif