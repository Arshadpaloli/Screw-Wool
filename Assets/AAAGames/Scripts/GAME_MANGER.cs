using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAME_MANGER : MonoBehaviour
{
    public static GAME_MANGER Instance;

    public Vector3 CAM_LAST_POS;
    public int CAM_LAST_ZOOM;

    public GameObject LAST_EFFECT;

    public float ZOOM_SIZE = 0.1F;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        DOTween.SetTweensCapacity(1000, 1000);
        //ZOOM_SIZE = 0.25F;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
