using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ModeButton : MonoBehaviour
{
    public Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.GetComponent<Text>().text = PlayerPrefs.GetString("Game Mode");
    }

    // Update is called once per frame
    public void ChangeMode()
    {
        if (PlayerPrefs.GetString("Game Mode") == "Normal")
        {
            PlayerPrefs.SetString("Game Mode", "Infinite");
            text.GetComponent<Text>().text = PlayerPrefs.GetString("Game Mode");
        }
        else
        {
            PlayerPrefs.SetString("Game Mode", "Normal");
            text.GetComponent<Text>().text = PlayerPrefs.GetString("Game Mode");
        }
    }
}
