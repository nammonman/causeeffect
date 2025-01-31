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
    [SerializeField] TextMeshProUGUI inventoryText;
    private void OnEnable()
    {
        GameStateManager.OnSceneUpdate += UpdateSceneText;
        GameStateManager.OnTimeUpdate += UpdateTimeText;
        GameStateManager.OnFlagsUpdate += UpdateInventory;
        GameStateManager.OnUIEnable += EnableUIText;
    }

    private void OnDisable()
    {
        GameStateManager.OnSceneUpdate -= UpdateSceneText;
        GameStateManager.OnTimeUpdate -= UpdateTimeText;
        GameStateManager.OnFlagsUpdate -= UpdateInventory;
        GameStateManager.OnUIEnable -= EnableUIText;
    }
    public void UpdateTimeText()
    {
        string[] t = { "morning", "afternoon", "evening", "night" };
        timeText.text = "Day " + GameStateManager.gameStates.currentDay.ToString() + " - " + t[GameStateManager.gameStates.currentTimeOfDay];
    }

    public void UpdateSceneText(string text) 
    {
        sceneText.text = text; //???
    }

    public void EnableUIText(bool b)
    {
        gameObject.SetActive(b);
    }

    public void UpdateInventory()
    {
        inventoryText.text = "";
        string t = "";
        bool tbFlag = false;

        if (GameStateManager.gameStates.globalFlags.Contains("MIX_EnhancedVision"))
        {
            t += "Enhanced Vision\n";
        }
        if (GameStateManager.gameStates.globalFlags.Contains("MIX_AlienInvasion"))
        {
            t += "Alien Invasion\n";
        }
        if (GameStateManager.gameStates.globalFlags.Contains("MIX_TimeBomb"))
        {
            t += "Time Bomb\n";
            tbFlag = true;
        }
        if (!tbFlag && GameStateManager.gameStates.globalFlags.Contains("MIX_StabilizerI"))
        {
            t += "Stabilizer I\n";
        }
        if (!tbFlag && GameStateManager.gameStates.globalFlags.Contains("MIX_StabilizerII"))
        {
            t += "Stabilizer II\n";
        }
        if (GameStateManager.gameStates.globalFlags.Contains("HACKED DOCUMENT"))
        {
            t += "Secret Documents\n";
        }
        
        inventoryText.text = t;
        Debug.Log("inventory text updated to " + t);
    }
    
}
