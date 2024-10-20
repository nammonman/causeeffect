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
            GameStateManager.gameStates.isInDialogue = true;
            GameStateManager.setPausedState(true);
            quickTL.SetActive(true);
            allTL.SetActive(false);
            notebook.SetActive(false);
            GameObject.Find(GameStateManager.gameStates.currentEventId.ToString()).GetComponent<TimelineEventDisplay>().selectTimeline();

        }

        

    }

    public void activateAllTL()
    {
        if (!allTL.activeSelf)
        {
            GameStateManager.gameStates.isInDialogue = true;
            GameStateManager.setPausedState(true);
            quickTL.SetActive(false);
            allTL.SetActive(true);
            notebook.SetActive(false);
        }
        
    }

    public void activateNotebook()
    {
        if (!notebook.activeSelf)
        {
            GameStateManager.gameStates.isInDialogue = true;
            GameStateManager.setPausedState(true);
            quickTL.SetActive(false);
            allTL.SetActive(false);
            notebook.SetActive(true);
        }

    }

    public void deactivateTL()
    {
        if (backgroundTL.activeSelf)
        {
            GameStateManager.gameStates.isInDialogue = false;
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

            if (checkRepeatTimer >= 0.3f)
            {
                activateQuickTL();
                checkRepeatTimer = 0f;
            }

        }
        else if (loadingBar.value >= 2 - 0.3f && loadingBar.value < 3 - 0.3f)
        {
            checkRepeatTimer += Time.deltaTime;

            if (checkRepeatTimer >= 0.3f)
            {
                activateAllTL();
                checkRepeatTimer = 0f;
            }
        }
        else if (loadingBar.value >= 3 - 0.3f)
        {
            checkRepeatTimer += Time.deltaTime;

            if (checkRepeatTimer >= 0.3f)
            {
                activateNotebook();
                checkRepeatTimer = 0f;
            }
        }

        if (loadingBar.value <= 0.001f)
        {
            checkRepeatTimer += Time.deltaTime;

            if (checkRepeatTimer >= 0.4f && backgroundTL.activeSelf)
            {
                deactivateTL();
                checkRepeatTimer = 0f;
            }
        }
        else
        {
            GameStateManager.gameStates.isInDialogue = true;
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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!GameStateManager.gameStates.isInDialogue)
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
                }
                else 
                {
                    deactivateTL();
                }
                //co++;
                //Debug.Log(co);
            }


            updateTL();


            if (Input.GetKey(KeyCode.Tab))
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

            if (Input.GetKeyDown(KeyCode.Return) && (allTL.activeSelf || quickTL.activeSelf))
            {
                int id = int.Parse(selectedGameObject.name);
                TimelineEvent currentTimelineEvent = GameObject.Find("Persistent Scripts").GetComponent<TimelineEvent>();
                currentTimelineEvent.id = MakeTL.TL[id].id;
                currentTimelineEvent.type = MakeTL.TL[id].type;
                currentTimelineEvent.title = MakeTL.TL[id].title;
                currentTimelineEvent.day = MakeTL.TL[id].day;
                currentTimelineEvent.timeOfDay = MakeTL.TL[id].timeOfDay;
                currentTimelineEvent.screenshotPath = MakeTL.TL[id].screenshotPath;
                currentTimelineEvent.saveDataId = MakeTL.TL[id].saveDataId;
                currentTimelineEvent.isEventStarted = MakeTL.TL[id].isEventStarted;
                currentTimelineEvent.isEventFinished = MakeTL.TL[id].isEventFinished;
                currentTimelineEvent.state = MakeTL.TL[id].state;
                currentTimelineEvent.nextEventIds = MakeTL.TL[id].nextEventIds;
                currentTimelineEvent.lastEventId = MakeTL.TL[id].lastEventId;
                GameStateManager.gameStates.currentEventId = id;
                MakeTL.loadPSFromCurrentTL();
                loadingBar.value = 0;
                deactivateTL();
            }
        }
    }
}
