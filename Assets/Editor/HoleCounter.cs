using UnityEditor;
using UnityEngine;

public class HoleCounter : EditorWindow
{
    public ColorType targetROLLCOLOR;

    [MenuItem("Tools/Hole Counter")]
    public static void ShowWindow()
    {
        GetWindow<HoleCounter>("Hole Counter");
    }

    private void OnGUI()
    {
        // Dropdown to select Enum value
        targetROLLCOLOR = (ColorType)EditorGUILayout.EnumPopup("Select Hole Type:", targetROLLCOLOR);

        if (GUILayout.Button("Count Holes"))
        {
            CountHoles();
        }
    }

    private void CountHoles()
    {
        // Find all GameObjects with the 'HOLES' tag
        GameObject[] holes = GameObject.FindGameObjectsWithTag("Cap");

        int count = 0;

        foreach (GameObject hole in holes)
        {
            // Assume each 'hole' GameObject has a component with the Enum (adjust as necessary)
            var holeComponent = hole.GetComponent<EachCap>(); // Replace with your component name
            if (holeComponent != null && holeComponent.CapColor == targetROLLCOLOR)
            {
                count++;
            }
        }

        Debug.Log($"Count of {targetROLLCOLOR} HOLES: {count}");
    }
}
