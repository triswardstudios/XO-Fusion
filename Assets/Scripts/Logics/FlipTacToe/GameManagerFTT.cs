using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManagerFTT : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[3];
    public Sprite defaultSprite;

    [Header("Selectable Objects")]
    public List<FTTButtonClick> selectableObjects = new();

    [Header("Selection Settings")]
    public int maxSelections = 3;

    public Color highlightColor = Color.yellow;
    public float resetDelay = 2f; // seconds before reset

    public List<FTTButtonClick> selectedObjects = new();
    private bool isRevealing = false; // prevent extra clicks during reveal/reset

    private GameObject gm;
    private GameFTT game;

    public int[] arr = new int[9] { 0, 1, 1, 1, 1, 2, 2, 2, 2 };
    public int turn = 1;
    public bool botMoved = false;
    public int[] memory = new int[3] { -1, -1, -1 };

    public int PlayerWin = 0;
    public int BotWin = 0;
    public GameObject playerIndicator;
    public GameObject botIndicator;
    public bool incremented = false;
    private int winMax = 3;
    public bool complete = false;
    public GameObject turnIndicator;
    private float target;
    public float time = 2f;
    private Coroutine fadeCoroutine;

    // Standardizing to Color (0-1f) for the Lerp function
    private Color color1 = new Color(0f, 0f, 1f, 0.5f); // Blue 50% Alpha
    private Color color2 = new Color(1f, 0f, 0f, 0.5f);
    [SerializeField] private float duration = 1.0f; // How many seconds the fade should take
    private float colorPercent = 0f;
    private void Start()
    {
        gm = GameObject.Find("Game");
        game = gm.GetComponent<GameFTT>();
        AssignValue(arr);
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
        playerIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerWin.ToString();
        botIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = BotWin.ToString();
        if (turn == 1)
        {
            StartFade(false, 1f);
        }
        else
        {
            StartFade(true, 1f);
        }

        if (PlayerWin == winMax)
        {
            if (PlayerPrefs.GetString("Game Mode") == "Normal")
            {
                PlayerPrefs.SetString("Last Scene", SceneManager.GetActiveScene().name);
                StartCoroutine(LoadSceneAsyncCoroutine("OPlayerWin"));
            }
            else
            {
                AssignValue(arr);
            }
        }
        else if (BotWin == winMax)
        {
            if (PlayerPrefs.GetString("Game Mode") == "Normal")
            {
                PlayerPrefs.SetString("Last Scene", SceneManager.GetActiveScene().name);
                StartCoroutine(LoadSceneAsyncCoroutine("XPlayerWin"));
            }
            else
            {
                AssignValue(arr);
            }
        }
        if (isRevealing) return; // ignore clicks during reveal/reset

        if (Input.GetMouseButtonUp(0) && ((turn == 1 && PlayerPrefs.GetString("Opponent Type") == "Bot")|| PlayerPrefs.GetString("Opponent Type") != "Bot"))
        {
            // Ignore clicks through UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                FTTButtonClick obj = hit.collider.GetComponent<FTTButtonClick>();
                if (obj != null)
                {
                    // Deselect if already selected
                    if (selectedObjects.Contains(obj))
                    {
                        DeselectObject(obj);
                    }
                    // Select if under limit
                    else if (selectedObjects.Count < maxSelections)
                    {
                        SelectObject(obj);
                    }

                    // If exactly 3 selected → reveal + start reset coroutine
                    if (selectedObjects.Count == maxSelections)
                    {
                        StartCoroutine(RevealAndResetAfterDelay());
                    }
                }
            }
        }
    }

    public void SelectObject(FTTButtonClick obj)
    {
        obj.Highlight(highlightColor);
        selectedObjects.Add(obj);
        Debug.Log($"Selected: {obj.name}");
    }

    public void DeselectObject(FTTButtonClick obj)
    {
        obj.ResetHighlight();
        selectedObjects.Remove(obj);
        Debug.Log($"Deselected: {obj.name}");
    }

    public IEnumerator RevealAndResetAfterDelay()
    {
        isRevealing = true;

        Debug.Log("Revealing selected objects...");


        for (int obj = 0; obj < 3; obj++)
        {
            selectedObjects[obj].ResetHighlight();
            selectedObjects[obj].RevealValue();
            if (selectedObjects[obj].valueToReveal == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (memory[i] == -1 && !memory.Contains(selectableObjects.IndexOf(selectedObjects[obj])))
                    {
                        memory[i] = selectableObjects.IndexOf(selectedObjects[obj]);
                        break;
                    }
                }
            }
        }
        if (selectedObjects[0].valueToReveal == 0 || selectedObjects[1].valueToReveal == 0 || selectedObjects[2].valueToReveal == 0)
        {
            if (turn == 1 && !complete)
            { 
                PlayerPrefs.SetInt("FlipTacToe", PlayerPrefs.GetInt("FlipTacToe") + 1);
                PlayerWin++;
                complete = true;
            }
            else if (turn == 2 && !complete)
            { 
                BotWin++;
                complete = true;
            }
            AssignValue(arr);
            memory[0] = -1;
            memory[1] = -1;
            memory[2] = -1;
            complete = false;
        }

        // Wait before resetting
        yield return new WaitForSeconds(resetDelay);

        if (selectedObjects[0].valueToReveal == selectedObjects[1].valueToReveal
    && selectedObjects[1].valueToReveal == selectedObjects[2].valueToReveal)
        {
            if (selectedObjects[0].valueToReveal == 1 && !incremented)
            {
                PlayerPrefs.SetInt("FlipTacToe", PlayerPrefs.GetInt("FlipTacToe") + 1);
                PlayerWin++;
                incremented = true;

            }
            else if (selectedObjects[0].valueToReveal == 2 &&!incremented)
            {
                BotWin++;
                incremented = true;
            }
            
        }
        ResetSelection();
        if (incremented)
        {
            AssignValue(arr);
            memory[0] = -1;
            memory[1] = -1;
            memory[2] = -1;
            incremented = false;
        }
            botMoved = (turn == 1) ? false : true;
        turn = (turn == 1) ? 2 : 1;
        isRevealing = false;
    }

    public void ResetSelection()
    {
        Debug.Log("Resetting selection...");
        foreach (var obj in selectedObjects)
        {
            obj.ResetHighlight();
        }
        selectedObjects.Clear();
    }

    private void AssignValue(int[] arr)
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        int n = arr.Length;
        for (int i = n - 1; i > 0; i--)
        {
            // Pick a random index from 0 to i (inclusive)
            int j = random.Next(0, i + 1);

            // Swap array[i] with the element at the random index j
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
            Debug.Log(arr[0] + " " + arr[1] + " " + arr[2] + " " + arr[3] + " " + arr[4] + " " + arr[5] + " " + arr[6] + " " + arr[7] + " " + arr[8]);
            Debug.Log(random);
            for (int k = 0; k < arr.Length; k++)
            {
                selectableObjects[k].valueToReveal = arr[k];
            }
        }
    }

    public void ChangeFace(int pos, int player)
    {
        selectedObjects[pos].GetComponent<SpriteRenderer>().sprite = sprites[player - 1];
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