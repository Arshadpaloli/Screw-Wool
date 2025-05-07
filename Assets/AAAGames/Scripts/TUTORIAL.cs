using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUTORIAL : MonoBehaviour
{
    public static TUTORIAL instance;

    public GameObject FIRST_TEXT;
    public GameObject SECOND_TEXT;

    public GameObject FIRST_HAND;
    public GameObject SECOND_HAND;

    public int TOUCHES;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FIRST_HAND_OFF()
    {
        FIRST_HAND.SetActive(false);
        SECOND_HAND.SetActive(true);

        FIRST_TEXT.SetActive(false);
        SECOND_TEXT.SetActive(true);
    }

    public void SECOND_HAND_OFF()
    {
        SECOND_HAND.SetActive(false);
        SECOND_TEXT.SetActive(false);
    }
}
