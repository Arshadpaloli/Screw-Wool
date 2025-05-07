using UnityEngine;
using UnityEditor;
using System.Linq;

public class BallEditor : EditorWindow
{
    // Number of balls to change
    private int numberOfrollsToChange = 5;

    // Enum value to set
    public ColorType newrollType;

    // Create the window
    [MenuItem("Window/Ball Enum Changer")]
    public static void ShowWindow()
    {
        GetWindow<BallEditor>("Ball Enum Changer");
    }

    // Window GUI
    private void OnGUI()
    {
        GUILayout.Label("Change Ball Enum", EditorStyles.boldLabel);

        // Input field for the number of balls and new enum type
        numberOfrollsToChange = EditorGUILayout.IntField("Number of Balls", numberOfrollsToChange);
        newrollType = (ColorType)EditorGUILayout.EnumPopup("New Ball Type", newrollType);

        // Button to trigger the change
        if (GUILayout.Button("Change Enum for Balls"))
        {
            ChangeBallEnum();
        }
    }

    // Method to change the enum for the balls
    private void ChangeBallEnum()
    {
        // Find all GameObjects tagged as "Ball"
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Cap");

        // Shuffle the array randomly
        System.Random rng = new System.Random();
        balls = balls.OrderBy(x => rng.Next()).ToArray();

        // Loop through the balls and change the enum for the specified number
        int count = 0;
        foreach (GameObject ball in balls)
        {
            if (count >= numberOfrollsToChange)
                break;

            EachCap ballComponent = ball.GetComponent<EachCap>();
            if (ballComponent != null && ballComponent.CapColor == ColorType.None)
            {
                ballComponent.CapColor = newrollType;  // Assuming the Ball script uses this enum
                ballComponent.MY_COLOR();
                EditorUtility.SetDirty(ballComponent); // Mark object as dirty to save changes
                count++;
            }
        }

        Debug.Log($"{count} ball(s) enum changed to {newrollType}");
    }
}
