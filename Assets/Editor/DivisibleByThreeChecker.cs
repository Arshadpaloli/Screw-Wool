using UnityEditor;
using UnityEngine;

public class DivisibleByThreeChecker : EditorWindow
{
    private int numberToCheck = 0;

    [MenuItem("Tools/Divisible By 3 Checker")]
    public static void ShowWindow()
    {
        GetWindow<DivisibleByThreeChecker>("Divisible By 3 Checker");
    }

    private void OnGUI()
    {
        GUILayout.Label("Check if a number is divisible by 3", EditorStyles.boldLabel);

        numberToCheck = EditorGUILayout.IntField("Number", numberToCheck);

        if (GUILayout.Button("Check"))
        {
            CheckDivisibleBy3(numberToCheck);
        }
    }

    private void CheckDivisibleBy3(int number)
    {
        if (number % 3 == 0)
        {
            int result = number / 3;
            Debug.Log($"{number} can be divided by 3. Result: {result}");
        }
        else
        {
            Debug.Log($"{number} cannot be divided by 3 to get a whole number.");
        }
    }
}
