using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private GameObject gm;
    private GameManager gameManager;
    private bool botIsMoving = false;
    public TicTacToeLogic win = new();

    public int playerWon = 0;
    public int botWon = 0;
    public int winMax = 3;
    public GameObject playerIndicator;
    public GameObject botIndicator;
    public bool refreshing = true;

    private void Start()
    {
        gm = GameObject.Find("GameManager");
        gameManager = gm.GetComponent<GameManager>();
        if (PlayerPrefs.GetInt("Number of Levels") % 2 == 0 && PlayerPrefs.GetInt("Number of Levels")>=3)
        {
            winMax = PlayerPrefs.GetInt("Number of Levels") / 2;
        }
        else
        {
            winMax = (PlayerPrefs.GetInt("Number of Levels") / 2) + 1;
        }
    }

    private void Update()
    {
        playerIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = playerWon.ToString();
        botIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = botWon.ToString();
        if (playerWon == winMax)
        {
            if (PlayerPrefs.GetString("Game Mode") == "Normal")
            {
                StartCoroutine(LoadSceneAsyncCoroutine("GameWonTTT"));
            }
            else
            {
                StartCoroutine(Refresh());
                refreshing = false;
            }
        }
        else if (botWon == winMax)
        {
            if (PlayerPrefs.GetString("Game Mode") == "Normal")
            {
                StartCoroutine(LoadSceneAsyncCoroutine("GameLoseTTT"));
            }
            else
            {
                StartCoroutine(Refresh());
                refreshing = false;
            }
        }
        if (win.WinCondition(gameManager.arr, 1) && refreshing)
        {
            StartCoroutine(Refresh());
            refreshing = false;
        }
        else if (win.WinCondition(gameManager.arr, 2) && refreshing)
        {
            StartCoroutine(Refresh());
            refreshing = false;
        }
        else if (gameManager.turn == 2 && !botIsMoving)
        {
            if (PlayerPrefs.GetString("Opponent Type") == "Bot")
            {
                StartCoroutine(BotMove());
                botIsMoving = true;
            }
        }
        else if (gameManager.turn == 1 && !gameManager.arr.Contains(0) && refreshing)
        {
            StartCoroutine(Refresh());
            refreshing = false;
        }
    }

    private IEnumerator BotMove()
    {
        botIsMoving = true;

        // Wait for 2 seconds before making the bot move
        yield return new WaitForSeconds(1f);

        NormalBotLogic bot = new NormalBotLogic();

        if (gameManager.turn == 2)
        {
            int move = bot.MakeMoveNormal(gameManager.count, gameManager.turn, gameManager.arr);
            if (move != -1)
            {
                gameManager.boardSpecs[move].GetComponent<ButtonClick>().ClickButton();
            }
            else
            {
                foreach (var button in gameManager.boardSpecs)
                {
                    button.GetComponent<SpriteRenderer>().sprite = gameManager.defaultSprite;
                    Color temp = button.GetComponent<SpriteRenderer>().color;
                    temp.a = 0f;
                    button.GetComponent<SpriteRenderer>().color = temp;
                    button.GetComponent<ButtonClick>().clicked = false;
                    gameManager.UpdateArray();
                }
            }

            gameManager.turn = 1;
        }

        botIsMoving = false;
    }

    private IEnumerator Refresh()
    {
        if (win.WinCondition(gameManager.arr, 1))
        {
            PlayerPrefs.SetInt("TicTacToe", PlayerPrefs.GetInt("TicTacToe") + 1);
            playerWon++;
            refreshing = false;
        }
        else if (win.WinCondition(gameManager.arr, 2))
        {
            botWon++;
            refreshing = false;
        }
        //gameManager.turn = 1;
        yield return new WaitForSeconds(1f);
        foreach (var button in gameManager.boardSpecs)
        {
            button.GetComponent<SpriteRenderer>().sprite = gameManager.defaultSprite;
            Color temp = button.GetComponent<SpriteRenderer>().color;
            temp.a = 0f;
            button.GetComponent<SpriteRenderer>().color = temp;
            button.GetComponent<ButtonClick>().clicked = false;
            gameManager.UpdateArray();
        }
        refreshing = true;
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        // While the scene is still loading, display progress
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading progress: " + asyncLoad.progress * 100 + "%");
            yield return null; // Wait for the next frame
        }

        Debug.Log("Scene loaded: " + sceneName);
    }
}