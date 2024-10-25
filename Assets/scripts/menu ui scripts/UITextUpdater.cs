using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextUpdater : MonoBehaviour
{
    //[SerializeField] public GameObject UI;
    [SerializeField] public TextMeshProUGUI timeText;
    [SerializeField] public TextMeshProUGUI sceneText;

    public void updateTimeText()
    {
        timeText.text = "Day " + GameStateManager.gameStates.currentDay.ToString() + " - " + GameStateManager.gameStates.currentTimeOfDay.ToString();
    }

    public void updateSceneText() 
    {
        sceneText.text = GameStateManager.gameStates.CurrentSceneName;
    }

    public void enableUIText(bool b)
    {
        gameObject.SetActive(b);
    }
}
