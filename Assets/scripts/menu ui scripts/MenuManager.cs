using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    private List<GameObject> menuWindows;
    private List<string> menuWindowsNames;


    // Start is called before the first frame update
    void Start()
    {
        // Get all GameObjects with the "menu window" tag
        GameObject[] windows = GameObject.FindGameObjectsWithTag("menu window");
        menuWindows = new List<GameObject>(windows);
        menuWindowsNames = new List<string>();
        foreach (GameObject w in windows) 
        {
            menuWindowsNames.Add(w.name);
            //Debug.Log(w.name);
        }
        ShowMenu("MainMenu");
        
    }

    // Function to activate a GameObject with a matching name and deactivate others
    public void ShowMenu(string windowName)
    {
        if (menuWindowsNames.Contains(windowName))
        {
            foreach (GameObject window in menuWindows)
            {
                if (window.name == windowName)
                {
                    window.SetActive(true);
                }
                else
                {
                    window.SetActive(false);
                }
            
            }
        }
        else
        {
            Debug.Log("can't find menu: " + windowName);
        }
    }

    public void LoadSceneFromMenu()
    {
        GameObject.FindGameObjectWithTag("player prefab").GetComponent<LoadScene>().LoadSceneAndTeleport("TestScene");
    }
}
