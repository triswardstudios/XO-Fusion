using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenLoader : MonoBehaviour
{
    public GameObject self;
    public GameObject self2;
    public GameObject Scene01;
    public GameObject Scene02;
    public GameObject Scene22;
    public GameObject Scene33;
    public GameObject Scene44;
    public GameObject Scene1;
    public GameObject Scene2;
    public GameObject Scene3;
    public GameObject Scene4;
    public int botMove;
    public int playerMove;
    public Sprite[] CPUSprites = new Sprite[3];
    public Sprite[] PlayerSprites = new Sprite[3];
    public GameMTT game;
    public bool final = false;
    public int player1 = -1;
    public int player2 = -1;
    public bool specialDraw = false;
    public TicTacToeLogic win1 = new();

    // Update is called once per frame

    public IEnumerator FullScreenAnimation(int botMove, int playerMove, int win)
    {
        Debug.Log("Coroutine Start");
        Scene2.transform.Find("Player").GetComponent<Image>().sprite = PlayerSprites[playerMove];
        Scene2.transform.Find("Bot").GetComponent<Image>().sprite = CPUSprites[botMove];
        yield return null;
        Scene1.SetActive(false);
        Scene2.SetActive(true);
        yield return new WaitForSeconds(2f);
        Scene2.SetActive(false);
        if (win == 0)
        {
            Scene3.SetActive(true);
            yield return new WaitForSeconds(1f);
            Scene3.SetActive(false);
            if(specialDraw)
            {
                PlayerPrefs.SetInt("MineTacToe", PlayerPrefs.GetInt("MineTacToe") + 1);
                game.playerWin++;
                game.checksafe = false;
                yield return StartCoroutine(game.Refresh());
            }
            self.SetActive(false);
            Scene1.SetActive(true);
        }
        else if (win == 1)
        {
            Scene4.SetActive(true);
            yield return new WaitForSeconds(1f);
            Scene4.SetActive(false);
            if (specialDraw)
            {
                game.botWin++;
                game.checksafe = false;
                yield return StartCoroutine(game.Refresh());
            }
            self.SetActive(false);
            Scene1.SetActive(true);
        }
        else
        {
            Debug.Log("entered else");
            Scene1.SetActive(true);
            game.checksafe = false;
            game.StopAllCoroutines();
        }
        yield return null;
        self.SetActive(false);
        if(!specialDraw)
        {
            yield return StartCoroutine(game.FinishingUpdate(win));
        }
        specialDraw = false;
    }
    public IEnumerator FullScreenAnimation2P(int secondMove, int firstMove, int win)
    {
        Debug.Log("Coroutine Start 2P");
        Scene22.transform.Find("Player").GetComponent<Image>().sprite = PlayerSprites[firstMove];
        Scene22.transform.Find("Bot").GetComponent<Image>().sprite = CPUSprites[secondMove];
        yield return null;
        Scene02.SetActive(false);
        Scene22.SetActive(true);
        yield return new WaitForSeconds(2f);
        Scene22.SetActive(false);
        if (win == 0)
        {
            Scene33.SetActive(true);
            yield return new WaitForSeconds(1f);
            Scene33.SetActive(false);
            if (specialDraw)
            {
                PlayerPrefs.SetInt("MineTacToe", PlayerPrefs.GetInt("MineTacToe") + 1);
                game.playerWin++;
                game.checksafe = false;
                yield return StartCoroutine(game.Refresh());
            }
            self2.SetActive(false);
            Scene01.SetActive(true);
        }
        else if (win == 1)
        {
            Scene44.SetActive(true);
            yield return new WaitForSeconds(1f);
            Scene44.SetActive(false);
            if (specialDraw)
            {
                game.botWin++;
                game.checksafe = false;
                yield return StartCoroutine(game.Refresh());
            }
            self2.SetActive(false);
            Scene01.SetActive(true);
        }
        else
        {
            Debug.Log("entered else");
            Scene01.SetActive(true);
            game.checksafe = false;
            game.StopAllCoroutines();
        }

        yield return null;
        self2.SetActive(false);
        if (!specialDraw)
        {
            yield return StartCoroutine(game.FinishingUpdate(win));
        }
        specialDraw = false;
    }
    public void setPlayer1(int val)
    {
        player1 = val;
    }
}