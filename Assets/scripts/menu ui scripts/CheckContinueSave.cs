using SaveGame;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CheckContinueSave : MonoBehaviour
{
    [SerializeField] GameObject contButton;
    void FixedUpdate()
    {
        if (File.Exists(Application.persistentDataPath + $"/{GameStateManager.gameStates.saveFileName}.json"))
        {
            contButton.SetActive(true);
        }
        else
        {
            contButton.SetActive(false);
        }
    }

}
