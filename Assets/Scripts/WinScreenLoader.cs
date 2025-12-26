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

    // Update is called once per frame

    public IEnumerator FullScreenAnimation(int botMove, int playerMove, int win)
    {
        Debug.Log("Coroutine Start");
        Scene2.transform.Find("Player").GetComponent<Image>().sprite = PlayerSprites[playerMove];
        Scene2.transform.Find("Bot").GetComponent<Image>().sprite = CPUSprites[botMove];
        yield return null;
        Scene1.SetActive(false);
        //yield return new WaitForSeconds(0.5f);
        Scene2.SetActive(true);
        yield return new WaitForSeconds(2f);
        Scene2.SetActive(false);
        if (win == 0 && !final)
        {
            Scene3.SetActive(true);
            yield return new WaitForSeconds(1f);
            Scene3.SetActive(false);
        }
        else if (win == 1 && !final)
        {
            Scene4.SetActive(true);
            yield return new WaitForSeconds(1f);
            Scene4.SetActive(false);
        }
        else if(!final)
        {
            Debug.Log("entered else");
            Scene1.SetActive(true);
            game.StopAllCoroutines();
        }
        if (!final)
        {
            yield return null;
            self.SetActive(false);
            yield return StartCoroutine(game.FinishingUpdate(win));
        }
        Debug.Log("skipped else");
        if (win == 0 && final)
        {
            Debug.Log("Won");
            if (PlayerPrefs.GetString("Game Mode") == "Normal")
            {
                yield return StartCoroutine(game.LoadSceneAsyncCoroutine("GameWonMTT"));
                game.checksafe = false;
            }
            else
            {
                yield return StartCoroutine(game.Refresh());
                game.checksafe = false;
            }
        }
        else if (win == 1 && final)
        {
            Debug.Log("Lost");
            if (PlayerPrefs.GetString("Game Mode") == "Normal")
            {
                yield return StartCoroutine(game.LoadSceneAsyncCoroutine("GameLoseMTT"));
                game.checksafe = false;
            }
            else
            {
                yield return StartCoroutine(game.Refresh());
                game.checksafe = false;
            }
            
        }
        else if (final)
        {
            Debug.Log("entered else");
            Scene1.SetActive(true);
            game.StopAllCoroutines();
            game.checksafe = false;
        }

    }
    public IEnumerator FullScreenAnimation2P(int secondMove, int firstMove, int win)
    {
        Debug.Log("Coroutine Start");
        Scene22.transform.Find("Player").GetComponent<Image>().sprite = PlayerSprites[firstMove];
        Scene22.transform.Find("Bot").GetComponent<Image>().sprite = CPUSprites[secondMove];
        yield return null;
        Scene02.SetActive(false);
        //yield return new WaitForSeconds(0.5f);
        Scene22.SetActive(true);
        yield return new WaitForSeconds(2f);
        Scene22.SetActive(false);
        if (win == 0 && !final)
        {
            Scene33.SetActive(true);
            yield return new WaitForSeconds(1f);
            Scene33.SetActive(false);
        }
        else if (win == 1 && !final)
        {
            Scene44.SetActive(true);
            yield return new WaitForSeconds(1f);
            Scene44.SetActive(false);
        }
        else if (!final)
        {
            Debug.Log("entered else");
            Scene01.SetActive(true);
            game.StopAllCoroutines();
        }
        if (!final)
        {
            yield return null;
            self2.SetActive(false);
            yield return StartCoroutine(game.FinishingUpdate(win));
        }
        Debug.Log("skipped else");
        if (win == 0 && final)
        {
            Debug.Log("Won");
            PlayerPrefs.SetInt("MineTacToe", PlayerPrefs.GetInt("MineTacToe") + 1);
            game.playerWin++;
            yield return StartCoroutine(game.Refresh());
            game.checksafe = false;
        }
        else if (win == 1 && final)
        {
            Debug.Log("Lost");
            game.botWin++;
            yield return StartCoroutine(game.Refresh());
            game.checksafe = false;

        }
        else if (final)
        {
            Debug.Log("entered else");
            Scene01.SetActive(true);
            game.StopAllCoroutines();
            game.checksafe = false;
        }

    }
    public void setPlayer1(int val)
    {
        player1 = val;
    }
}