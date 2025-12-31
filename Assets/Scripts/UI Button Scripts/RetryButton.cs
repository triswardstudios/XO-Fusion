using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string scene;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            LoadNewScene(scene);
        }
    }

    public void LoadNewScene(string scene)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(scene));
    }
    public void AutoLoadNewScene()
    {
        StartCoroutine(LoadSceneAsyncCoroutine(PlayerPrefs.GetString("Last Scene")));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        yield return null;
    }
}