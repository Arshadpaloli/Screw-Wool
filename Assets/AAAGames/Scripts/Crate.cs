using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Crate : MonoBehaviour
{
    public ColorType CrateColor;
    public int ThreadStand;
    public bool Center;
    public SolidRevealController Rope;
    public MeshRenderer CYLINDER_MESH;

   public int IsFull;

    public GameObject SMOKE_EFFECT;
    void Start()
    {
        SortCapsByX();
        //MY_COLOR();
    }
    public Transform OnThreadEnter()
    {
        IsFull++;
        SCALING();
        Rope.StartReveal(Rope.transform.GetChild(0).transform);
        return Rope.transform.GetChild(0).transform;
    }

    public bool CanGo()
    {
        if (IsFull < 3)
        {
            return true;
        }
        return false;
    }
    public void SCALING()
    {
        transform.DOScale(new Vector3(0.26F, 0.2550344F, 0.31136F), 0.1F).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            transform.DOScale(new Vector3(0.2424177F, 0.2377879F, 0.2903045F), 0.1F).SetEase(Ease.OutCirc);
        });
    }
    public void checkFull()
    {
        ThreadStand++;
        if (ThreadStand == 3)
        {
            //print("Thread enter");
            ManageTheCrate.Instance.CancelFail();
            Rope.Oqq = true;
            transform.GetComponent<WaitingThreadEffect>().ROPE_GO();
            CubeGridGenerator.instance.RevealColorGroup(this);
            Rope.StartOne();

        }
    }
    public void MY_COLOR()
    {
        if (COLOR_MANAGER.Instance==null) return;
        CYLINDER_MESH.material = COLOR_MANAGER.Instance.THREAD_HOLDER_COLORS[(int)CrateColor];
        Rope.gameObject.GetComponent<MeshRenderer>().material = COLOR_MANAGER.Instance.TOP_ROLL_COLORS[(int)CrateColor];
        
    }

    private void OnValidate()
    {
        MY_COLOR();
    }

    void SortCapsByX()
    {
        // Order by X position and update the list
        // Caps = Caps.OrderByDescending(cap => cap.transform.position.x).ToList();

    }
}