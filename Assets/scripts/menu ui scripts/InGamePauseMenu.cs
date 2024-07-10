using Subtegral.DialogueSystem.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static raycastinteract;

public class InGamePauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseWindow;

    private void Start()
    {
        GameStateManager.canPause = true;
        GameStateManager.isPaused = false;
        pauseWindow.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameStateManager.isPaused)
                {
                    //unpause
                    GameStateManager.isPaused = false;
                    GameStateManager.canPlayerMove = true;
                    GameStateManager.canPlayerJump = true;
                    GameStateManager.canPlayerMoveCamera = true;
                    GameStateManager.canPlayerInteract = true;
                    GameStateManager.canLoadNewScene = true;
                    pauseWindow.SetActive(false);
                }
                else
                {
                    //pause
                    GameStateManager.isPaused = true;
                    GameStateManager.canPlayerMove = false;
                    GameStateManager.canPlayerJump = false;
                    GameStateManager.canPlayerMoveCamera = false;
                    GameStateManager.canPlayerInteract = false;
                    GameStateManager.canLoadNewScene = false;
                    pauseWindow.SetActive(true);
                }
                
            }
        }
    }
}
