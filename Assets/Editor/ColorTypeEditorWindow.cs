using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ColorTypeEditorWindow : EditorWindow
{
    private ColorType fromColor;
    private ColorType toColor;

    [MenuItem("Tools/Replace ColorType in EachCap")]
    public static void ShowWindow()
    {
        GetWindow<ColorTypeEditorWindow>("Replace ColorType");
    }

    void OnGUI()
    {
        GUILayout.Label("Replace ColorType in EachCap", EditorStyles.boldLabel);

        fromColor = (ColorType)EditorGUILayout.EnumPopup("From Color", fromColor);
        toColor = (ColorType)EditorGUILayout.EnumPopup("To Color", toColor);

        if (GUILayout.Button("Replace"))
        {
            ReplaceColorTypes();
        }
    }

    void ReplaceColorTypes()
    {
        EachCap[] allCaps = FindObjectsOfType<EachCap>();
        int count = 0;

        foreach (EachCap cap in allCaps)
        {
            if (cap.CapColor == fromColor)
            {
                Undo.RecordObject(cap, "Change CapColor");
                cap.CapColor = toColor;
                cap.MY_COLOR();
                EditorUtility.SetDirty(cap);
                count++;
            }
        }

        Debug.Log($"Replaced {count} Cap(s) with color {fromColor} to {toColor}.");
    }
}
