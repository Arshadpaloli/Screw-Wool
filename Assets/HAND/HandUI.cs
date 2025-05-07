using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandUI : MonoBehaviour
{
    private Transform handTransform;
    private Image hand;
    public Vector2 offset;
    public Sprite idle;
    public Sprite click;

    void Start()
    {
        handTransform = transform.GetChild(0);
        hand = handTransform.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        handTransform.position = Input.mousePosition + new Vector3(offset.x,offset.y);

        if (Input.GetMouseButtonDown(0))hand.sprite = click;
        else if (Input.GetMouseButtonUp(0))hand.sprite = idle;
    }
}
