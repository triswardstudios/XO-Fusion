using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string scene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadNewScene(string scene)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(scene));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        // While the scene is still loading, display progress
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading progress: " + asyncLoad.progress * 100 + "%");
            yield return null; // Wait for the next frame
        }

        Debug.Log("Scene loaded: " + sceneName);
    }
}