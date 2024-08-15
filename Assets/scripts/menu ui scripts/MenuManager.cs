using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuWindow;
    [SerializeField] GameObject sceneSelectWindow;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuWindow.SetActive(true);
        sceneSelectWindow.SetActive(false);
    }

    public void switchToSceneSelect()
    {
        mainMenuWindow.SetActive(false);
        sceneSelectWindow.SetActive(true);
    }

    public void switchToMainMenu()
    {
        mainMenuWindow.SetActive(true);
        sceneSelectWindow.SetActive(false);
    }
}
