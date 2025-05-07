using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UnityEngine;

public class KeyAndLock : MonoBehaviour
{
    public ColorType KeyOrLock;
    // Start is called before the first frame update
    void Start()
    {
        DOVirtual.DelayedCall(.2f, () =>
        {
            if (COLOR_MANAGER.Instance == null) return;
            transform.GetComponent<MeshRenderer>().material = COLOR_MANAGER.Instance.KEY_COLORS[(int)KeyOrLock];
        
        });
       
    }

    // Update is called once per frame
}
