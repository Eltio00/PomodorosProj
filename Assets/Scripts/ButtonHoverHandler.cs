using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ButtonHoverHandler : MonoBehaviour
{
    [Header("Hovered Images")]
    [SerializeField] private Sprite backImg;
    [SerializeField] private Sprite nextImg;
    [SerializeField] private Sprite prevImg;
    [SerializeField] private Sprite cacheImg;
    [SerializeField] private Sprite conversationImg;
    [SerializeField] private Sprite loadImg;
    [SerializeField] private Sprite dropDownOpenImg;
    [SerializeField] private Sprite dropDownCloseImg;
    [SerializeField] private Sprite addImg;
    [SerializeField] private Sprite removeImg;
    [SerializeField] private Sprite saveImg;
    [SerializeField] private Sprite menuImg;
    [SerializeField] private Sprite pauseImg;
    [SerializeField] private Sprite sendImg;
    [SerializeField] private Sprite stopImg;
    [SerializeField] private Sprite playImg;
    [SerializeField] private Sprite quitImg;
    [SerializeField] private Sprite volumeImg;
    [SerializeField] private Sprite creditsImg;
    
    [Header("Press Simulation")]
    [SerializeField] private float pressDuration = 0.15f;
    [SerializeField] private GameObject menuOpen;
    [SerializeField] private GameObject dropDownClose;

    [Header("Sound")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSound;

    private Dictionary<Button, Sprite> allButtonsHovered = new Dictionary<Button,Sprite>();
    public static ButtonHoverHandler Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Button[] tmpBtns = FindObjectsByType<Button>();
        audioSource = this.GetComponent<AudioSource>();

        foreach (Button btn in tmpBtns)
        {
            Image img = btn.GetComponent<Image>();

            if (img == null || img.sprite == null)
                continue;

            AddToButtonMap(img.sprite.name, btn);
            btn.onClick.AddListener(() => OnButtonClicked(btn));
        }
        menuOpen.SetActive(false);
        dropDownClose.SetActive(false);
    }
    private void AddToButtonMap(string btnName, Button btn)
    {
        if (btnName.Contains(Constants.ADD)) allButtonsHovered[btn] = addImg;
        if (btnName.Contains(Constants.BACK)) allButtonsHovered[btn] = backImg;
        if (btnName.Contains(Constants.DROPDOWN_CLOSE)) allButtonsHovered[btn] = dropDownCloseImg;
        if (btnName.Contains(Constants.DROPDOWN_OPEN)) allButtonsHovered[btn] = dropDownOpenImg;
        if (btnName.Contains(Constants.NEXT_SCENE)) allButtonsHovered[btn] = nextImg;
        if (btnName.Contains(Constants.PREVIOUS_SCENE)) allButtonsHovered[btn] = prevImg;
        if (btnName.Contains(Constants.CACHE)) allButtonsHovered[btn] = cacheImg;
        if (btnName.Contains(Constants.CONVERSATIONS)) allButtonsHovered[btn] = conversationImg;
        if (btnName.Contains(Constants.CREDITS)) allButtonsHovered[btn] = creditsImg;
        if (btnName.Contains(Constants.LOAD)) allButtonsHovered[btn] = loadImg;
        if (btnName.Contains(Constants.MENU)) allButtonsHovered[btn] = menuImg;
        if (btnName.Contains(Constants.PAUSE)) allButtonsHovered[btn] = pauseImg;
        if (btnName.Contains(Constants.PLAY)) allButtonsHovered[btn] = playImg;
        if (btnName.Contains(Constants.QUIT)) allButtonsHovered[btn] = quitImg;
        if (btnName.Contains(Constants.REMOVE)) allButtonsHovered[btn] = removeImg;
        if (btnName.Contains(Constants.SAVE)) allButtonsHovered[btn] = saveImg;
        if (btnName.Contains(Constants.SEND)) allButtonsHovered[btn] = sendImg;
        if (btnName.Contains(Constants.STOP)) allButtonsHovered[btn] = stopImg;
        if (btnName.Contains(Constants.VOLUME)) allButtonsHovered[btn] = volumeImg;
    }

    public void OnButtonClicked(Button btn)
    {
        if (allButtonsHovered.TryGetValue(btn, out Sprite pressedSprite))
        {
            if (audioSource)
                audioSource.PlayOneShot(buttonClickSound);
            StartCoroutine(SimulatePress(btn, pressedSprite));
        }
    }

    IEnumerator SimulatePress(Button btn, Sprite pressedSprite)
    {
        Image img = btn.GetComponent<Image>();
        Sprite originalSprite = img.sprite;

        img.sprite = pressedSprite;
        yield return new WaitForSeconds(pressDuration);
        img.sprite = originalSprite;
    }
}
