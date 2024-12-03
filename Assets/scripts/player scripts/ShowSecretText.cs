using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ShowSecretText : MonoBehaviour
{
    public static List<GameObject> currentSceneTexts = new List<GameObject>();
    private void OnEnable()
    {
        GameStateManager.OnSecretText += DisplaySecretText;
    }

    private void OnDisable()
    {
        GameStateManager.OnSecretText -= DisplaySecretText;
    }

    public void DisplaySecretText(bool b)
    {
        GameStateManager.gameStates.canSeeSecretText = b;
        
        foreach (var item in currentSceneTexts) { item.SetActive(b); }
    }
}
