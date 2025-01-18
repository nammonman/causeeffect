using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookItem : MonoBehaviour
{
    private void OnEnable()
    {
        if (GameStateManager.gameStates.globalFlags.Contains("Notebook"))
        {
            gameObject.tag = "Untagged";
        }
    }
}
