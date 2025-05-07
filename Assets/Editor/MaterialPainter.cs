using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class MaterialPainter : EditorWindow
{
    [System.Serializable]
    private class MaterialData
    {
        public string materialGUID; // Store GUID instead of Material reference
        public ColorType colourType;

        public Material GetMaterial()
        {
            if (!string.IsNullOrEmpty(materialGUID))
            {
                string path = AssetDatabase.GUIDToAssetPath(materialGUID);
                return AssetDatabase.LoadAssetAtPath<Material>(path);
            }
            return null;
        }

        public void SetMaterial(Material material)
        {
            if (material != null)
            {
                materialGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(material));
            }
        }
    }

    [System.Serializable]
    private class MaterialDataContainer
    {
        public List<MaterialData> materials = new List<MaterialData>();
    }

    [SerializeField] private List<MaterialData> selectedMaterials = new List<MaterialData>();
    private bool isPaintingEnabled = false;
    private int selectedMaterialIndex = 0;
    private const string savePath = "Assets/Editor/MaterialPainterData.json";

    [MenuItem("Tools/Material Painter")]
    public static void ShowWindow()
    {
        GetWindow<MaterialPainter>("Material Painter");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        LoadData();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        SaveData();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Materials", EditorStyles.boldLabel);

        for (int i = 0; i < selectedMaterials.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            Material loadedMaterial = selectedMaterials[i].GetMaterial();
            Material newMaterial = (Material)EditorGUILayout.ObjectField($"Material {i + 1}", loadedMaterial, typeof(Material), false);
            if (newMaterial != loadedMaterial) selectedMaterials[i].SetMaterial(newMaterial);

            selectedMaterials[i].colourType = (ColorType)EditorGUILayout.EnumPopup("Colour Type", selectedMaterials[i].colourType);

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                selectedMaterials.RemoveAt(i);
                return;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("+ Add Material"))
        {
            selectedMaterials.Add(new MaterialData());
        }

        if (GUILayout.Button("Save Materials"))
        {
            SaveData();
        }

        isPaintingEnabled = EditorGUILayout.Toggle("Enable Painting", isPaintingEnabled);
        GUILayout.Label($"Selected Material Index: {selectedMaterialIndex}", EditorStyles.boldLabel);

        if (selectedMaterials.Count > 0 && selectedMaterialIndex < selectedMaterials.Count)
        {
            GUILayout.Label($"Selected Colour Type: {selectedMaterials[selectedMaterialIndex].colourType}", EditorStyles.boldLabel);
        }
        Repaint();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e != null && e.type == EventType.ScrollWheel && selectedMaterials.Count > 0)
        {
            selectedMaterialIndex -= (int)Mathf.Sign(e.delta.y);
            selectedMaterialIndex = Mathf.Clamp(selectedMaterialIndex, 0, selectedMaterials.Count - 1);
            e.Use();
            SceneView.RepaintAll();
            Repaint();
        }

        if (!isPaintingEnabled) return;

        if (e.type == EventType.MouseDown && e.button == 0 && selectedMaterials.Count > 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                if (clickedObject.CompareTag("Cap"))
                {
                    ApplyMaterialToHierarchy(clickedObject, selectedMaterialIndex);
                    ApplyColourTypeToComponent(clickedObject, selectedMaterialIndex);
                    SaveData();
                }
            }
            e.Use();
        }
    }

    private void ApplyMaterialToHierarchy(GameObject parent, int materialIndex)
    {
        if (selectedMaterials.Count == 0) return;

        Material selectedMaterial = selectedMaterials[materialIndex].GetMaterial();
        if (selectedMaterial == null)
        {
            Debug.LogError($"Material at index {materialIndex} could not be found!");
            return;
        }

        Renderer[] renderers = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Undo.RecordObject(renderer, "Apply Material");
            Material[] newMaterials = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = selectedMaterial;
            }
            renderer.sharedMaterials = newMaterials;
            EditorUtility.SetDirty(renderer);
        }
    }

    private void ApplyColourTypeToComponent(GameObject parent, int materialIndex)
    {
        EachCap eachCapComponent = parent.GetComponent<EachCap>();
        if (eachCapComponent != null)
        {
            Undo.RecordObject(eachCapComponent, "Apply Colour Type");
            eachCapComponent.CapColor = selectedMaterials[materialIndex].colourType;
            EditorUtility.SetDirty(eachCapComponent);
        }
    }

    private void SaveData()
    {
        MaterialDataContainer container = new MaterialDataContainer { materials = selectedMaterials };
        string json = JsonUtility.ToJson(container, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Materials saved successfully.");
    }

    private void LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            MaterialDataContainer container = JsonUtility.FromJson<MaterialDataContainer>(json);
            selectedMaterials = container.materials ?? new List<MaterialData>();
            Debug.Log("Materials loaded successfully.");
        }
    }
}
