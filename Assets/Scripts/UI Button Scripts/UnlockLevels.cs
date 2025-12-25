using UnityEngine;
using UnityEngine.UI;

public class UnlockLevels : MonoBehaviour
{
    public string levelToUnlock;
    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt(levelToUnlock) > 5)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
