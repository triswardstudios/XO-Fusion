using UnityEngine;

public class SoundSettingController : MonoBehaviour
{
    public GameObject bgmSlider;
    public GameObject sfxSlider;
    public GameObject muteToggle;
    void Awake()
    {
        sfxSlider = GameObject.Find("SFX Slider");
        bgmSlider = GameObject.Find("BGM Slider");
        muteToggle = GameObject.Find("Sound Toggle");
        sfxSlider.GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("SFX Volume");
        bgmSlider.GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("BGM Volume");
        muteToggle.GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("Mute");
    }
}
