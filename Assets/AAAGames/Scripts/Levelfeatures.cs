using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Linq;

public class Levelfeatures : MonoBehaviour
{
    public static Levelfeatures instance;
    public GameObject KeyPrefab,LockPrefab,HiddenPrefab,RopePrefab;
    public List<Tray> LockedTrays;
    public List<GameObject> SecondTrays;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < SecondTrays.Count; i++)
        {
            Tray tray = SecondTrays[i].GetComponentInChildren<Tray>();
            if (tray == null) continue;

            if (tray.IsLock || tray.IsKeyHolder)
            {
                tray.IsLock = false;
                tray.IsKeyHolder = false;
            }
        }
        DOVirtual.DelayedCall(.1f, () =>
        {
            List<Tray> keyHolders = LockedTrays.Where(t => t.IsKeyHolder && t.key != null).ToList();
            List<Tray> locks = LockedTrays.Where(t => t.IsLock && t.Lock != null).ToList();

            int pairCount = Mathf.Min(keyHolders.Count, locks.Count);

            // Get all valid colors (excluding None)
            ColorType[] validColors = System.Enum.GetValues(typeof(ColorType))
                .Cast<ColorType>()
                .Where(color => color != ColorType.None && color != ColorType.White && color != ColorType.Gray)

                .ToArray();

            for (int i = 0; i < pairCount && i < validColors.Length; i++)
            {
                ColorType color = validColors[i];

                // Assign color to key GameObject
                KeyAndLock keyComponent = keyHolders[i].key.GetComponent<KeyAndLock>();
                if (keyComponent != null)
                    keyComponent.KeyOrLock = color;

                // Assign color to lock GameObject
                KeyAndLock lockComponent = locks[i].Lock.GetComponent<KeyAndLock>();
                if (lockComponent != null)
                    lockComponent.KeyOrLock = color;
            }

        
        });
      

    }
    public void NewTraySpawner(GameObject MovingTray)
    {
        if (SecondTrays.Count > 0)
        {
            print("sound");
            for (int i = 0; i < SecondTrays.Count; i++)
            {
                if (!SecondTrays[i].activeInHierarchy&&SecondTrays[i].transform.position==MovingTray.transform.position)
                {
                    SecondTrays[i].transform.localScale=Vector3.zero;

                    SecondTrays[i].SetActive(true);
                    SecondTrays[i].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
                    SecondTrays.Remove(SecondTrays[i]);
                    break;
                }
            }
        }
       
    }
    public void key_MoveToLock(KeyAndLock Key)
    {
        if (LockedTrays == null || LockedTrays.Count == 0) return;

        // Filter out trays where Lock is not active
        List<Tray> activeTrays = LockedTrays.FindAll(tray => tray.Lock == true && tray.Lock.activeSelf);
        Tray targetTray = null;
        if (activeTrays.Count == 0) return; // No active trays found

        // Choose a random tray from active trays
        for (int i = 0; i < activeTrays.Count; i++)
        {
            if (activeTrays[i].Lock.GetComponent<KeyAndLock>().KeyOrLock == Key.KeyOrLock)
            {
                targetTray = activeTrays[i];
            }
        }
        // = activeTrays[Random.Range(0, activeTrays.Count)];
        LockedTrays.Remove(targetTray);
        Key.transform.DORotate(new Vector3(0, 0, 90), 0.5f).SetEase(Ease.Linear);

        // Move the key to the target tray's position using DOTween
        Key.transform.DOMove(targetTray.Lock.transform.position, 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            Key.transform.DOScale(Vector3.one * 0.2f, 0.2f)
                .SetEase(Ease.OutBack);
            // Unlock animation (scale up then disappear)
            targetTray.Lock.transform.DOScale(Vector3.one * 0.2f, 0.2f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    Key.transform.DOScale(Vector3.zero, 0.2f)
                        .SetEase(Ease.InBack);
                    targetTray.Lock.transform.DOScale(Vector3.zero, 0.2f)
                        .SetEase(Ease.InBack)
                        .OnComplete(() =>
                        {
                            targetTray.Lock.SetActive(false);
                            Key.gameObject.SetActive(false);
                            // Enable colliders of tray content
                            for (int i = 0; i < targetTray.TrayHold.Count; i++)
                            {
                                targetTray.TrayHold[i].transform.GetComponent<CapsuleCollider>().enabled = true;
                            }
                        });
                });
        });
    }

    public void SPAWNING_TRAY_0FF()
    {
        for (int i = 0;i<SecondTrays.Count;i++)
        {
            SecondTrays[i].SetActive(false);
        }
    }

//6 to 10 lock === 11 to 16 hidden === 17 to 22 Rope == 23 to 26 inside
}
