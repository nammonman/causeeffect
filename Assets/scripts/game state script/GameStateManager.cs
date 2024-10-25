using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStates
{
    //current save file
    public string saveFileName;
    public bool isSaving;
    public int saveId;

    //timeline event
    public int currentEventId;
    public int currentDay;
    public int currentTimeOfDay;
    public int activeEventType;
    public bool isInActiveEvent;

    //pause menu
    public bool canPause;
    public bool isPaused;
    public bool isSpecialMenu;

    //player movement
    public bool canPlayerMove;
    public bool canPlayerJump;
    public bool canPlayerMoveCamera;

    //player interaction with object
    public bool canPlayerInteract;

    //dialogue
    public bool isInDialogue;

    //loaded scene
    public bool canLoadNewScene;
    public string CurrentSceneName;
    public string CurrentSceneSetting;
}

public class GameStateManager : MonoBehaviour
{
    public static GameStates gameStates;
    [SerializeField] TextMeshProUGUI DEBUGTEXT;
    private void FixedUpdate()
    {
        //DEBUGTEXT.text = JsonUtility.ToJson(Camera.main.transform.rotation, true);
    }
    private void Awake()
    {
        gameStates = new GameStates();
        
        //init variables
        gameStates.saveFileName = "slot1";
        gameStates.canLoadNewScene = true;   
    }


    // Start is called before the first frame update
    void Start()
    {
        //gameStates = new GameStates();
        Debug.Log("BEFORE JP TODO: \n1. load scene and implement with TL\n2. link main menu with actual game\n3. save file implementation");
         
    }


    public static void setPausedState(bool b)
    {
        //Debug.Log(b);
        gameStates.isPaused = b;
        gameStates.canPlayerMove = !b;
        gameStates.canPlayerJump = !b;
        gameStates.canPlayerMoveCamera = !b;
        gameStates.canPlayerInteract = !b;
        gameStates.canLoadNewScene = !b;
    }

}
