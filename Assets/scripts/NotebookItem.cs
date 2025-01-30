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
        if (GameStateManager.gameStates.globalFlags.Contains("MIX_EnhancedVision"))
        {
            StartCoroutine(SecretTextUnlockSequence());
        }
    }

    public IEnumerator SecretTextUnlockSequence()
    {
        yield return null;
        GameStateManager.setSecretText(true);
        TriggerRunner.RunFuncsCaller(new List<string> { "Monologue_you can now press [T] to toggle something..." });
        yield return new WaitForSeconds(3);
        GameStateManager.setSecretText(false);
    }
}
