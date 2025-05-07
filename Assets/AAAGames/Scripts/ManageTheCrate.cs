using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ManageTheCrate : MonoBehaviour
{
    public static ManageTheCrate Instance;
    private CrateManager _crateManager;
    public List<SolidRevealController> WaitingRop;
    private Coroutine failCoroutine;
    public bool cancelFail;
    public Transform SlotLeg;
    private int lockedSlotsCount = 3;
    public List<bool> lockedSlots; // true = locked
    public List<GameObject> ExtraSlot;
    private bool OnExtraProcess;
   public GameObject ClickOnExtra;
    public GameObject PLUS_BTN;
    public GameObject STAR_EFFECT;

    public int SLOT_ADDED;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _crateManager = FindObjectOfType<CrateManager>();
        WaitingRop = GetComponentsInChildren<SolidRevealController>().ToList();

        lockedSlots = new List<bool>(new bool[WaitingRop.Count]);
        LockLastNSlots(lockedSlotsCount);
        // UnlockLockedSlot();
    }

    private void LockLastNSlots(int n)
    {
        for (int i = WaitingRop.Count - n; i < WaitingRop.Count; i++)
        {
            lockedSlots[i] = true;
        }
    }

    public void UnlockLockedSlot()
    {
        if (!OnExtraProcess)
        {
            OnExtraProcess = true;
            for (int i = WaitingRop.Count - lockedSlotsCount; i < WaitingRop.Count; i++)
            {
                if (lockedSlots[i])
                {
                    // Unlock the slot
                    lockedSlots[i] = false;
            
                    // Disable the corresponding UI element
                    if (i - (WaitingRop.Count - lockedSlotsCount) >= 0 && i - (WaitingRop.Count - lockedSlotsCount) < ExtraSlot.Count)
                    {
                        GameObject slot = ExtraSlot[i - (WaitingRop.Count - lockedSlotsCount)];
                        SLOT_ADDED++;
                        if(SLOT_ADDED==3)
                        {
                            PLUS_BTN.SetActive(false);
                        }
                        ClickOnExtra.SetActive(false);

                        STAR_EFFECT.SetActive(false);
                        STAR_EFFECT.SetActive(true);

                        UI_MANAGER.Instance.CLICK_SOUND();

                        SlotLeg.transform.DOMoveX(slot.transform.position.x+1.4f, .5f).SetEase(Ease.InOutCubic);
                        slot.transform.DOMoveX(slot.transform.position.x+0.9f, .5f).SetEase(Ease.InOutCubic).OnComplete(
                            () =>
                            {
                                OnExtraProcess = false;
                                SoundHapticManager.instance.PlayAudioWithOutVibration("SLOT_UNLOCK");
                            });
                        slot.transform.GetChild(0).gameObject.SetActive(true);
                        Debug.Log($"🔓 Unlocked slot {i}, corresponding UI disabled.");
                    }

                    // Cancel fail logic if needed
                    CancelFail();
                    return;
                }
            }

            Debug.LogWarning("⚠ All locked slots are already unlocked.");
        }
       
    }


    public void On_ThreadStandCall(Crate crate)
    {
        bool foundMatch = false;

        for (int i = 0; i < WaitingRop.Count; i++)
        {
            if (!lockedSlots[i] && WaitingRop[i].ReveleColor == crate.CrateColor && WaitingRop[i].Oqq && crate.IsFull < 3)
            {
                foundMatch = true;
                CancelFail();

                ThreadEffect ThreadRope = WaitingRop[i].GetComponent<ThreadEffect>();
                ThreadRope.endPoint = crate.OnThreadEnter();
                WaitingRop[i].ReveleColor = ColorType.None;
                ThreadRope.ROPE_GO();
                WaitingRop[i].GetComponent<LineRenderer>().material.color = crate.transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
                WaitingRop[i].Start_invincibleForWaititngRop();
                ThreadRope._crate = crate;
            }
        }
        
        if (!foundMatch)
        {
            Debug.Log("fail");
            CheckFail();
        }
    }

    public void CheckFail()
    {
        int unprocessedCount = 0;
        for (int i = 0; i < WaitingRop.Count; i++)
        {
            if (!WaitingRop[i].Oqq && !lockedSlots[i])
            {
                unprocessedCount++;
            }
        }

        if (unprocessedCount == 0 && failCoroutine == null)
        {
            failCoroutine = StartCoroutine(FailAfterDelay(2));
            DragRotate.Instance.GameStop = true;
            print("Startfail");
        }
    }

    public void RopeMoveTOWaitingRop(EachCap Rope)
    {
        int unprocessedCount = 0;
        for (int i = 0; i < WaitingRop.Count; i++)
        {
            if (!WaitingRop[i].Oqq && !lockedSlots[i])
                unprocessedCount++;
        }

        if (unprocessedCount == 2)
        {
            Debug.Log("<color=#FFA500><size=16><b>⚠ Only One Slot Left!</b></size></color>");
        }
        else if (unprocessedCount == 1 && failCoroutine == null)
        {
            failCoroutine = StartCoroutine(FailAfterDelay(3));
            DragRotate.Instance.GameStop = true;
            if (!PlayerPrefs.HasKey("HasShownClickOnExtra"))
            {
                PlayerPrefs.SetInt("HasShownClickOnExtra", 1); // Mark as shown
                ClickOnExtra.SetActive(true);
                ClickOnExtra.transform.DOScale(.15f, 0.5f) // Scale to 1.2x in 0.5s
                    .SetLoops(-1, LoopType.Yoyo) // Loop forever, back and forth
                    .SetEase(Ease.InOutSine);   // Smooth easing
            }

        
            print("Startfail");
        }

        for (int i = 0; i < WaitingRop.Count; i++)
        {
            if (!WaitingRop[i].Oqq && !lockedSlots[i])
            {
                Rope.GetComponent<CapsuleCollider>().enabled = false;
                _crateManager.On_Touch(Rope);

                WaitingRop[i].GetComponent<MeshRenderer>().material.color =
                    Rope.transform.GetChild(0).GetComponent<MeshRenderer>().material.color;

                ThreadEffect ThreadRope = Rope.GetComponent<ThreadEffect>();
                WaitingRop[i].ReveleColor = Rope.CapColor;
                WaitingRop[i].Oqq = true;
                ThreadRope.endPoint = WaitingRop[i].transform;
                ThreadRope.ROPE_GO();
                WaitingRop[i].Start_revelForWaititngRop();
                Debug.Log("Rope Rope Moved");
                return;
            }
        }
    }

    public bool QuickCheckFail()
    {
        int unprocessedCount = 0;
        for (int i = 0; i < WaitingRop.Count; i++)
        {
            if (!WaitingRop[i].Oqq && !lockedSlots[i])
                unprocessedCount++;
        }

        if (unprocessedCount == 0)
        {
            return true;
            
        }
       return false;
        
    }
    private IEnumerator FailAfterDelay(int Delay)
    {
        cancelFail = false;
        yield return new WaitForSeconds(Delay);

        if (!cancelFail&&QuickCheckFail())
        {
            Debug.Log("<color=#FF0000><size=16><b>❌ All slots are filled! Rope can't be placed.</b></size></color>");
            UI_MANAGER.Instance.FAIL_PANEL.gameObject.SetActive(true);
            UI_MANAGER.Instance.FAIL_PANEL.transform.DOScale(1, 0.5F);
            UI_MANAGER.Instance.REVIVE_BTN.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Unlock a New Slot";
            SoundHapticManager.instance.PlayAudio("FAIL");
        }

        failCoroutine = null;
    }

    public void CancelFail()
    {
        DragRotate.Instance.GameStop = false;

        if (failCoroutine != null)
        {
            DragRotate.Instance.GameStop = false;

            print("CAllled");

            cancelFail = true;
            StopCoroutine(failCoroutine);
            failCoroutine = null;
        }
    }
}
