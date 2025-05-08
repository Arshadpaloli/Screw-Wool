using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EachCap : MonoBehaviour
{
    public ColorType CapColor;
    public GameObject TOUCH_EFFECT;
    public MeshRenderer _OwnMeshRenderer;
    // public List<GameObject> THREAD_ROLLS;
    public Tray _Tray;
    public HingeJoint _HingeJoint;

   
    private void Start()
    {
        MY_COLOR();
       _Tray= transform.parent.parent.transform.GetComponent<Tray>();
       _Tray.TrayHold.Add(this);
        _HingeJoint = GetComponent<HingeJoint>();
        _HingeJoint.connectedBody= transform.parent.parent.transform.GetComponent<Rigidbody>();
        transform.parent = transform.parent.parent.transform.parent.transform;

    }

 
  
    public void TOUCH_EFECT()
    {
        _Tray.TrayHold.Remove(this);

        _Tray.OnOneGo();
        TOUCH_EFFECT.transform.SetParent(null);
        TOUCH_EFFECT.SetActive(true);

        _HingeJoint.connectedBody = null;
        transform.GetComponent<Collider>().enabled = false;
        transform.DOLocalRotate(new Vector3(0, 360, 0), 0.75f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear);
        transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.gameObject.SetActive(false);
        });
        transform.DOShakeRotation(
            duration: 0.75f,
            strength: new Vector3(5, 0, 20),
            vibrato: 10,
            randomness: 0,
            fadeOut: true
        );
        // _Tray.CheckForNeighbors(gameObject);
        // _Tray.TrayHold.Remove(this);
        // transform.gameObject.SetActive(false);
       
        // if (_Tray.TrayHold.Count == 0)
        // {
        //     _Tray.IfAny();
        //     _Tray.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(
        //         () =>
        //         {
        //             _Tray.SecondTraySpwan();
        //             _Tray.transform.parent.gameObject.SetActive(false);
        //         });
        // }
    }

    public void MY_COLOR()
    {
        if (COLOR_MANAGER.Instance == null) return;
       
           _OwnMeshRenderer.material = COLOR_MANAGER.Instance.ROLL_COLORS[(int)CapColor];
        
    }

    private void OnValidate()
    {
        MY_COLOR();
    }
}
