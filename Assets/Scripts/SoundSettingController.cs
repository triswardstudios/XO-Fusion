using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingController : MonoBehaviour
{
    public GameObject bgmSlider;
    public GameObject sfxSlider;
    public GameObject sfxDisplay;
    public GameObject bgmDisplay;
    public GameObject muteToggle;
    void Awake()
    {
        sfxSlider = GameObject.Find("SFX Slider");
        bgmSlider = GameObject.Find("BGM Slider");
        muteToggle = GameObject.Find("Sound Toggle");
        sfxDisplay = GameObject.Find("SFX Display");
        bgmDisplay = GameObject.Find("BGM Display");
        sfxSlider.GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("SFX Volume");
        bgmSlider.GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("BGM Volume");
        muteToggle.GetComponent<UnityEngine.UI.Slider>().value = PlayerPrefs.GetFloat("Mute");
        if (bgmSlider.GetComponent<Slider>().value < 10)
        {
            bgmDisplay.GetComponent<TextMeshProUGUI>().text = "0" + bgmSlider.GetComponent<Slider>().value.ToString("0");

        }

        else
        {
            bgmDisplay.GetComponent<TextMeshProUGUI>().text = bgmSlider.GetComponent<Slider>().value.ToString("0");
        }
        if (sfxSlider.GetComponent<Slider>().value < 10)
        {
            sfxDisplay.GetComponent<TextMeshProUGUI>().text = "0" + sfxSlider.GetComponent<Slider>().value.ToString("0");
        }

        else
        {
            sfxDisplay.GetComponent<TextMeshProUGUI>().text = sfxSlider.GetComponent<Slider>().value.ToString("0");
        }
    }
}
