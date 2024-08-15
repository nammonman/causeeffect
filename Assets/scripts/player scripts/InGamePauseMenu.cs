using Subtegral.DialogueSystem.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static raycastinteract;

public class InGamePauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseWindow;
    bool canPause;
    bool isPaused;
    public delegate void PlayerMoveable(bool b);
    public static event PlayerMoveable SetPlayerMoveable;

    private void Start()
    {
        canPause = true;
        isPaused = false;
        pauseWindow.SetActive(false);
    }
    private void EnablePausing()
    {
        StartCoroutine(PauseDelay(0.1f));
    }

    private void DisablePausing(string temp1, bool temp2) 
    {
        canPause = false;
        Debug.Log("Disabled");
    }

    IEnumerator PauseDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPause = true;
        Debug.Log("Enabled");
    }

    private void OnEnable()
    {
        raycastinteract.OnDialogueEnter += DisablePausing;
        DialogueParser.OnDialogueExit += EnablePausing;
    }

    private void OnDisable()
    {
        raycastinteract.OnDialogueEnter -= DisablePausing;
        DialogueParser.OnDialogueExit -= EnablePausing;
    }

    // Update is called once per frame
    void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    //unpause
                    isPaused = false;
                    SetPlayerMoveable?.Invoke(true);
                    pauseWindow.SetActive(false);
                }
                else
                {
                    //pause
                    isPaused = true;
                    SetPlayerMoveable?.Invoke(false);
                    pauseWindow.SetActive(true);
                }
                
            }
        }
    }
}
