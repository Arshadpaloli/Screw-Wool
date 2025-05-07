using UnityEngine;
using UnityEditor;

public class customshortcut : Editor
{
    private static Vector3 copiedPosition; // Store copied position
    private static bool positionCopied = false; // Track if a position has been copied

    // Shift + A to select parent
    [MenuItem("Tools/Select Parent #a", false, 0)] 
    static void SelectParent()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length > 0)
        {
            var newSelection = new System.Collections.Generic.List<GameObject>();

            foreach (GameObject selectedObject in selectedObjects)
            {
                if (selectedObject.transform.parent != null)
                {
                    newSelection.Add(selectedObject.transform.parent.gameObject);
                }
                else
                {
                    Debug.Log($"'{selectedObject.name}' has no parent.");
                }
            }

            if (newSelection.Count > 0)
            {
                Selection.objects = newSelection.ToArray();
            }
            else
            {
                Debug.Log("None of the selected objects have parents.");
            }
        }
        else
        {
            Debug.Log("No objects selected.");
        }
    }

    // Shortcut C to lock/unlock the Inspector
    [MenuItem("Tools/Toggle Inspector Lock _c", false, 0)] 
    static void ToggleInspectorLock()
    {
        var inspectorWindowType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
        if (inspectorWindowType == null)
        {
            Debug.LogError("Failed to find InspectorWindow type.");
            return;
        }

        EditorWindow[] inspectorWindows = Resources.FindObjectsOfTypeAll(inspectorWindowType) as EditorWindow[];
        if (inspectorWindows == null || inspectorWindows.Length == 0)
        {
            Debug.LogError("No Inspector windows found.");
            return;
        }

        foreach (var inspectorWindow in inspectorWindows)
        {
            var isLockedProperty = inspectorWindowType.GetProperty("isLocked", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (isLockedProperty == null)
            {
                Debug.LogError("Failed to access the 'isLocked' property.");
                continue;
            }

            bool isLocked = (bool)isLockedProperty.GetValue(inspectorWindow, null);
            isLockedProperty.SetValue(inspectorWindow, !isLocked, null); // Toggle lock

            inspectorWindow.Repaint();
        }
    }

    // Shortcut Shift + Q to collapse children
    [MenuItem("Tools/Collapse Hierarchy #q", false, 0)] 
    static void CollapseHierarchy()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject selectedObject in selectedObjects)
        {
            if (selectedObject.transform.childCount > 0)
            {
                SetExpanded(selectedObject, false); // Collapse
            }
        }
    }

    // Shortcut Shift + W to expand children
    [MenuItem("Tools/Expand Hierarchy #w", false, 0)] 
    static void ExpandHierarchy()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject selectedObject in selectedObjects)
        {
            if (selectedObject.transform.childCount > 0)
            {
                SetExpanded(selectedObject, true); // Expand
            }
        }
    }

    // Shortcut Alt + W to copy/paste the transform position
    [MenuItem("Tools/Copy/Paste Position _&w", false, 0)] 
    static void CopyPastePosition()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length > 0)
        {
            if (!positionCopied)
            {
                // Copy the position
                copiedPosition = selectedObjects[0].transform.position;
                positionCopied = true;
                // Debug.Log($"Position copied: {copiedPosition}");
            }
            else
            {
                // Paste the position and support Undo/Redo
                foreach (GameObject selectedObject in selectedObjects)
                {
                    Undo.RecordObject(selectedObject.transform, "Paste Position"); // Record for undo/redo
                    selectedObject.transform.position = copiedPosition;
                    // Debug.Log($"Position pasted to: {selectedObject.name}");
                }
                positionCopied = false; // Reset after pasting
            }
        }
        else
        {
            Debug.Log("No objects selected to copy/paste position.");
        }
    }

    // Shortcut Shift + Z to delete all children
    [MenuItem("Tools/Delete All Children #z", false, 0)] 
    static void DeleteAllChildren()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length > 0)
        {
            foreach (GameObject selectedObject in selectedObjects)
            {
                int childCount = selectedObject.transform.childCount;
                for (int i = childCount - 1; i >= 0; i--) // Iterate backwards to avoid modifying the collection
                {
                    Transform child = selectedObject.transform.GetChild(i);
                    Undo.DestroyObjectImmediate(child.gameObject); // Support undo/redo
                }
            }
            Debug.Log("Deleted all children of selected objects.");
        }
        else
        {
            Debug.Log("No objects selected.");
        }
    }

    // Shortcut Shift + X to clear console
    [MenuItem("Tools/Clear Console #x", false, 0)] 
    static void ClearConsole()
    {
        var assembly = typeof(Editor).Assembly;
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
        // Debug.Log("Console cleared.");
    }

    // Expands or collapses a GameObject's hierarchy in the editor
    static void SetExpanded(GameObject go, bool expand)
    {
        // Get the active editor window focused on the hierarchy
        EditorWindow hierarchyWindow = GetHierarchyWindow();

        if (hierarchyWindow != null)
        {
            // Select the object in the hierarchy
            Selection.activeGameObject = go;

            // Send a key event for left (collapse) or right (expand) arrow keys
            Event e = new Event
            {
                keyCode = expand ? KeyCode.RightArrow : KeyCode.LeftArrow,
                type = EventType.KeyDown
            };

            hierarchyWindow.SendEvent(e);
        }
    }

    // Gets the current hierarchy window
    static EditorWindow GetHierarchyWindow()
    {
        var hierarchyWindowType = typeof(Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
        if (hierarchyWindowType != null)
        {
            EditorWindow[] windows = Resources.FindObjectsOfTypeAll(hierarchyWindowType) as EditorWindow[];
            if (windows != null && windows.Length > 0)
            {
                return windows[0]; // Return the first hierarchy window
            }
        }
        return null;
    }
}
