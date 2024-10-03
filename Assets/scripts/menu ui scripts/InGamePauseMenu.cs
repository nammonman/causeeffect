using Subtegral.DialogueSystem.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using static raycastinteract;

public class InGamePauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseWindow;
    [SerializeField] GameObject backgroundTL;
    [SerializeField] GameObject quickTL;
    [SerializeField] GameObject allTL;
    [SerializeField] Slider loadingBar;

    private float holdDuration = 1f; // Time in seconds to hold the key
    private float holdDurationLong = 2f; // Time in seconds to hold the key
    private float keyHoldTimer = 0f; // Timer to track how long the key is held
    private float checkRepeatTimer = 0f; // fix toggle multiple times
    private void Start()
    {
        GameStateManager.canPause = true;
        GameStateManager.isPaused = false;
        pauseWindow.SetActive(false);
    }

    public void setSliderValue(float val)
    {
        if (loadingBar.value - 0.01f <= val && loadingBar.value + 0.01f >= val)
        {
            loadingBar.value = val;
        }
        else if (loadingBar.value < val)
        {
            loadingBar.value = val - 0.4f;
        }
        else if (loadingBar.value > val)
        {
            loadingBar.value = val + 0.4f;
        }
        else
        {
            loadingBar.value = val;
        }
    }
    void updateTL()
    {
        if (loadingBar.value >= 0.5 && loadingBar.value < holdDurationLong - 0.3f)
        {
            checkRepeatTimer += Time.deltaTime;

            if (checkRepeatTimer >= 0.2f)
            {
                quickTL.SetActive(true);
                allTL.SetActive(false);
                checkRepeatTimer = 0f;
            }

        }
        else if (loadingBar.value >= holdDurationLong - 0.3f )
        {
            checkRepeatTimer += Time.deltaTime;

            if (checkRepeatTimer >= 0.2f)
            {
                quickTL.SetActive(false);
                allTL.SetActive(true);
                checkRepeatTimer = 0f;
            }
        }

        if (loadingBar.value <= 0.001f)
        {
            checkRepeatTimer += Time.deltaTime;

            if (checkRepeatTimer >= 0.5f)
            {
                quickTL.SetActive(false);
                allTL.SetActive(false);
                backgroundTL.SetActive(false);
                checkRepeatTimer = 0f;
            }
        }
        else
        {
            backgroundTL.SetActive(true);

        }
    }

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


            updateTL();


            if (Input.GetKey(KeyCode.Tab))
            {
                
                if (loadingBar.value > 1.5f)
                {
                    SliderEasing.easeEnable = true;
                }
                else
                {
                    SliderEasing.easeEnable = false;
                    keyHoldTimer += Mathf.Clamp(Time.deltaTime, 0, 2);
                    loadingBar.value = keyHoldTimer;
                }
            }
            else
            {
                SliderEasing.easeEnable = true;
                keyHoldTimer = 0;
            }

            if (Input.GetKeyDown(KeyCode.Tab) && loadingBar.value >= 0.9f)
            {
                loadingBar.value = 0;
            }
        }
    }
}
