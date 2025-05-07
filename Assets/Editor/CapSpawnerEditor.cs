using UnityEngine;
using UnityEditor;
using System.Linq;

public class CapSpawnerEditor : EditorWindow
{
    private GameObject prefabToSpawn;
    private int numberToSpawn = 5;
    //private Vector3 spawnOffset = new Vector3(2, 0, 0);
    private ColorType newCapType;
    private Transform parentTransform; // Parent object for spawned prefabs

    [MenuItem("Tools/Cap Spawner & Enum Setter")]
    public static void ShowWindow()
    {
        GetWindow<CapSpawnerEditor>("Cap Spawner & Enum Setter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Cap Spawner Settings", EditorStyles.boldLabel);

        prefabToSpawn = (GameObject)EditorGUILayout.ObjectField("Prefab to Spawn", prefabToSpawn, typeof(GameObject), false);
        numberToSpawn = EditorGUILayout.IntField("Number of Caps", numberToSpawn);
        //spawnOffset = EditorGUILayout.Vector3Field("Spawn Offset", spawnOffset);
        newCapType = (ColorType)EditorGUILayout.EnumPopup("Set Cap Enum", newCapType);
        parentTransform = (Transform)EditorGUILayout.ObjectField("Parent Transform", parentTransform, typeof(Transform), true);

        if (GUILayout.Button("Spawn & Set Enum"))
        {
            if (prefabToSpawn == null || numberToSpawn <= 0)
            {
                Debug.LogWarning("Assign a valid prefab and a positive number.");
                return;
            }

            SpawnAndAssign();
        }
    }

    private void SpawnAndAssign()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            GameObject newCap = (GameObject)PrefabUtility.InstantiatePrefab(prefabToSpawn);

            // Set parent
            if (parentTransform != null)
                newCap.transform.SetParent(parentTransform, false);

            // Set fixed position
            newCap.transform.localPosition = new Vector3(-8.500001f, 0.7822722f, -0.6229369f);

            // Set fixed rotation
            newCap.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);

            // Rename with numbering in brackets
            newCap.name = $"{prefabToSpawn.name} ({i + 1})";

            Crate capComponent = newCap.GetComponent<Crate>();
            if (capComponent != null && capComponent.CrateColor == ColorType.None)
            {
                capComponent.CrateColor = newCapType;
                capComponent.MY_COLOR();
                EditorUtility.SetDirty(capComponent);
            }

            Undo.RegisterCreatedObjectUndo(newCap, "Spawn Cap");
        }

        Debug.Log($"{numberToSpawn} cap(s) spawned with rotation (0, 90, 0) and renamed under {parentTransform?.name ?? "World"}");
    }



}
