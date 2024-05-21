using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneFromMenu : MonoBehaviour
{
    // Load a scene by its name
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(sceneName);
    }

}