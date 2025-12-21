using UnityEngine;
using System.Linq;
using System.Collections;

public class GameFTT : MonoBehaviour
{
    private GameObject gm;
    private GameManagerFTT game;
    public int[] botSelection = new int[3];

    private void Start()
    {
        gm = GameObject.Find("GameManager");
        game = gm.GetComponent<GameManagerFTT>();
    }

    private void Update()
    {
        if (game.turn == 2 && !game.botMoved && PlayerPrefs.GetString("Opponent Type") == "Bot")
        {
            StartCoroutine(BotMove());
        }
    }

    private IEnumerator BotMove()
    {
        game.botMoved = true;
        yield return new WaitForSeconds(1f);
        int[] selected = new int[3] { -1, -1, -1 };
        game.memory.CopyTo(selected, 0);
        for (int i = 0; i < 3; i++)
        {
            if (selected[i] == -1)
            {
                int temp = Random.Range(0, 9);
                while (selected.Contains(temp))
                {
                    temp = Random.Range(0, 9);
                }

                selected[i] = temp;
            }
        }
        botSelection = selected;
        for (int i = 0; i < 3; i++)
        {
            if (game.selectableObjects[selected[i]].valueToReveal == 2)
            {
                game.memory[i] = selected[i];
            }
            game.SelectObject(game.selectableObjects[selected[i]]);
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(game.RevealAndResetAfterDelay());
    }
}