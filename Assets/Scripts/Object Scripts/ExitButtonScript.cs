using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButtonScript : MonoBehaviour
{
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
    IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        yield return null;
    }
}
