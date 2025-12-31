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
    public bool animFinished = true;
    private int winMax = 3;
    public int playerWin = 0;
    public int botWin = 0;
    public GameObject playerIndicator;
    public GameObject botIndicator;
    public bool checksafe = false;
    public GameObject turnIndicator;
    private Coroutine fadeCoroutine;

    // Standardizing to Color (0-1f) for the Lerp function
    private Color color1 = new Color(0f, 0f, 1f, 0.5f); // Blue 50% Alpha
    private Color color2 = new Color(1f, 0f, 0f, 0.5f);
    [SerializeField] private float duration = 1.0f; // How many seconds the fade should take
    private float colorPercent = 0f;

    private void Start()
    {
        gm = GameObject.Find("GameManager");
        gameManager = gm.GetComponent<GameManagerMTT>();
        if (PlayerPrefs.GetInt("Number of Levels")<= 20)
        {
            winMax = (PlayerPrefs.GetInt("Number of Levels") / 2)+1;
        }
        else
        {
            winMax = 99999;
        }
    }                 
    private void Update()
    {
        Debug.Log("Entered Update");
        playerIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = playerWin.ToString();
        botIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = botWin.ToString();
        if (gameManager.turn == 1)
        {
            StartFade(false, 1f);
        }
        else
        {
            StartFade(true, 1f);
        }
        if (playerWin == winMax && !checksafe)
        {
            Debug.Log("Entered Player Win");
            if (PlayerPrefs.GetString("Game Mode") == "Normal")
            {
                PlayerPrefs.SetString("Last Scene", SceneManager.GetActiveScene().name);
                StartCoroutine(LoadSceneAsyncCoroutine("OPlayerWin"));
                checksafe = true;
            }
            else
            {
                StartCoroutine(Refresh());
                checksafe = true;
            }
        }
        else if (botWin == winMax && !checksafe)
        {
            Debug.Log("Entered Bot Win");
            if (PlayerPrefs.GetString("Game Mode") == "Normal")
            {
                PlayerPrefs.SetString("Last Scene", SceneManager.GetActiveScene().name);
                StartCoroutine(LoadSceneAsyncCoroutine("XPlayerWin"));
                checksafe = true;
            }
            else
            {
                StartCoroutine(Refresh());
                checksafe = true;
            }
        }
        else if (win.WinCondition(gameManager.arr, 1) && !win.WinCondition(gameManager.arr, 2) && !checksafe)
        {
            Debug.Log("Entered Player Score Update");
            PlayerPrefs.SetInt("MineTacToe", PlayerPrefs.GetInt("MineTacToe") + 1);
            playerWin++;
            StartCoroutine(Refresh());
            checksafe = true;
        }
        else if (win.WinCondition(gameManager.arr, 2) && !win.WinCondition(gameManager.arr, 1) && !checksafe)
        {
            Debug.Log("Entered Bot Score Update");
            botWin++;
            StartCoroutine(Refresh());
            checksafe = true;
        }
        if (PlayerPrefs.GetString("Opponent Type") == "Bot")
        {
            BotMode();
            Debug.Log("Entered Against Bot");
        }
        else
        {
            AgainstPlayer();
            Debug.Log("Entered Against Player");
        }

    }
    private void BotMode()
    {
        if (win.WinCondition(gameManager.arr, 2) && win.WinCondition(gameManager.arr, 1) && !checksafe)
        {
            Debug.Log("Entered Winning Draw Bot");
            gameManager.tieBreaker.SetActive(true);
            wsl.Scene1.SetActive(true);
            checksafe = true;
            wsl.specialDraw = true;
        }
        else if (gameManager.turn == 2 && !botIsMoving)
        {
            Debug.Log("Entered Bot Move Checker");
            StartCoroutine(BotMove());
            botIsMoving = true;
        }
        else if (gameManager.turn == 1 && !gameManager.arr.Contains(0) && !checksafe)
        {
            StartCoroutine(Refresh());
            checksafe = true;
        }
    }
    private void AgainstPlayer()
    {
        if (win.WinCondition(gameManager.arr, 2) && win.WinCondition(gameManager.arr, 1) && !checksafe)
        {
            checksafe = true;
            Debug.Log("Entered Winning Draw Player");
            gameManager.tieBreaker2.SetActive(true);
            wsl.Scene01.SetActive(true);
            wsl.specialDraw = true;
        }
        else if (gameManager.turn == 2 && !botIsMoving)
        {
            Debug.Log("Entered 2P move checker");
            if (gameManager.currentButtonClicked2 != -1 && !checksafe)
            {
                checksafe = true;
                Debug.Log("Entered Versus Move init");
                versusMode(gameManager.currentButtonClicked2);
                wsl.player2 = gameManager.currentButtonClicked2;
                gameManager.currentButtonClicked2 = -1;
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
                    gameManager.boardSpecs[gameManager.currentButtonClicked1].GetComponent<ButtonClickMTT>().ClickButton();
                }
                else
                {
                    gameManager.tieBreaker.SetActive(true);
                    wsl.Scene1.SetActive(true);
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
        wsl.Scene1.SetActive(false);
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
        wsl.Scene02.SetActive(false);
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
            gameManager.boardSpecs[gameManager.currentButtonClicked1].GetComponent<ButtonClickMTT>().ClickButton();
            checksafe = false;
            gameManager.currentButtonClicked1 = -1;
            gameManager.currentButtonClicked2 = -1;
        }
        else
        {
            gameManager.tieBreaker2.SetActive(true);
            wsl.Scene01.SetActive(true);
            checksafe = false;
        }
    }
    public IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
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
    public void StartFade(bool toColor2, float duration)
    {
        // Stop any existing fade so they don't fight each other
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        Color targetColor = toColor2 ? color2 : color1;
        fadeCoroutine = StartCoroutine(FadeTo(targetColor, duration));
    }

    IEnumerator FadeTo(Color targetColor, float duration)
    {
        Color startColor = turnIndicator.GetComponent<SpriteRenderer>().color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / duration;

            // Optional: Adds a "SmoothStep" for a more organic feel
            // normalizedTime = Mathf.SmoothStep(0f, 1f, normalizedTime);

            turnIndicator.GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, targetColor, normalizedTime);
            yield return null; // Wait for the next frame
        }

        turnIndicator.GetComponent<SpriteRenderer>().color = targetColor; // Ensure it finishes exactly at the target
    }
}