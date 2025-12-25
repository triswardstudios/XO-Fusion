using TMPro;
using UnityEngine;

public class LevelDisplayer : MonoBehaviour
{
    public GameObject levelSlider;
    void Update()
    {
        if (levelSlider != null)
        {
            int levelValue = (int)levelSlider.GetComponent<UnityEngine.UI.Slider>().value;
            
            if (levelValue <10)
            {
                PlayerPrefs.SetString("Game Mode", "Normal");
                PlayerPrefs.SetInt("Number of Levels", levelValue);
                gameObject.GetComponent<TextMeshProUGUI>().text = "0" + levelValue.ToString();
            }
            else if(levelValue < levelSlider.GetComponent<UnityEngine.UI.Slider>().maxValue)
            {
                PlayerPrefs.SetString("Game Mode", "Normal");
                PlayerPrefs.SetInt("Number of Levels", levelValue);
                gameObject.GetComponent<TextMeshProUGUI>().text = levelValue.ToString();
            }
            else
            {
                PlayerPrefs.SetString("Game Mode", "Infinite");
                gameObject.GetComponent<TextMeshProUGUI>().text = "∞";
            }
        }
    }
}
