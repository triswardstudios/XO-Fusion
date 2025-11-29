using UnityEngine;
using UnityEngine.UI;

public class FTTButtonClick : MonoBehaviour
{
    private GameObject gm;
    [Header("Reveal Info")]
    public int valueToReveal;
    public GameObject floatingTextPrefab;
    public GameManagerFTT game;

    private Sprite originalSprite;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        gm = GameObject.Find("GameManager");
        game = gm.GetComponent<GameManagerFTT>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            originalSprite = spriteRenderer.sprite;
        }
            
    }

    public void Highlight(Color color)
    {
        if (spriteRenderer != null)
            spriteRenderer.color = color;
            

    }

    public void ResetHighlight()
    {
        if (spriteRenderer != null)
        {  
            spriteRenderer.color = originalColor;
            spriteRenderer.sprite = originalSprite;
        }
    }

    public void RevealValue()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
            spriteRenderer.sprite = game.sprites[valueToReveal];
        }
        else
            Debug.Log("Confusionnnnnnnn!");
    }
}
