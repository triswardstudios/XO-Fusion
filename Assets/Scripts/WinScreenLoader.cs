using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenLoader : MonoBehaviour
{
    public GameObject self;
    public GameObject Scene1;
    public GameObject Scene2;
    public GameObject Scene3;
    public GameObject Scene4;
    public int botMove;
    public int playerMove;
    public Sprite[] CPUSprites = new Sprite[3];
    public Sprite[] PlayerSprites = new Sprite[3];
    public GameMTT game;

    // Update is called once per frame

    public IEnumerator FullScreenAnimation(int botMove, int playerMove, int win)
    {
        Scene2.transform.Find("Player").GetComponent<Image>().sprite = PlayerSprites[playerMove];
        Scene2.transform.Find("Bot").GetComponent<Image>().sprite = CPUSprites[botMove];
        yield return null;
        Scene1.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Scene2.SetActive(true);
        yield return new WaitForSeconds(2f);
        Scene2.SetActive(false);
        if (win == 0)
        {
            Scene3.SetActive(true);
            yield return new WaitForSeconds(1f);
            Scene3.SetActive(false);
        }
        else if (win == 1)
        {
            Scene4.SetActive(true);
            yield return new WaitForSeconds(1f);
            Scene4.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
        self.SetActive(false);
        yield return StartCoroutine(game.FinishingUpdate(win));
    }
}