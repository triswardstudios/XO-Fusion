using UnityEngine;

public class UIScript : MonoBehaviour
{
    public GameObject PauseScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(PauseScreen.activeSelf) 
                PauseScreen.SetActive(false);
            else
                PauseScreen.SetActive(true);
        }
    }

    public void ButtonPause()
    {
        if (PauseScreen.activeSelf)
            PauseScreen.SetActive(false);
        else
            PauseScreen.SetActive(true);
    }
}
