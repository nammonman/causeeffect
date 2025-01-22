using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TrueEndingSequence : MonoBehaviour
{
    [SerializeField] GameObject fixedCamera;
    [SerializeField] GameObject endScreen; 
    [SerializeField] TextMeshProUGUI endingText;

    public static TrueEndingSequence Instance;
    private void OnEnable()
    {
        endScreen.SetActive(false);
        StartCoroutine(WaitSwitchCamera());
    }

    IEnumerator WaitSwitchCamera()
    {
        yield return new WaitForSeconds(10);
        fixedCamera.SetActive(false);
        SceneObjectFinder.FindObjectInScene("DontDestroyOnLoad", "Main Camera").SetActive(true);
    }

    public static void GoodEndScreen()
    {
        Instance.fixedCamera.SetActive(true);
        Instance.endingText.text = "YOU NO LONGER EXIST";
        Instance.endScreen.SetActive(true);
    }

    public static void TrueEndScreen()
    {
        Instance.fixedCamera.SetActive(true);
        Instance.endingText.text = "TRUE ENDING";
        Instance.endScreen.SetActive(true);
    }

    public void RestartGame()
    {
        TriggerRunner.RunFuncsCaller(new List<string> { "LoadTL_2" });
    }

    public void MainMenu()
    {
        GameStateManager.setLoadNewScene("MainMenu");
        GameStateManager.setSave();
    }
}
