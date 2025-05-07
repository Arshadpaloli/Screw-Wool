using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MANAGER : MonoBehaviour
{
    public static UI_MANAGER Instance;

    public GameObject PROGRESSION_BAR;

    [Header("PANELS")]
    public GameObject WIN_PANEL;
    public GameObject FAIL_PANEL;

    [Header("---[BUTTONS]---")]
    public List<GameObject> POWER_UPS_BUTTONS;
    public GameObject POWERUPS_PARENT;
    public Button NEXT_BTN, RETRY_BTN, REVIVE_BTN;

    [Header("---[VALUES]---")]
    public int levelNo;
    public int COINS;
    public int INCREASE_COIN, DECREASE_COIN;

    [Header("---[TEXTS]---")]
    public TextMeshProUGUI LEVEL_TEXT;
    public TextMeshProUGUI COIN_TEXT;

    [Header("---[LEVEL IMAGES]---")]
    public List<Sprite> LEVEL_IMAGES;

    public Image MAIN_IMAGE;

    public Slider zoomSlider;
    public int maxZoomInSteps = 3;
    public Vector3 STARTNG_SIZE;
    private Vector3 MaxScale => STARTNG_SIZE + new Vector3(GAME_MANGER.Instance.ZOOM_SIZE, GAME_MANGER.Instance.ZOOM_SIZE, GAME_MANGER.Instance.ZOOM_SIZE) * maxZoomInSteps;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PLAY();
        }

        NEXT_BTN.onClick.AddListener(NEXT_LEVEL);
        RETRY_BTN.onClick.AddListener(RETRY);
        REVIVE_BTN.onClick.AddListener(REVIVE);

        LEVEL_TEXT.text = "Level " + (PlayerPrefs.GetInt("LEVEL_COUNT")).ToString();
        //SETTING_MAIN_IMAGE();
        SoundHapticManager.instance.PlayAudioWithOutVibration("MUSIC");
        Camera.main.DOOrthoSize(16.5f, 1f);
        STARTNG_SIZE = DragRotate.Instance.CubeToRotate.transform.localScale;
        zoomSlider.minValue = 0f;
        zoomSlider.maxValue = 1f;
        zoomSlider.onValueChanged.AddListener(SetZoomFromSlider);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PLAY()
    {
        //SoundHapticManager.instance.PlayAudio("CLICK_AUDIO", 25);
        if (PlayerPrefs.HasKey("LEVEL"))
        {
            int savedLevel = PlayerPrefs.GetInt("LEVEL");
            if (savedLevel != SceneManager.GetActiveScene().buildIndex)
            {
                DOVirtual.DelayedCall(0, () =>
                {
                    SceneManager.LoadScene(savedLevel);
                });
            }
        }
        else
        {
            DOVirtual.DelayedCall(0, () =>
            {
                PlayerPrefs.SetInt("LEVEL_COUNT", 0);
                PlayerPrefs.SetInt("LEVEL_COUNT", PlayerPrefs.GetInt("LEVEL_COUNT") + 1);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            });
        }
    }

    public void NEXT_LEVEL()
    {  
        CLICK_SOUND();
        //COIN_SOUND();

        NEXT_BTN.transform.DOScale(1.15f, 0.1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            NEXT_BTN.transform.DOScale(1.25f, 0.1f).SetEase(Ease.OutBounce);

        });

        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        int nextLevel = currentLevel + 1;

        PlayerPrefs.SetInt("LEVEL_COUNT", PlayerPrefs.GetInt("LEVEL_COUNT") + 1);

        NEXT_BTN.interactable = false;

        if (NEW_FEATURE_PROGRESSION.INSTANCE != null)
        {
            NEW_FEATURE_PROGRESSION.INSTANCE.SAVE();
        }

        DOVirtual.DelayedCall(1.5F, () =>
        {
            INCREASE_COIN = COINS + 25;
            StartCoroutine(COIN_INCREMENT());
        });

        if (nextLevel < totalScenes) // Check if the next level exists
        {
            if (PlayerPrefs.GetInt("LEVEL_COUNT") <= 30)
            {
                DOVirtual.DelayedCall(0, () =>
                {
                    PlayerPrefs.SetInt("LEVEL", nextLevel);
                    SceneManager.LoadScene(nextLevel);
                    //SceneManager.LoadScene(1);
                });

            }
            else
            {
                DOVirtual.DelayedCall(0, RANDOM_LEVELS);
            }
        }
        else
        {
            DOVirtual.DelayedCall(0, RANDOM_LEVELS);
        }
    }

    public void SKIP_NEXT_LEVEL()
    {

        //CLICK_SOUND();
        //COIN_SOUND();

        //NEXT_BTN.transform.DOScale(1.5f, 0.1f).SetEase(Ease.OutQuad).OnComplete(() =>
        //{
        //    NEXT_BTN.transform.DOScale(1.75f, 0.1f).SetEase(Ease.OutBounce);

        //});

        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        int nextLevel = currentLevel + 1;

        PlayerPrefs.SetInt("LEVEL_COUNT", PlayerPrefs.GetInt("LEVEL_COUNT") + 1);

        //NEXT_BTN.interactable = false;

        DOVirtual.DelayedCall(0, () =>
        {
            INCREASE_COIN = COINS + 25;
            StartCoroutine(COIN_INCREMENT());
        });

        if (nextLevel < totalScenes) // Check if the next level exists
        {
            if (PlayerPrefs.GetInt("LEVEL_COUNT") <= 66)
            {
                DOVirtual.DelayedCall(0, () =>
                {
                    PlayerPrefs.SetInt("LEVEL", nextLevel);
                    SceneManager.LoadScene(nextLevel);
                    //SceneManager.LoadScene(1);
                });

            }
            else
            {
                DOVirtual.DelayedCall(0, RANDOM_LEVELS);
            }
        }
        else
        {
            DOVirtual.DelayedCall(0, RANDOM_LEVELS);
        }
    }

    public void RANDOM_LEVELS()
    {
        int RANDOM_LEVEL = Random.Range(10, 30);
        //RANDOM_LEVEL -= 4;

        HashSet<int> restrictedLevels = new HashSet<int> { 6, 11, 17, 23};
        if (restrictedLevels.Contains(RANDOM_LEVEL))
        {
            RANDOM_LEVEL = 15;
        }

        PlayerPrefs.SetInt("LEVEL", RANDOM_LEVEL);
        //SceneManager.LoadScene(1);
        SceneManager.LoadScene(RANDOM_LEVEL);
    }

    public void RETRY()
    {
        CLICK_SOUND();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void REVIVE()
    {
        CLICK_SOUND();

        bool allSlotsFalse = true; // Assume all slots are false initially
    
        for (int i = 0; i < ManageTheCrate.Instance.lockedSlots.Count; i++)
        {
            if (ManageTheCrate.Instance.lockedSlots[i]) // If any slot is true, exit loop
            {
                allSlotsFalse = false;
                FAIL_PANEL.transform.DOScale(0, 0.5F).SetEase(Ease.InBack).OnComplete(() =>
                {
                    FAIL_PANEL.gameObject.SetActive(false);
                    ManageTheCrate.Instance.UnlockLockedSlot();
                    DragRotate.Instance.GameStop = false;
                });
                break;
            }
        }
    
        if (allSlotsFalse)
        {
           REVIVE_BTN.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "NO SLOTS AVAILABLE";
           SoundHapticManager.instance.PlayAudio("ERROR");
        }
    }

    public void SETTING_MAIN_IMAGE()
    {
        MAIN_IMAGE.sprite = LEVEL_IMAGES[SceneManager.GetActiveScene().buildIndex];
    }

    public IEnumerator COIN_INCREMENT()
    {
        float elapsedTime = 0f;
        int initialCoins = COINS;

        while (elapsedTime < 0.75)
        {
            elapsedTime += Time.deltaTime;
            COINS = (int)Mathf.Lerp(initialCoins, INCREASE_COIN, elapsedTime / 0.75F);
            COIN_TEXT.text = COINS.ToString();
            yield return null;
        }

        // Ensure the value reaches the exact target after the loop.
        COINS = INCREASE_COIN;
        COIN_TEXT.text = COINS.ToString();
        PlayerPrefs.SetInt("COINS", COINS);
    }

    public void SetZoomFromSlider(float value)
    {
        Transform target = DragRotate.Instance.CubeToRotate.transform;

        // Lerp between STARTNG_SIZE and MaxScale based on slider value
        Vector3 targetScale = Vector3.Lerp(STARTNG_SIZE, MaxScale, value);

        // Animate to new scale
        target.DOScale(targetScale, 0.4f).SetEase(Ease.OutQuad);
    }

    public IEnumerator COIN_DECREMENT()
    {
        float elapsedTime = 0f;
        int initialCoins = COINS;

        while (elapsedTime < 0.75)
        {
            elapsedTime += Time.deltaTime;
            COINS = (int)Mathf.Lerp(initialCoins, DECREASE_COIN, elapsedTime / 0.75F);
            COIN_TEXT.text = COINS.ToString();
            yield return null;
        }

        // Ensure the value reaches the exact target after the loop.
        COINS = DECREASE_COIN;
        COIN_TEXT.text = COINS.ToString();
        PlayerPrefs.SetInt("COINS", COINS);
    }

    public void CLICK_SOUND()
    {
        SoundHapticManager.instance.PlayAudio("CLICK_SOUND");
    }

    public void COIN_SOUND()
    {
        SoundHapticManager.instance.PlayAudioWithOutVibration("COIN_POP_SOUND");
        SoundHapticManager.instance.PlayAudioWithOutVibration("COIN_INCREMENT_SOUND");
    }
}
