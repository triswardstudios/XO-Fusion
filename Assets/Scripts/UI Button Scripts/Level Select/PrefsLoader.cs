using UnityEngine;
using UnityEngine.UI;

public class PrefsLoader : MonoBehaviour
{
    public string prefKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObject.GetComponent<Slider>() != null) { 
            gameObject.GetComponent<Slider>().value = PlayerPrefs.GetInt(prefKey);
        }
    }
}
