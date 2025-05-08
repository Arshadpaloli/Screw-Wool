using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Button3DEvent : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onClick;
    public void OnMouseDown()
    {
        // if(UI_MANAGER.Instance.FAIL_PANEL.gameObject.activeInHierarchy==false)
        // {
            Debug.Log("3D Button Pressed!");
            onClick?.Invoke(); // Safely invoke the event
        // }
    }
}