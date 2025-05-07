using UnityEditor;
using UnityEngine;

public class CubeShapeGenerator : EditorWindow
{
    GameObject cubePrefab;
    int cubeCount = 10;
    float spacing = 1f;
    PatternType pattern = PatternType.Snake;

    enum PatternType { Straight, LShape, Snake, Spiral }

    [MenuItem("Tools/Cube Shape Generator")]
    public static void ShowWindow()
    {
        GetWindow<CubeShapeGenerator>("Cube Shape Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Cube Shape Settings", EditorStyles.boldLabel);
        cubePrefab = (GameObject)EditorGUILayout.ObjectField("Cube Prefab", cubePrefab, typeof(GameObject), false);
        cubeCount = EditorGUILayout.IntSlider("Cube Count", cubeCount, 1, 100);
        spacing = EditorGUILayout.FloatField("Spacing", spacing);
        pattern = (PatternType)EditorGUILayout.EnumPopup("Pattern", pattern);

        if (GUILayout.Button("Generate Shape"))
        {
            if (cubePrefab == null)
            {
                Debug.LogWarning("Assign a Cube Prefab first!");
                return;
            }

            GenerateShape();
        }
    }

    void GenerateShape()
    {
        GameObject parent = new GameObject("GeneratedShape");

        Vector3 position = Vector3.zero;
        Vector3 direction = Vector3.right;
        int turnCount = 0;

        for (int i = 0; i < cubeCount; i++)
        {
            GameObject cube = (GameObject)PrefabUtility.InstantiatePrefab(cubePrefab);
            cube.transform.position = position;
            cube.transform.SetParent(parent.transform);

            switch (pattern)
            {
                case PatternType.Straight:
                    position += Vector3.right * spacing;
                    break;

                case PatternType.LShape:
                    position += (i < cubeCount / 2 ? Vector3.right : Vector3.forward) * spacing;
                    break;

                case PatternType.Snake:
                    if (i % 5 == 0 && i != 0)
                    {
                        direction = (direction == Vector3.right) ? Vector3.forward : Vector3.right;
                    }
                    position += direction * spacing;
                    break;

                case PatternType.Spiral:
                    if (i % (turnCount + 2) == 0 && i != 0)
                    {
                        turnCount++;
                        switch (turnCount % 4)
                        {
                            case 0: direction = Vector3.right; break;
                            case 1: direction = Vector3.forward; break;
                            case 2: direction = Vector3.left; break;
                            case 3: direction = Vector3.back; break;
                        }
                    }
                    position += direction * spacing;
                    break;
            }
        }

        Selection.activeGameObject = parent;
    }
}
