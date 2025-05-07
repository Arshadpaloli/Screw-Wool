using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EachCap : MonoBehaviour
{
    public ColorType CapColor;
    public GameObject TOUCH_EFFECT;

    public List<GameObject> THREAD_ROLLS;
    public Tray _Tray;
    public bool Hidden,Rope;
    public GameObject HiddenGameObject;
    public EachCap ConnectedObject;
    private void Awake()
    {
        _Tray = transform.parent.transform.parent.GetComponent<Tray>();
        _Tray.TrayHold.Add(this);
    }
    private void Start()
    {
        MY_COLOR();
       
        
    }

    public void SetHiddeen()
    {
        if (Hidden)
        {
            transform.GetComponent<CapsuleCollider>().enabled = false;
           
            Transform topParent = transform;
            while (topParent.parent != null)
            {
                topParent = topParent.parent;
                
            }
            
            Vector3 TopParentPos = topParent.lossyScale;
            topParent.transform.localScale = Vector3.one;
            HiddenGameObject = Instantiate(Levelfeatures.instance.HiddenPrefab,transform.position,Quaternion.identity);
            HiddenGameObject.transform.SetParent(transform);
            HiddenGameObject.transform.localPosition =new Vector3(0f,1.25f,0f);
            HiddenGameObject.transform.localRotation=Quaternion.Euler(0f,0f,0f);
            Vector3 HiddernOgS = HiddenGameObject.transform.localScale;
            HiddenGameObject.transform.localScale=Vector3.zero;
            topParent.transform.localScale = TopParentPos;
            HiddenGameObject.transform.DOScale(HiddernOgS, 0.5f).SetEase(Ease.OutBack);

        }
    }
    public void TOUCH_EFECT()
    {
        TOUCH_EFFECT.transform.SetParent(null);
        _Tray.CheckForNeighbors(gameObject);
        TOUCH_EFFECT.SetActive(true);
        _Tray.TrayHold.Remove(this);
        
        if (Rope)
        {
            _Tray.RopObject.SetActive(false);
        }
        
        if (_Tray.TrayHold.Count == 0)
        {
            _Tray.IfAny();
            _Tray.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(
                () =>
                {
                    _Tray.SecondTraySpwan();
                    _Tray.transform.parent.gameObject.SetActive(false);
                });
        }
    }

    public void MY_COLOR()
    {
        if (COLOR_MANAGER.Instance == null) return;
        for (int i = 0; i < THREAD_ROLLS.Count; i++)
        {
            THREAD_ROLLS[i].GetComponent<MeshRenderer>().material = COLOR_MANAGER.Instance.ROLL_COLORS[(int)CapColor];
        }
    }

    private void OnValidate()
    {
        MY_COLOR();
    }
}
