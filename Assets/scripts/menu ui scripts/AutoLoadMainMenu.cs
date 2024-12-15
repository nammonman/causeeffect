using Subtegral.SceneGraphSystem.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoLoadMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.setLoadNewScene("MainMenu");
    }

    
}
