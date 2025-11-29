using UnityEngine;

public class ExitAppScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            Application.Quit();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}