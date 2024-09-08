using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    //current save file
    public static string saveFileName;
    public static bool isSaving;

    //timeline event
    public static int currentEventId;
    public static int currentDay;
    public static int currentTimeOfDay;
    public static int activeEventType;
    public static bool isInActiveEvent;

    //pause menu
    public static bool canPause;
    public static bool isPaused;

    //player movement
    public static bool canPlayerMove;
    public static bool canPlayerJump;
    public static bool canPlayerMoveCamera;

    //player interaction with object
    public static bool canPlayerInteract;

    //dialogue
    public static bool isInDialogue;

    //loaded scene
    public static bool canLoadNewScene;
    public static string CurrentSceneName;
    public static string CurrentSceneSetting;


    // Start is called before the first frame update
    void Start()
    {
        //init variables
        saveFileName = "slot1";

        canPause = true;
        isPaused = false;

        canPlayerMove = true;
        canPlayerJump = true;
        canPlayerMoveCamera = true;

        canPlayerInteract = true;

        isInDialogue = false;

        canLoadNewScene = true;    
    }

    public void setCanPause(bool b) { canPause = b;}
    
    public void setIsPaused(bool b) { isPaused = b; }

    public void setCanPlayerMove(bool b) { canPlayerMove = b; }

    public void setCanPlayerJump(bool b) { canPlayerJump = b; }

    public void setCanPlayerMoveCamera(bool b) { canPlayerMoveCamera = b; }

    public void setCanPlayerInteract(bool b) { canPlayerInteract = b; }

    public void setIsInDialogue(bool b) { isInDialogue = b; }

    public void setCanLoadNewScene(bool b) { canLoadNewScene = b; }

    public static void incrementTimelineId() { currentEventId++; }

}
