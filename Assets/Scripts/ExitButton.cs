using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string scene;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            LoadNewScene(scene);
        }
    }

    public void LoadNewScene(string scene)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(scene));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        yield return null;
    }
}