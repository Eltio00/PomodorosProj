using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    [Header("Audio Prefab")]
    [SerializeField] private GameObject popupVolume;
    [SerializeField] private TextMeshProUGUI sfxText;
    [SerializeField] private TextMeshProUGUI musicText;
    
    [Header("Audio Sprites")]    
    [SerializeField] private Sprite[] audioSprites;
    
    private AudioSource sfxSource;
    private Slider sfxSlider;
    private Slider musicSlider;
    private float sfxMaxVolume;
    private bool sameImage = false;
    
    private Image handleImage;
    void Start()
    {
        sfxSlider = GameObject.Find(Constants.SFX_SLIDER).GetComponent<Slider>();
        musicSlider = GameObject.Find(Constants.MUSIC_SLIDER).GetComponent<Slider>();
        sfxSource = this.GetComponent<AudioSource>();
        sfxMaxVolume = sfxSource.volume;
        handleImage = sfxSlider.handleRect.GetComponent<Image>();
        popupVolume.SetActive(false);
        sfxSlider.onValueChanged.AddListener(UpdateSfx);
        musicSlider.onValueChanged.AddListener(UpdateMusic);
        UpdateSfx(sfxSlider.value);
        UpdateMusic(musicSlider.value);
    }
    private void UpdateSfx(float value)
    {
        
        sfxText.text = $"{Mathf.RoundToInt(value * 100)}";
        sfxSource.volume = value;
        if (value <= 0)
        {
            handleImage.sprite = audioSprites[0];
        }
        else if (value > 0 &&  value <= (sfxMaxVolume * 0.25f))
        {
            handleImage.sprite = audioSprites[1];
        }
        else if (value > (sfxMaxVolume * 0.25f) &&  value <= (sfxMaxVolume * 0.5f))
        {
            handleImage.sprite = audioSprites[2];
        }
        else if (value > (sfxMaxVolume * 0.5f))
        {
            handleImage.sprite = audioSprites[3];
        }
    }
    private void UpdateMusic(float value)
    {
        //musicSource.volume = value;
        musicText.text = $"{Mathf.RoundToInt(value * 100)}";
    
        if (value <= 0)
        {
            handleImage.sprite = audioSprites[0];
        }
        else if (value > 0 &&  value <= (sfxMaxVolume * 0.25f))
        {
            handleImage.sprite = audioSprites[1];
        }
        else if (value > (sfxMaxVolume * 0.25f) &&  value <= (sfxMaxVolume * 0.5f))
        {
            handleImage.sprite = audioSprites[2];
        }
        else if (value > (sfxMaxVolume * 0.5f))
        {
            handleImage.sprite = audioSprites[3];
        }
    }

    public void OpenPopup()
    {
        popupVolume.SetActive(true);
    }
    public void ClosePopup()
    {
        popupVolume.SetActive(false);
    }
}