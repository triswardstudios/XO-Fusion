using UnityEngine;

public class PlayerPrefsInit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetString("Game Mode", "Normal");
        PlayerPrefs.SetString("Opponent Type", "Versus");
        PlayerPrefs.Save();
    }

}
