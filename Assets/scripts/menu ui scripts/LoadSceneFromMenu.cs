using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneFromMenu : MonoBehaviour
{
    // Load a scene by its name
    public void LoadSceneByName(string sceneName)
    {
        if (GameStateManager.gameStates.canLoadNewScene)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            SceneManager.LoadSceneAsync(sceneName);
            StartCoroutine(DelayLoad(2.5f));
        }

        IEnumerator DelayLoad(float delay)
        {
            GameStateManager.gameStates.canLoadNewScene = false;
            yield return new WaitForSeconds(delay);
            GameStateManager.gameStates.canLoadNewScene = true;
        }
    }

}