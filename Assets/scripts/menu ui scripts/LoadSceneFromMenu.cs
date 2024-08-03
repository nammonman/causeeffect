using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneFromMenu : MonoBehaviour
{
    // Load a scene by its name
    public void LoadSceneByName(string sceneName)
    {
        if (GameStateManager.canLoadNewScene)
        {
            SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(sceneName);
            StartCoroutine(DelayLoad(2.5f));
        }

        IEnumerator DelayLoad(float delay)
        {
            GameStateManager.canLoadNewScene = false;
            yield return new WaitForSeconds(delay);
            GameStateManager.canLoadNewScene = true;
        }
    }

}