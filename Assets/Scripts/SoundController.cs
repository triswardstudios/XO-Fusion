using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public GameObject bgmSlider;
    public GameObject sfxSlider;
    public GameObject bgmSource;
    public GameObject sfxSource;
    public GameObject muteToggle;
    public GameObject sfxDisplay;
    public GameObject bgmDisplay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(sfxSlider != null)
        {
            sfxSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("SFX Volume");
            Debug.LogError(PlayerPrefs.GetFloat("SFX Volume"));
        }
        if(bgmSlider != null)
        {
            bgmSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("BGM Volume");
            Debug.LogError(PlayerPrefs.GetFloat("BGM Volume"));
        }
        if(bgmSource == null)
        {
            Debug.LogError("BGM Source GameObject is not assigned in SoundController.");
        }
        if(sfxSource == null)
        {
            Debug.LogError("SFX Source GameObject is not assigned in SoundController.");
        }
        if(muteToggle != null)
        {
            muteToggle.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Mute");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sfxSlider != null) { 
            if (muteToggle.GetComponent<Slider>().value == 1)
            {
                if(PlayerPrefs.GetFloat("Mute") != 1)
                {
                    PlayerPrefs.SetFloat("Mute", 1);
                }
                if (bgmSlider.GetComponent<Slider>().value < 10)
                {
                    bgmDisplay.GetComponent<TextMeshProUGUI>().text = "0"+ bgmSlider.GetComponent<Slider>().value.ToString("0");
                    
                }

                else
                {
                    bgmDisplay.GetComponent<TextMeshProUGUI>().text = bgmSlider.GetComponent<Slider>().value.ToString("0");
                }
                if (sfxSlider.GetComponent<Slider>().value < 10)
                {
                    sfxDisplay.GetComponent<TextMeshProUGUI>().text = "0"+ sfxSlider.GetComponent<Slider>().value.ToString("0");
                }

                else
                {
                    sfxDisplay.GetComponent<TextMeshProUGUI>().text = sfxSlider.GetComponent<Slider>().value.ToString("0");
                }

                bgmSource.GetComponent<AudioSource>().volume = bgmSlider.GetComponent<Slider>().value/100;
                PlayerPrefs.SetFloat("BGM Volume", bgmSlider.GetComponent<Slider>().value);
                sfxSource.GetComponent<AudioSource>().volume = sfxSlider.GetComponent<Slider>().value/100;
                
                PlayerPrefs.SetFloat("SFX Volume", sfxSlider.GetComponent<Slider>().value);
            }
            else
            {
                if(bgmSource.GetComponent<AudioSource>().volume != 0 || sfxSource.GetComponent<AudioSource>().volume != 0)
                {
                    PlayerPrefs.SetFloat("BGM Volume", bgmSlider.GetComponent<Slider>().value);
                    PlayerPrefs.SetFloat("SFX Volume", sfxSlider.GetComponent<Slider>().value);
                }
                bgmSource.GetComponent<AudioSource>().volume = 0;
                sfxSource.GetComponent<AudioSource>().volume = 0;
                PlayerPrefs.SetFloat("Mute", 0);
            }
        }
        else
        {
            if(GameObject.Find("SFX") != null)
            {
                sfxSlider = GameObject.Find("SFX Slider");
                bgmSlider = GameObject.Find("BGM Slider");
                bgmSource = GameObject.Find("BGM");
                sfxSource = GameObject.Find("SFX");
                muteToggle = GameObject.Find("Sound Toggle");
                sfxDisplay = GameObject.Find("SFX Display");
                bgmDisplay = GameObject.Find("BGM Display");
            }

            else
            {
                Debug.LogError("No audio adjusting present");
            }
        }
    }
}
