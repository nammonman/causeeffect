using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingCheck : MonoBehaviour
{
    private void OnEnable()
    {
        if (gameObject.scene.name.StartsWith("WorkLab") && GameStateManager.gameStates.globalFlags.Contains("approveLeave"))
        {
            GameStateManager.setLoadSceneSetting(gameObject.scene.name + " b leave");
            StartCoroutine(UnloadCurrentScene());
        }
    }

    private IEnumerator UnloadCurrentScene()
    {
        yield return null;
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
