using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string scene = "";

    //public string sceneToLoad; // Name of the scene to load
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this new duplicate
            Destroy(gameObject);
        }
    }

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