using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string scene;
    public string setPrefKey;
    public string setPrefValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadNewScene(string scene)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(scene));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        if (setPrefKey != null && setPrefValue != null)
        {
            PlayerPrefs.SetString(setPrefKey, setPrefValue);
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        yield return null;
    }

}