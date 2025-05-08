using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ReconnectPrefabDingzi01 : EditorWindow
{
    private static string prefabPath = "Assets/AAAGames/Prefab/Dingzi01.prefab";

    [MenuItem("Tools/Reconnect Dingzi01 to Prefab")]
    public static void ReconnectPrefabs()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError("Prefab not found at: " + prefabPath);
            return;
        }

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "Dingzi01")
            {
                GameObject source = PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj);
                if (source == null)
                {
                    PrefabUtility.UnpackPrefabInstance(obj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                    PrefabUtility.SaveAsPrefabAssetAndConnect(obj, prefabPath, InteractionMode.AutomatedAction);
                    count++;
                }
            }
        }

        Debug.Log($"Reconnected {count} Dingzi01 objects to prefab.");
    }
}