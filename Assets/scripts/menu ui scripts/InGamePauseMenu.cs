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

    private int co = 0;
    
    private bool canUnPause = false;

    private void Start()
    {
        GameStateManager.setPausedState(false);
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
                GameStateManager.setPausedState(true);
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
                GameStateManager.setPausedState(true);
                quickTL.SetActive(false);
                allTL.SetActive(true);
                checkRepeatTimer = 0f;
            }
        }

        if (loadingBar.value <= 0.001f)
        {
            checkRepeatTimer += Time.deltaTime;

            if (checkRepeatTimer >= 0.5f && backgroundTL.activeSelf)
            {
                GameStateManager.setPausedState(false);
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

    IEnumerator EnableUnPauseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canUnPause = true;
    }
    void Update()
    {
        if (GameStateManager.gameStates.canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !GameStateManager.gameStates.isInDialogue)
            {
                if (pauseWindow.activeSelf && canUnPause)
                {
                    //unpause
                    GameStateManager.setPausedState(false);
                    pauseWindow.SetActive(false);
                    canUnPause = false;
                }
                else
                {
                    //pause
                    GameStateManager.setPausedState(true);
                    pauseWindow.SetActive(true);
                    StartCoroutine(EnableUnPauseAfterDelay(0.5f));
                }
                //co++;
                //Debug.Log(co);
            }


            updateTL();


            if (Input.GetKey(KeyCode.Tab))
            {
                keyHoldTimer += Mathf.Clamp(Time.deltaTime, 0, 2);

                if (loadingBar.value > 1.6f)
                {
                    SliderEasing.easeEnable = true;
                }
                else
                {
                    SliderEasing.easeEnable = false;
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
