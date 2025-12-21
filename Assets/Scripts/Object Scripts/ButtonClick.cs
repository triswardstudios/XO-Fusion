using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    private GameObject gm;
    private GameManager gameManager;
    public bool clicked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        gm = GameObject.Find("GameManager");
        gameManager = gm.GetComponent<GameManager>();
    }

    private void OnMouseUp()
    {
        if (!clicked && ((gameManager.turn == 1 && PlayerPrefs.GetString("Opponent Type") == "Bot")||(PlayerPrefs.GetString("Opponent Type") != "Bot")))
        {
            ClickButton();
        }
        else
            Debug.Log("Already Clicked.");
    }

    public void ClickButton()
    {
        gameManager.count++;
        if (gameManager.turn == 1)
        {
            GetComponent<SpriteRenderer>().sprite = gameManager.sprites[0];
            GetComponent<SpriteRenderer>().color = Color.white;
            clicked = true;
            gameManager.turn = 2;
            gameManager.UpdateArray();
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = gameManager.sprites[1];
            GetComponent<SpriteRenderer>().color = Color.white;
            clicked = true;
            gameManager.turn = 1;
            gameManager.UpdateArray();
        }
    }
}