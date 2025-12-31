using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private GameObject gm;
    private GameManager gameManager;
    private GameManagerFTT gameManagerFTT;
    public bool botIsMoving = false;
    public TicTacToeLogic win = new();

    public int playerWon = 0;
    public int botWon = 0;
    public int winMax = 3;
    public GameObject playerIndicator;
    public GameObject botIndicator;
    public bool refreshing = true;
    public GameObject turnIndicator;
    public float time = 2f;
    private Coroutine fadeCoroutine;

    // Standardizing to Color (0-1f) for the Lerp function
    private Color color1 = new Color(0f, 0f, 1f, 0.5f); // Blue 50% Alpha
    private Color color2 = new Color(1f, 0f, 0f, 0.5f);
    [SerializeField] private float duration = 1.0f; // How many seconds the fade should take
    private float colorPercent = 0f;

    private void Start()
    {
        gm = GameObject.Find("GameManager");
        gameManager = gm.GetComponent<GameManager>();
        if (PlayerPrefs.GetInt("Number of Levels") <= 20)
        {
            winMax = (PlayerPrefs.GetInt("Number of Levels") / 2) + 1;
        }
        else
        {
            winMax = 99999;
        }
    }

    private void Update()
    {
        playerIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = playerWon.ToString();
        botIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = botWon.ToString();
        if (gameManager.turn == 1)
        {
            StartFade(false, 1f);
        }
        else
        {
            StartFade(true, 1f);
        }
        
        if (playerWon == winMax)
        {
            if (PlayerPrefs.GetString("Game Mode") == "Normal")
            {
                PlayerPrefs.SetString("Last Scene", SceneManager.GetActiveScene().name);
                StartCoroutine(LoadSceneAsyncCoroutine("OPlayerWin"));
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
                PlayerPrefs.SetString("Last Scene", SceneManager.GetActiveScene().name);
                StartCoroutine(LoadSceneAsyncCoroutine("XPlayerWin"));
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
        else if (gameManager.turn == 2 && !botIsMoving && refreshing)
        {
            if (PlayerPrefs.GetString("Opponent Type") == "Bot")
            {
                StartCoroutine(BotMove());
                botIsMoving = true;
            }
        }
        if (gameManager.turn == 1 && !gameManager.arr.Contains(0) && refreshing)
        {

            StartCoroutine(Refresh());
            refreshing = false;
        }
    }

    private IEnumerator BotMove()
    {
        Debug.Log("Bot is making a move...");
        botIsMoving = true;

        // Wait for 2 seconds before making the bot move
        yield return new WaitForSeconds(1f);

        NormalBotLogic bot = new NormalBotLogic();
        int move = bot.MakeMoveNormal(gameManager.count, gameManager.turn, gameManager.arr);
        if (move != -1)
        {
            gameManager.boardSpecs[move].GetComponent<ButtonClick>().ClickButton();
            gameManager.turn = 1;
        }

        else
        {

            StartCoroutine(Refresh());
            refreshing = false;
        }

        botIsMoving = false;
    }

    private IEnumerator Refresh()
    {
        Debug.Log("Refreshing the board...");
        if (win.WinCondition(gameManager.arr, 1))
        {
            PlayerPrefs.SetInt("TicTacToe", PlayerPrefs.GetInt("TicTacToe") + 1);
            playerWon++;
            //refreshing = false;
        }
        else if (win.WinCondition(gameManager.arr, 2))
        {
            botWon++;
            //refreshing = false;
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