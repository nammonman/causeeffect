using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretItem : MonoBehaviour
{
    private void OnEnable()
    {
        if (gameObject.name == "FoundItem1")
        {
            if (GameStateManager.gameStates.currentDay > 1 || !GameStateManager.gameStates.globalFlags.Contains("Item1Location"))
            {
                Destroy(gameObject);
            }
        }
        else if (gameObject.name == "FoundItem2")
        {
            if (GameStateManager.gameStates.currentDay > 3 || !GameStateManager.gameStates.globalFlags.Contains("Item2Location"))
            {
                Destroy(gameObject);
            }
        }
        else if (gameObject.name == "FoundItem3")
        {
            if (GameStateManager.gameStates.currentDay > 5 || !GameStateManager.gameStates.globalFlags.Contains("Item3Location"))
            {
                Destroy(gameObject);
            }
        }
        
        
        
    }
}
