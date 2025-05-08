using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class CrateManager : MonoBehaviour
{
    public static CrateManager instance;

    [Header("All Rolls GameObjects")]
    public List<GameObject> TOTAL_ROLLS;

    [Header("Grouped Rolls")]
    public List<CapGroup> groupedCaps = new List<CapGroup>();

    [Header("Crate Setup")]
    public GameObject CrateHolder;
    public List<Crate> _allCrates;
    public Transform slotone,slottwo,slotthree,slotfour,EndPoint;
    private ManageTheCrate _ManageTheCrate;

    public int TOTAL_THREAD_STAND;
    public int THREAD_STAND_SORTED;

    public Image CLR_IMG;
    public TextMeshProUGUI PERCENT_TEXT;
    private float currentPercent = 0f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _ManageTheCrate = FindObjectOfType<ManageTheCrate>();
        _allCrates = CrateHolder.GetComponentsInChildren<Crate>().ToList();

        TOTAL_THREAD_STAND = _allCrates.Count;

        slotone.position = _allCrates[0].transform.position;
        slottwo.position = _allCrates[1].transform.position;
        slotthree.position = _allCrates[2].transform.position;
        slotfour.position = _allCrates[3].transform.position;
//
// Keep the first 2 crates intact
        var firstTwo = _allCrates.Take(2).ToList();
        var rest = _allCrates.Skip(2).ToList();

// Shuffle the rest
        System.Random rng = new System.Random();
        int n = rest.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = rest[k];
            rest[k] = rest[n];
            rest[n] = value;
        }

// Combine them back
        _allCrates = firstTwo.Concat(rest).ToList();


        GETTING_ROLLS();
        // StartCoroutine(Rolls_Entry());
        GroupCapsByColor();
        CheckIfonSlot();

        if (Levelfeatures.instance != null)
        {
            Levelfeatures.instance.SPAWNING_TRAY_0FF();
        }
    }

    public void GETTING_ROLLS()
    {
        GameObject[] roll = GameObject.FindGameObjectsWithTag("Cap");
        foreach (GameObject rolls in roll)
        {
            TOTAL_ROLLS.Add(rolls);
        }
    }

    public IEnumerator Rolls_Entry()
    {
        // First, shrink all rolls to zero
        foreach (var roll in TOTAL_ROLLS)
        {
            roll.transform.localScale = Vector3.zero;
        }

        // Sort rolls based on distance to the main camera
        List<GameObject> sortedRolls = TOTAL_ROLLS
            .OrderBy(roll => Vector3.Distance(Camera.main.transform.position, roll.transform.position))
            .ToList();

        // Animate them one by one based on proximity
        foreach (var roll in sortedRolls)
        {
            roll.transform.DOScale(0.3529992f, 0.5f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    roll.transform.GetComponent<EachCap>()._Tray.TurnOnRop();
                    // roll.transform.GetComponent<EachCap>().SetHiddeen();

                });

            yield return new WaitForSeconds(0.02f);
        }

    }

    public void GAME_WIN()
    {
        Debug.Log("<color=#00FF00><size=16><b>✔ Game Win!</b></size></color>");
        CubeGridGenerator.instance.LAST_WAVE();
        SoundHapticManager.instance.PlayAudio("WIN");

        if (TUTORIAL.instance != null)
        {
            TUTORIAL.instance.SECOND_HAND.SetActive(false);
            TUTORIAL.instance.SECOND_TEXT.SetActive(false);
        }

    }

    [ContextMenu("Group Caps By Color")]
    public void GroupCapsByColor()
    {
        groupedCaps.Clear();
        Dictionary<ColorType, List<GameObject>> groupedDict = new Dictionary<ColorType, List<GameObject>>();

        foreach (var obj in TOTAL_ROLLS)
        {
            EachCap cap = obj.GetComponent<EachCap>();
            if (cap != null)
            {
                if (!groupedDict.ContainsKey(cap.CapColor))
                    groupedDict[cap.CapColor] = new List<GameObject>();

                groupedDict[cap.CapColor].Add(obj);
            }
        }

        foreach (var pair in groupedDict)
        {
            CapGroup group = new CapGroup();
            group.colorType = pair.Key;
            group.caps = pair.Value;
            groupedCaps.Add(group);
        }
    }

    public void MoveConnectedRop(EachCap Thread)
    {
        Crate OwnerPoint = ThreadPoint(Thread);
        ThreadEffect ThreadRope = Thread.GetComponent<ThreadEffect>();
        if (OwnerPoint != null)
        {
            ThreadRope.endPoint = OwnerPoint.OnThreadEnter();
            Thread.GetComponent<CapsuleCollider>().enabled = false;
            ThreadRope.ROPE_GO();
            ThreadRope._crate = OwnerPoint;
            On_Touch(Thread);
        }
        else
        {
            _ManageTheCrate.RopeMoveTOWaitingRop(Thread);
        }
    }
    public void ThreadToSlot(EachCap Thread)
    {
        // Thread.gameObject.SetActive(false);
       Thread.TOUCH_EFECT(); 
        Crate OwnerPoint = ThreadPoint(Thread);
        ThreadEffect ThreadRope = Thread.GetComponent<ThreadEffect>();
        if (OwnerPoint != null)
        {
            // if (Thread.ConnectedObject != null)
            // {
            //     DOVirtual.DelayedCall(.1f, () =>
            //     {
            //         MoveConnectedRop(Thread.ConnectedObject);
            //
            //     });
            // }
            ThreadRope.endPoint = OwnerPoint.OnThreadEnter();
            // Thread.GetComponent<CapsuleCollider>().enabled = false;
            ThreadRope._crate = OwnerPoint;

            ThreadRope.ROPE_GO();
            On_Touch(Thread);
          
        }
        else
        {
            // if (Thread.ConnectedObject != null)
            // {
            //     DOVirtual.DelayedCall(.1f, () =>
            //     {
            //         MoveConnectedRop(Thread.ConnectedObject);
            //
            //     });
            // }
            _ManageTheCrate.RopeMoveTOWaitingRop(Thread);
        }
    }

    public void On_Touch(EachCap Thread)
    {
       
    }
    public void CheckIfonSlot()
    {
        for (int i = 0; i < _allCrates.Count; i++)
        {
            Vector3 cratePos = _allCrates[i].transform.position;

            if (cratePos == slotone.position ||
                cratePos == slottwo.position ||
                cratePos == slotthree.position ||
                cratePos == slotfour.position)
            {
                _allCrates[i].Center = true;
            }
        }
    }
    
    public void BringToSlot(Crate mover)
    {
        Transform slot = GetSlot(mover);
        //Debug.Log("<color=#00FF00><size=16><b>✔ On Thread End</b></size></color>");

        FILL_COLOR();

        SoundHapticManager.instance.PlayAudioWithOutVibration("WHOOSH");

        mover.SMOKE_EFFECT.transform.parent = null;
        mover.SMOKE_EFFECT.SetActive(true);

        ManageTheCrate.Instance.CancelFail();
        // Animate mover to the endpoint, then deactivate it
        mover.transform.DOMoveX(EndPoint.transform.position.x, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                _allCrates.Remove(mover);
                mover.gameObject.SetActive(false);

                Crate newCenterCrate = null;
                if (_allCrates.Count < 9)
                {
                    for (int i = 0; i < _allCrates.Count; i++)
                    {
                        if (!_allCrates[i].Center)
                        {
                            newCenterCrate = _allCrates[i];
                            break;
                        }
                    }
                }
                else
                {
                    var cubeList = CubeGridGenerator.instance.cubeList;
                    var borderY = CubeGridGenerator.instance.borderDown.transform.position.y;

                    foreach (var crate in _allCrates)
                    {
                        if (crate.Center) continue;

                        foreach (var cube in cubeList)
                        {
                            if (cube.CellType != crate.CrateColor || cube.SORTED) continue;

                            if (cube.transform.position.y > borderY)
                            {
                                newCenterCrate = crate;
                                break;
                            }
                        }

                        if (newCenterCrate != null)
                            break;
                    }

                    // Fallback if none found in the above logic
                    if (newCenterCrate == null)
                    {
                        for (int i = 0; i < _allCrates.Count; i++)
                        {
                            if (!_allCrates[i].Center)
                            {
                                newCenterCrate = _allCrates[i];
                                break;
                            }
                        }
                    }
                }

                // Cache references
               



                if (_allCrates.Count==0)
                {
                    GAME_WIN();
                }

                if (newCenterCrate != null)
                {
                    newCenterCrate.Center = true;

                    // Cache newCenterCrate in local variable to avoid closure issues in loop
                    Crate crateToMove = newCenterCrate;

                    crateToMove.transform.DOMove(slot.position, 0.5f)
                        .SetEase(Ease.OutBack)
                        .OnComplete(() =>
                        {
                            _ManageTheCrate.On_ThreadStandCall(crateToMove);
                        });
                }
            });
    }


    public Transform GetSlot(Crate mover)
    {
        Vector3 moverPos = mover.transform.position;

        if (moverPos == slotone.position)
            return slotone;
        else if (moverPos == slottwo.position)
            return slottwo;
        else if (moverPos == slotthree.position)
            return slotthree;
        else if (moverPos == slotfour.position)
            return slotfour;

        return null;
    }
    private Crate ThreadPoint(EachCap Thread)
    {
        Crate bestCrate = null;
        int highestIsFull = int.MinValue;

        for (int i = 0; i < _allCrates.Count; i++)
        {
            var crate = _allCrates[i];
            if (crate.CrateColor == Thread.CapColor && crate.Center && crate.CanGo())
            {
                if (crate.IsFull > highestIsFull)
                {
                    highestIsFull = crate.IsFull;
                    bestCrate = crate;
                }
            }
        }

        return bestCrate;
    }


    public void FILL_COLOR()
    {
        THREAD_STAND_SORTED++;
        float targetPercent = ((float)THREAD_STAND_SORTED / TOTAL_THREAD_STAND) * 100f;

        // Kill any existing tween before starting a new one
        DOTween.Kill("PercentTween");

        DOTween.To(() => currentPercent, x => {
            currentPercent = x;
            PERCENT_TEXT.text = Mathf.RoundToInt(currentPercent) + " %";
        }, targetPercent, 0.5f) // duration can be tweaked
        .SetId("PercentTween")
        .SetEase(Ease.OutQuad);

        // Calculate fill amount: number of collected balls divided by total balls
        CLR_IMG.DOFillAmount((float)THREAD_STAND_SORTED / TOTAL_THREAD_STAND, 1);

        // Optional: Clamp to prevent overfilling
        if (CLR_IMG.fillAmount > 1f)
        {
            CLR_IMG.fillAmount = 1f;
        }
    }
}


[System.Serializable]
public class CapGroup
{
    public ColorType colorType;
    public List<GameObject> caps = new List<GameObject>();
}
