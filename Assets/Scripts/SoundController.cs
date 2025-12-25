using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public GameObject bgmSlider;
    public GameObject sfxSlider;
    public GameObject bgmSource;
    public GameObject sfxSource;
    public GameObject muteToggle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(sfxSlider == null)
        {
            Debug.LogError("SFX Slider GameObject is not assigned in SoundController.");
        }
        if(bgmSlider == null)
        {
            Debug.LogError("BGM Slider GameObject is not assigned in SoundController.");
        }
        if(bgmSource == null)
        {
            Debug.LogError("BGM Source GameObject is not assigned in SoundController.");
        }
        if(sfxSource == null)
        {
            Debug.LogError("SFX Source GameObject is not assigned in SoundController.");
        }
        if(muteToggle == null)
        {
            Debug.LogError("Mute Toggle GameObject is not assigned in SoundController.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sfxSlider != null) { 
            if (muteToggle.GetComponent<Slider>().value == 1)
            {
                bgmSource.GetComponent<AudioSource>().volume = bgmSlider.GetComponent<Slider>().value;
                sfxSource.GetComponent<AudioSource>().volume = sfxSlider.GetComponent<Slider>().value;
            }
            else
            {
                bgmSource.GetComponent<AudioSource>().volume = 0;
                sfxSource.GetComponent<AudioSource>().volume = 0;
            }
        }
    }
}
