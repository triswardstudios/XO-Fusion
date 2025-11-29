using UnityEngine;

public class GameManagerMTT : MonoBehaviour
{
    public GameObject[] boardSpecs = new GameObject[9];
    public int turn = 1;
    public Sprite[] sprites = new Sprite[2];
    public Sprite defaultSprite;
    public int count = 0;
    public int[] arr = new int[9];
    public int currentButtonClicked = -1;
    public GameObject tieBreaker;

    private void Start()
    {
        UpdateArray();
    }

    public void UpdateArray()
    {
        for (int i = 0; i < 9; i++)
        {
            if (boardSpecs[i].GetComponent<SpriteRenderer>().sprite == sprites[0])
                arr[i] = 1;
            else if (boardSpecs[i].GetComponent<SpriteRenderer>().sprite == sprites[1])
                arr[i] = 2;
            else
                arr[i] = 0;
        }
    }

    public void ChangeFace(int pos, int player)
    {
        boardSpecs[pos].GetComponent<SpriteRenderer>().sprite = sprites[player - 1];
    }
}