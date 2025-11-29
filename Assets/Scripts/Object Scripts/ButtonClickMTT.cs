using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickMTT : MonoBehaviour
{
    private GameObject gm;
    private GameManagerMTT gameManager;
    private GameObject gm2;
    public GameMTT game;
    public bool clicked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        gm = GameObject.Find("GameManager");
        gameManager = gm.GetComponent<GameManagerMTT>();
        //gm = GameObject.Find("Game");
        //game = gm2.GetComponent<GameMTT>();
    }

    private void OnMouseUp()
    {
        DetectClickedObject();
    }

    private void DetectClickedObject()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Click ignored (pointer over UI)");
            return; // Skip raycast
        }
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            GameObject clickedObject = hit.collider.gameObject;
            Debug.Log("Clicked: " + clickedObject.name);

            // Find its position in the GameManager array
            int index = System.Array.IndexOf(gameManager.boardSpecs, clickedObject);

            if (index >= 0 && clicked == false)
            {
                //game.timeUp = false;
                //game.timeIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = "5";
                gameManager.currentButtonClicked = index;
                Debug.Log("Object found in array at index: " + index);
                gameManager.turn = 2;
                // You can now access gameManager.objects[index]
            }
            else
            {
                Debug.Log("Object not found in array.");
            }
        }
    }

    public void ClickButton()
    {
        game.time = false;
        gameManager.count++;
        if (gameManager.turn == 1)
        {
            GetComponent<SpriteRenderer>().sprite = gameManager.sprites[0];
            GetComponent<SpriteRenderer>().color = Color.white;
            clicked = true;
            //gameManager.turn = 2;
            gameManager.UpdateArray();
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = gameManager.sprites[1];
            GetComponent<SpriteRenderer>().color = Color.white;
            clicked = true;
            //gameManager.turn = 1;
            gameManager.UpdateArray();
        }
    }
}