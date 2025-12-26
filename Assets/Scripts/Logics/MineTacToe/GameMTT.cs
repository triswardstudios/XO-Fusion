using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Random = System.Random;

public class GameMTT : MonoBehaviour
{
    private GameObject gm;
    private GameManagerMTT gameManager;
    private bool botIsMoving = false;
    public TicTacToeLogic win = new();
    private int botMove = -1;
    private bool tieBreak = false;
    public int botRPS;
    public WinScreenLoader wsl;
    public bool timeUp = true;
    public bool time = true;
    public GameObject timeIndicator;
    public LevelLoader levelLoader;
    private bool loaded = false;
    public bool animFinished = true;
    private Coroutine timerCoroutine;
    private int winMax = 3;
    public int playerWin = 0;
    public int botWin = 0;
    public GameObject playerIndicator;
    public GameObject botIndicator;
    public bool checksafe = false;

    private void Start()
    {
        gm = GameObject.Find("GameManager");
        gameManager = gm.GetComponent<GameManagerMTT>();
        if (PlayerPrefs.GetInt("Number of Levels") % 2 == 0 && PlayerPrefs.GetInt("Number of Levels") >= 3)
        {
            winMax = (PlayerPrefs.GetInt("Number of Levels") / 2)+1;
        }
        else
        {
            winMax = (PlayerPrefs.GetInt("Number of Levels") / 2) + 1;
        }
    }                 
    private void Update()
    {
        Debug.Log("Entered Update");
        playerIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = playerWin.ToString();
        botIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = botWin.ToString();
        if (playerWin == winMax && !checksafe)
        {
            Debug.Log("Entered Player Win");
            if (PlayerPrefs.GetString("Game Mode") == "Normal")
            {
                StartCoroutine(LoadSceneAsyncCoroutine("GameWonMTT"));
                checksafe = true;
            }
            else
            {
                StartCoroutine(Refresh());
                checksafe = true;
            }
        }
        else if(botWin == winMax && !checksafe)
        {
            Debug.Log("Entered Bot Win");
            if (PlayerPrefs.GetString("Game Mode") == "Normal")
            {
                StartCoroutine(LoadSceneAsyncCoroutine("GameLoseMTT"));
                checksafe = true;
            }
            else
            {
                StartCoroutine(Refresh());
                checksafe = true;
            }
        }
        if (win.WinCondition(gameManager.arr, 1) && !win.WinCondition(gameManager.arr, 2) && !loaded && !checksafe)
        {
            Debug.Log("Entered Player Score Update");
            PlayerPrefs.SetInt("MineTacToe", PlayerPrefs.GetInt("MineTacToe") + 1);
            playerWin++;
            StartCoroutine(Refresh());
            checksafe = true;
        }
        else if (win.WinCondition(gameManager.arr, 2) && !win.WinCondition(gameManager.arr, 1) && !loaded && !checksafe) 
        {
            Debug.Log("Entered Bot Score Update");
            botWin++;
            StartCoroutine(Refresh());
            checksafe = true;
        }
        else if (win.WinCondition(gameManager.arr, 2) && win.WinCondition(gameManager.arr, 1) && !loaded && !checksafe)
        {
            Debug.Log("Entered Winning Draw");
            gameManager.tieBreaker.SetActive(true);
            if (!wsl.final)
            { wsl.Scene1.SetActive(true); }
            timeUp = false;
            wsl.final = true;
            checksafe = true;
        }
        else if (gameManager.turn == 2 && !botIsMoving && !checksafe)
        {
            Debug.Log("Entered 2P move checker");
            if (PlayerPrefs.GetString("Opponent Type") == "Bot")
            {
                Debug.Log("Entered Bot Move");
                StartCoroutine(BotMove());
                botIsMoving = true;
                checksafe = true;
            }
            else
            {
                Debug.Log("Entered 2P Move");
                if (gameManager.currentButtonClicked2 != -1 &&!checksafe)
                {
                    checksafe = true;
                    Debug.Log("Entered Versus Move init");
                    versusMode(gameManager.currentButtonClicked2);
                    wsl.player2 = gameManager.currentButtonClicked2;
                    gameManager.currentButtonClicked2 = -1;
                }
            }
        }
        else if (gameManager.turn == 1 && !gameManager.arr.Contains(0) && !checksafe)
        {
            StartCoroutine(Refresh());
            checksafe = true;
        }
    }

    private IEnumerator BotMove()
    {
        botIsMoving = true;
        // Wait for 2 seconds before making the bot move
        yield return new WaitForSeconds(1f);

        MineTacToeBotLogic bot = new();

        if (gameManager.turn == 2 && !tieBreak)
        {
            int move = bot.MakeMoveMTT(gameManager.count, gameManager.turn, gameManager.arr);
            botMove = move;
            if (move != -1)
            {
                if (gameManager.currentButtonClicked1 != move)
                {
                    gameManager.boardSpecs[move].GetComponent<ButtonClickMTT>().ClickButton();
                    gameManager.turn = 1;
                    timeUp = false;
                    gameManager.boardSpecs[gameManager.currentButtonClicked1].GetComponent<ButtonClickMTT>().ClickButton();
                    time = true;
                }
                else
                {
                    gameManager.tieBreaker.SetActive(true);
                    wsl.Scene1.SetActive(true);
                    timeUp = false;
                }
            }
            else
            {
                foreach (var button in gameManager.boardSpecs)
                {
                    button.GetComponent<SpriteRenderer>().sprite = button.GetComponent<ButtonClickMTT>().originalSprite;
                    button.GetComponent<SpriteRenderer>().color = button.GetComponent<ButtonClickMTT>().originalColor;
                    button.GetComponent<ButtonClickMTT>().clicked = false;
                    gameManager.UpdateArray();
                }
            }

            gameManager.turn = 1;
        }

        botIsMoving = false;
        checksafe = false;
    }

    public IEnumerator Refresh()
    {
        yield return new WaitForSeconds(1f);
        foreach (var button in gameManager.boardSpecs)
        {
            button.GetComponent<SpriteRenderer>().sprite = button.GetComponent<ButtonClickMTT>().originalSprite;
            button.GetComponent<SpriteRenderer>().color = button.GetComponent<ButtonClickMTT>().originalColor;
            button.GetComponent<ButtonClickMTT>().clicked = false;
            gameManager.UpdateArray();
        }
        checksafe = false;
    }

    public void RockPaperScissors(int num)
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        tieBreak = true;
        
        botRPS = random.Next(0, 3);
        if ((num == 0 && botRPS == 2) || (num == 1 && botRPS == 0) || (num == 2 && botRPS == 1))
        {
            StartCoroutine(wsl.FullScreenAnimation(botRPS, num, 0));
        }
        else if (num == botRPS)
        {
            StartCoroutine(wsl.FullScreenAnimation(botRPS, num, 2));
        }
        else
        {
            StartCoroutine(wsl.FullScreenAnimation(botRPS, num, 1));
        }
        time = true;
        tieBreak = false;
        gameManager.UpdateArray();
        checksafe = false;
    }

    public void RockPaperScissors2(int num2)
    {
        int num = wsl.player1;
        if ((num == 0 && num2 == 2) || (num == 1 && num2 == 0) || (num == 2 && num2 == 1))
        {
            StartCoroutine(wsl.FullScreenAnimation2P(num2, num, 0));
        }
        else if (num == num2)
        {
            StartCoroutine(wsl.FullScreenAnimation2P(num2, num, 2));
        }
        else
        {
            StartCoroutine(wsl.FullScreenAnimation2P(num2, num, 1));
        }
        wsl.setPlayer1(-1);
        time = true;
        tieBreak = false;
        gameManager.UpdateArray();
        checksafe = false;
    }

    public IEnumerator FinishingUpdate(int win)
    {
        yield return null;
        if (win == 0)
        {
            gameManager.turn = 1;
            gameManager.boardSpecs[gameManager.currentButtonClicked1].GetComponent<ButtonClickMTT>().ClickButton();
            Debug.Log("Player wins!");
            time = true;
        }
        else if (win == 1)
        {
            gameManager.turn = 2;
            if (PlayerPrefs.GetString("Opponent Type") == "Bot")
            {
                gameManager.boardSpecs[botMove].GetComponent<ButtonClickMTT>().ClickButton();
            }
            else
            {
                gameManager.boardSpecs[wsl.player2].GetComponent<ButtonClickMTT>().ClickButton();
                wsl.player2 = -1;
            }
            gameManager.turn = 1;
            time = true;
            Debug.Log("Bot Wins!");
        }
        else
        {
            gameManager.turn = 1;
            time = true;
            Debug.Log("Draw!");
        }

    }

    public void versusMode(int secondMove)
    {
        Debug.Log("Entered versus mode");
        checksafe = false;
        if (gameManager.currentButtonClicked1 != secondMove)
        {
            gameManager.boardSpecs[secondMove].GetComponent<ButtonClickMTT>().ClickButton();
            gameManager.turn = 1;
            timeUp = false;
            gameManager.boardSpecs[gameManager.currentButtonClicked1].GetComponent<ButtonClickMTT>().ClickButton();
            time = true;
            checksafe = false;
            gameManager.currentButtonClicked1 = -1;
            gameManager.currentButtonClicked2 = -1;
        }
        else
        {
            gameManager.tieBreaker2.SetActive(true);
            wsl.Scene01.SetActive(true);
            timeUp = false;
            checksafe = false;
        }
    }

    //public IEnumerator Timer()
    //{
    //    while (time)
    //    {
    //        int currentTime = int.Parse(timeIndicator.GetComponent<TMPro.TextMeshProUGUI>().text);

    //        while (currentTime > 0)
    //        {
    //            timeUp = false;
    //            yield return new WaitForSeconds(1f);
    //            currentTime--;
    //            timeIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = currentTime.ToString();
    //        }

    //        // When it reaches 0
    //        timeUp = false;
    //        yield return new WaitForSeconds(1f);
    //        timeIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = "5";

    //        // Optional: switch turn or handle logic here
    //        // gameManager.turn = (gameManager.turn == 1) ? 2 : 1;
    //    }
    //}

    public IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        loaded = true;
        yield return new WaitForSeconds(2f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        // While the scene is still loading, display progress
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading progress: " + asyncLoad.progress * 100 + "%");
            yield return null; // Wait for the next frame
        }

        Debug.Log("Scene loaded: " + sceneName);
    }

    //public void StopTimer()
    //{
    //    if (timerCoroutine != null)
    //    {
    //        StopCoroutine(timerCoroutine);
    //        timerCoroutine = null;
    //    }

    //    // Reset display
    //    //timeIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = "5";
    //    timeUp = false;
    //}
}