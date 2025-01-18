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
    [SerializeField] GameObject notebook;
    [SerializeField] Slider loadingBar;

    private float holdDuration = 1f; // Time in seconds to hold the key
    private float holdDurationLong = 2f; // Time in seconds to hold the key

    private float keyHoldTimer = 0f; // Timer to track how long the key is held
    private float checkRepeatTimer = 0f; // fix toggle multiple times

    private int co = 0;
    
    private bool canUnPause = false;
    public static GameObject selectedGameObject = null;

    public static bool inPause = false;
    public static bool inTL = false;


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

    public void activateQuickTL()
    {
        if (!quickTL.activeSelf)
        {

            GameStateManager.setPausedState(true);
            if (GameStateManager.gameStates.fixLevel >= 2)
            {
                quickTL.SetActive(true);
            }
            allTL.SetActive(false);
            notebook.SetActive(false);
            GameObject.Find("999999").GetComponent<TimelineEventDisplay>().selectTimeline();

        }

        

    }

    public void activateAllTL()
    {
        if (!allTL.activeSelf)
        {

            GameStateManager.setPausedState(true);
            quickTL.SetActive(false);
            if (GameStateManager.gameStates.fixLevel >= 2)
            {
                allTL.SetActive(true);
            }
            notebook.SetActive(false);
            GameObject.Find("999999").GetComponent<TimelineEventDisplay>().selectTimeline();
        }
        
    }

    public void activateNotebook()
    {
        if (!notebook.activeSelf)
        {

            GameStateManager.setPausedState(true);
            quickTL.SetActive(false);
            allTL.SetActive(false);
            if (GameStateManager.gameStates.globalFlags.Contains("Notebook"))
            {
                notebook.SetActive(true);
            }
            
        }

    }

    public void deactivateTL()
    {
        if (backgroundTL.activeSelf)
        {
            GameStateManager.setPausedState(false);
            quickTL.SetActive(false);
            allTL.SetActive(false);
            notebook.SetActive(false);
            backgroundTL.SetActive(false);
        }
        
    }

    void updateTL()
    {
        if (loadingBar.value >= 0.5 && loadingBar.value < 2 - 0.3f)
        {
            checkRepeatTimer += Time.deltaTime;

            if (checkRepeatTimer >= 0.4f)
            {
                activateQuickTL();
                checkRepeatTimer = 0f;
            }

        }
        else if (loadingBar.value >= 2 - 0.3f && loadingBar.value < 3 - 0.3f)
        {
            checkRepeatTimer += Time.deltaTime;

            if (checkRepeatTimer >= 0.4f)
            {
                activateAllTL();
                checkRepeatTimer = 0f;
            }
        }
        else if (loadingBar.value >= 3 - 0.3f)
        {
            checkRepeatTimer += Time.deltaTime;

            if (checkRepeatTimer >= 0.4f)
            {
                activateNotebook();
                checkRepeatTimer = 0f;
            }
        }

        if (loadingBar.value <= 0.001f)
        {
            checkRepeatTimer += Time.deltaTime;

            if (checkRepeatTimer >= 0.5f && backgroundTL.activeSelf)
            {
                deactivateTL();
                checkRepeatTimer = 0f;
                inTL = false;
            }
        }
        else if (!backgroundTL.activeSelf)
        {
            inTL = true;
            backgroundTL.SetActive(true);
        }
    }

    IEnumerator EnableUnPauseAfterDelay(float delay)
    {
        float elapsedTime = 0f;

        while (elapsedTime < delay)
        {
            GameStateManager.setPausedState(true);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canUnPause = true;
    }
    void Update()
    {
        if (GameStateManager.gameStates.canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!inTL)
                {
                    if (pauseWindow.activeSelf && canUnPause)
                    {
                        //unpause
                        GameStateManager.setPausedState(false);
                        pauseWindow.SetActive(false);
                        canUnPause = false;
                        inPause = false;
                    }
                    else
                    {
                        //pause
                        inPause = true;
                        GameStateManager.setPausedState(true);
                        pauseWindow.SetActive(true);
                        StartCoroutine(EnableUnPauseAfterDelay(0.5f));
                    }
                }
                else
                {
                    loadingBar.value = 0;
                }

            }


            updateTL();

            if (!inPause)
            {
                if (Input.GetKey(KeyCode.Tab) && GameStateManager.gameStates.fixLevel >= 2)
                {
                    keyHoldTimer += Mathf.Clamp(Time.deltaTime, 0, 2);

                    if (loadingBar.value > 0.6f)
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

                if (Input.GetKeyDown(KeyCode.Return) && selectedGameObject )
                {
                    if (allTL.activeSelf && !quickTL.activeSelf && GameStateManager.gameStates.fixLevel >=4)
                    {
                        int id = int.Parse(selectedGameObject.name);
                        Debug.Log("selected id: " + id);
                        if (id == 999999) // forbidden id
                        {
                            loadingBar.value = 0;
                            return;
                        }

                        MakeTL.LoadTLPS(id);
                        loadingBar.value = 0;
                        selectedGameObject = null;
                    }
                    else if (quickTL.activeSelf && !allTL.activeSelf && GameStateManager.gameStates.fixLevel >= 2)
                    {
                        int id = int.Parse(selectedGameObject.name);
                        Debug.Log("selected id: " + id);
                        if (id == 999999) // forbidden id
                        {
                            loadingBar.value = 0;
                            return;
                        }

                        MakeTL.LoadTLPS(id);
                        loadingBar.value = 0;
                        selectedGameObject = null;
                    }
                    
                }
            }
            else
            {
                
            }

        }
    }
}
