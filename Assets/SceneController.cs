using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        Debug.Log("NextLevel() called");

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Ensure the next scene index is within bounds
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            string nextSceneName = SceneManager.GetSceneByBuildIndex(nextSceneIndex).name;
            Debug.Log("Loading scene: " + nextSceneName); // Debug log for scene name

            StartCoroutine(LoadSceneAsync(nextSceneName));
        }
        else
        {
            Debug.LogWarning("No next level! You've reached the last scene.");
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        // Wait until the scene has finished loading
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Explicitly activate the scene after it's loaded
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        Debug.Log("Scene loaded and activated.");
    }
}
