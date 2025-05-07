using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NEW_FEATURE_PROGRESSION : MonoBehaviour
{
    public static NEW_FEATURE_PROGRESSION INSTANCE;

    public GameObject NEW_FEATURE_PANEL;
    public GameObject LOCK_TRAY, HIDDEN_THREADS, CONNECTED_THREADS,SPAWNING_TRAYS;
    public Image LOCK_TRAY_CLR, HIDDEN_THREADS_CLR, CONNECTED_THREADS_CLR, SPAWNING_TRAYS_CLR;
    public Image CLR_IMG, BORDER;
    public int PROGRESSION,NEXT_PROGRESSION;


    private void Awake()
    {
        INSTANCE = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        IMAGE_CHNAGE();

        if(PlayerPrefs.HasKey("NEXT_PROGRESSION"))
        {
            NEXT_PROGRESSION = PlayerPrefs.GetInt("NEXT_PROGRESSION");
        }
        else
        {
            NEXT_PROGRESSION = 5;
            PlayerPrefs.SetInt("NEXT_PROGRESSION", NEXT_PROGRESSION);
        }

        PROGRESSION = PlayerPrefs.GetInt("PROGRESSION");
        CLR_IMG.DOFillAmount((float)PROGRESSION / NEXT_PROGRESSION, 0);
        if(PlayerPrefs.GetInt("NEW_FEATURE_FINISHED") == 1)
        {
            NEW_FEATURE_PANEL.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FILL_COLOR()
    {
        PROGRESSION++;

        // Calculate fill amount: number of collected balls divided by total balls
        CLR_IMG.DOFillAmount((float)PROGRESSION / NEXT_PROGRESSION, 0.5F).SetEase(Ease.Linear).OnComplete(()=>
        {
            UI_MANAGER.Instance.NEXT_BTN.interactable = true;
        });

        // Optional: Clamp to prevent overfilling
        if (CLR_IMG.fillAmount > 1f)
        {
            CLR_IMG.fillAmount = 1f;
        }

        if(PROGRESSION == NEXT_PROGRESSION)
        {
            if(PlayerPrefs.GetInt("LOCK_TRAY") ==0)
            {
                Vector3 currentScale = LOCK_TRAY.transform.localScale;
                Vector3 targetScale = currentScale + new Vector3(0.025f, 0.05f, 0.2f);
                LOCK_TRAY.transform.DOScale(targetScale, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            }
            else if(PlayerPrefs.GetInt("HIDDEN_THREADS") == 0)
            {
                Vector3 currentScale = HIDDEN_THREADS.transform.localScale;
                Vector3 targetScale = currentScale + new Vector3(0.025f, 0.05f, 0.2f);
                HIDDEN_THREADS.transform.DOScale(targetScale, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            }
            else if (PlayerPrefs.GetInt("CONNECTED_THREADS") == 0)
            {
                Vector3 currentScale = CONNECTED_THREADS.transform.localScale;
                Vector3 targetScale = currentScale + new Vector3(0.025f, 0.05f, 0.2f);
                CONNECTED_THREADS.transform.DOScale(targetScale, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            }
            else if (PlayerPrefs.GetInt("SPAWNING_TRAYS") == 0)
            {
                Vector3 currentScale = SPAWNING_TRAYS.transform.localScale;
                Vector3 targetScale = currentScale + new Vector3(0.025f, 0.05f, 0.2f);
                SPAWNING_TRAYS.transform.DOScale(targetScale, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            }
        }
    }

    public void IMAGE_CHNAGE()
    {
        if (PlayerPrefs.GetInt("LOCK_TRAY") == 0)
        {
            LOCK_TRAY.SetActive(true);
            LOCK_TRAY_CLR.gameObject.SetActive(true);
            CLR_IMG = LOCK_TRAY_CLR;
        }
        else if (PlayerPrefs.GetInt("HIDDEN_THREADS") == 0)
        {
            HIDDEN_THREADS.SetActive(true);
            HIDDEN_THREADS_CLR.gameObject.SetActive(true);
            CLR_IMG = HIDDEN_THREADS_CLR;
        }
        else if (PlayerPrefs.GetInt("CONNECTED_THREADS") == 0)
        {
            CONNECTED_THREADS.SetActive(true);
            CONNECTED_THREADS_CLR.gameObject.SetActive(true);
            CLR_IMG = CONNECTED_THREADS_CLR;
        }
        else if (PlayerPrefs.GetInt("SPAWNING_TRAYS") == 0)
        {
            SPAWNING_TRAYS.SetActive(true);
            SPAWNING_TRAYS_CLR.gameObject.SetActive(true);
            CLR_IMG = SPAWNING_TRAYS_CLR;
        }
    }

    public void SAVE()
    {
        if(PROGRESSION == NEXT_PROGRESSION)
        {
            PROGRESSION = 0;

            if (PlayerPrefs.GetInt("LOCK_TRAY") == 0)
            {
                NEXT_PROGRESSION = 5;
                PlayerPrefs.SetInt("NEXT_PROGRESSION", NEXT_PROGRESSION);

                PlayerPrefs.SetInt("LOCK_TRAY", 1);

            }
            else if (PlayerPrefs.GetInt("HIDDEN_THREADS") == 0)
            {
                NEXT_PROGRESSION = 6;
                PlayerPrefs.SetInt("NEXT_PROGRESSION", NEXT_PROGRESSION);

                PlayerPrefs.SetInt("HIDDEN_THREADS", 1);
            }
            else if (PlayerPrefs.GetInt("CONNECTED_THREADS") == 0)
            {
                NEXT_PROGRESSION = 6;
                PlayerPrefs.SetInt("NEXT_PROGRESSION", NEXT_PROGRESSION);
                PlayerPrefs.SetInt("CONNECTED_THREADS", 1);
            }
            else if (PlayerPrefs.GetInt("SPAWNING_TRAYS") == 0)
            {
                PlayerPrefs.SetInt("SPAWNING_TRAYS", 1);
                PlayerPrefs.SetInt("NEW_FEATURE_FINISHED", 1);
            }
        }

        PlayerPrefs.SetInt("PROGRESSION", PROGRESSION);
    }
}
