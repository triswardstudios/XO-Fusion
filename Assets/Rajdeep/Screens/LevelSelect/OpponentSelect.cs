using UnityEngine;
using UnityEngine.UI;

public class OpponentSelect : MonoBehaviour
{
    [Header("Opponent Sprites")]
    public Sprite player;
    public Sprite bot;
    public Image self;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("Opponent Type"))
        {
            if (PlayerPrefs.GetString("Opponent Type") == "Versus")
            {
                self.sprite = player;
            }
            else
            {
                self.sprite = bot;
            }
        }
        else
        {
            PlayerPrefs.SetString("Opponent Type", "Bot");
            self.sprite = bot;
        }
    }

    // Update is called once per frame
    public void ChangeOnClick()
    {
        if(PlayerPrefs.GetString("Opponent Type") == "Bot")
        {
            PlayerPrefs.SetString("Opponent Type", "Versus");
            GetComponent<Image>().sprite = player;
        }
        else
        {
            PlayerPrefs.SetString("Opponent Type", "Bot");
            GetComponent<Image>().sprite = bot;
        }
    }
}
