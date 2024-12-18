using Subtegral.DialogueSystem.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextUpdater : MonoBehaviour
{
    //[SerializeField] public GameObject UI;
    [SerializeField] public TextMeshProUGUI timeText;
    [SerializeField] public TextMeshProUGUI sceneText;

    private void OnEnable()
    {
        GameStateManager.OnSceneUpdate += UpdateSceneText;
        GameStateManager.OnTimeUpdate += UpdateTimeText;
        GameStateManager.OnUIEnable += EnableUIText;
    }

    private void OnDisable()
    {
        GameStateManager.OnSceneUpdate -= UpdateSceneText;
        GameStateManager.OnTimeUpdate -= UpdateTimeText;
        GameStateManager.OnUIEnable -= EnableUIText;
    }
    public void UpdateTimeText()
    {
        timeText.text = "Day " + GameStateManager.gameStates.currentDay.ToString() + " - " + GameStateManager.gameStates.currentTimeOfDay.ToString();
    }

    public void UpdateSceneText(string text) 
    {
        sceneText.text = text; //???
    }

    public void EnableUIText(bool b)
    {
        gameObject.SetActive(b);
    }

}
