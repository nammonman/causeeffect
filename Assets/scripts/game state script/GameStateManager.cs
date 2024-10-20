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

        gameStates.canPause = true;
        gameStates.isPaused = false;

        gameStates.canPlayerMove = true;
        gameStates.canPlayerJump = true;
        gameStates.canPlayerMoveCamera = true;

        gameStates.canPlayerInteract = true;

        gameStates.isInDialogue = false;

        gameStates.canLoadNewScene = true;   
    }


    // Start is called before the first frame update
    void Start()
    {
        //gameStates = new GameStates();

         
    }

    public void setCanPause(bool b) { gameStates.canPause = b;}
    
    public void setIsPaused(bool b) { gameStates.isPaused = b; }

    public void setCanPlayerMove(bool b) { gameStates.canPlayerMove = b; }

    public void setCanPlayerJump(bool b) { gameStates.canPlayerJump = b; }

    public void setCanPlayerMoveCamera(bool b) { gameStates.canPlayerMoveCamera = b; }

    public void setCanPlayerInteract(bool b) { gameStates.canPlayerInteract = b; }

    public void setIsInDialogue(bool b) { gameStates.isInDialogue = b; }

    public void setCanLoadNewScene(bool b) { gameStates.canLoadNewScene = b; }

    public static void incrementTimelineId() { gameStates.currentEventId++; }

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

    public static void setJumpState(bool b)
    {
        //Debug.Log(b? "can jump" : "can'T jump");
        gameStates.canPlayerJump = b;
    }

    public static void setSceneLoadState(bool b)
    {
        gameStates.canLoadNewScene = b;
    }
}
