using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CubeGridGenerator))]
public class CubeGridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CubeGridGenerator generator = (CubeGridGenerator)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Generate Grid"))
        {
            generator.GenerateGridInEditor();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            generator.ClearGrid();
        }
    }
}
