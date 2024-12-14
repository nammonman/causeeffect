using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sleep : MonoBehaviour
{
    [SerializeField] GameObject secondFloorCameraClose;
    [SerializeField] GameObject secondFloorCameraFar;
    [SerializeField] GameObject firstFloorCamera;

    private void OnEnable()
    {
        secondFloorCameraClose.SetActive(false);
        secondFloorCameraFar.SetActive(true);
        firstFloorCamera.SetActive(false);
    }

    public void GoToSleep()
    {
        GameStateManager.setDream(1);
        firstFloorCamera.SetActive(true);
        secondFloorCameraClose.SetActive(false);
        secondFloorCameraFar.SetActive(false);
        GameStateManager.setDateTime(GameStateManager.gameStates.currentDay + 1, 0);
    }
}
