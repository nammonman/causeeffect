using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //secret text
    public bool canSeeSecretText;

    //loaded scene
    public bool canLoadNewScene;
    public string CurrentSceneName;
    public string CurrentSceneSetting;

    // Method to create a deep copy of GameStates
    public GameStates Clone()
    {
        return new GameStates
        {
            saveFileName = this.saveFileName,
            isSaving = this.isSaving,
            saveId = this.saveId,
            currentEventId = this.currentEventId,
            currentDay = this.currentDay,
            currentTimeOfDay = this.currentTimeOfDay,
            activeEventType = this.activeEventType,
            isInActiveEvent = this.isInActiveEvent,
            canPause = this.canPause,
            isPaused = this.isPaused,
            canPlayerMove = this.canPlayerMove,
            canPlayerJump = this.canPlayerJump,
            canPlayerMoveCamera = this.canPlayerMoveCamera,
            canPlayerInteract = this.canPlayerInteract,
            isInDialogue = this.isInDialogue,
            canLoadNewScene = this.canLoadNewScene,
            CurrentSceneName = this.CurrentSceneName,
            CurrentSceneSetting = this.CurrentSceneSetting
        };
    }
}

public class GameStateManager : MonoBehaviour
{
    public static GameStates gameStates;
    [SerializeField] TextMeshProUGUI DEBUGTEXT;

    public static event Action<string> OnSceneUpdate;
    public static event Action OnTimeUpdate;
    public static event Action<bool> OnUIEnable;
    public static event Action OnFadeIn;
    public static event Action OnFadeOut;
    public static event Action<int> OnDream;
    public static event Action OnStartGlitch;
    public static event Action OnStopGlitch;
    public static event Action<bool> OnFreezePlayer;
    public static event Action OnNewTL;
    public static event Action<string> OnLoadNewScene;
    public static event Action<string, Vector3> OnLoadNewSceneWithPos;
    public static event Action OnSave;
    public static event Action<bool> OnSecretText;

    private void FixedUpdate()
    {
        if (DEBUGTEXT.enabled)
        {
            DEBUGTEXT.text = JsonUtility.ToJson(gameStates, true);
        }
    }
    private void Awake()
    {
        gameStates = new GameStates();
        
        //init variables
        gameStates.saveFileName = "slot1";
        gameStates.CurrentSceneName = SceneManager.GetActiveScene().name;
        gameStates.canSeeSecretText = false;
    }


    // Start is called before the first frame update
    void Start()
    {
 
    }

    public static void setPausedState(bool b)
    {
        //Debug.Log(b);

        gameStates.isPaused = b;
        gameStates.canPlayerMove = !b;
        gameStates.canPlayerJump = !b;
        gameStates.canPlayerMoveCamera = !b;
        gameStates.canPlayerInteract = !b;


    }

    public static void setDateTime(int date, int time)
    {
        //Debug.Log(b);
        gameStates.currentDay = date;
        gameStates.currentTimeOfDay = time;
        OnTimeUpdate.Invoke();
    }

    public static void setSceneName(string name)
    {
        //Debug.Log(b);
        gameStates.CurrentSceneName = name;

        OnSceneUpdate.Invoke(name);
    }

    public static void setUIVisible(bool b)
    {
        OnUIEnable.Invoke(b);
    }

    public static void setScreenFadeIn()
    {
        OnFadeIn.Invoke();
    }

    public static void setScreenFadeOut()
    {
        OnFadeOut.Invoke();
    }

    public static void setDream(int i)
    {
        OnDream.Invoke(i);
    }

    public static void setStartGlitch()
    {
        OnStartGlitch.Invoke();
    }

    public static void setStopGlitch()
    {
        OnStopGlitch.Invoke();
    }

    public static void setFreeze(bool b)
    {
        OnFreezePlayer.Invoke(b);
    }

    public static void setNewTL()
    {
        OnNewTL.Invoke();
    }

    public static void setLoadNewScene(string s)
    {
        OnLoadNewScene.Invoke(s);
    }
    public static void setLoadNewSceneWithPos(string s, Vector3 v3)
    {
        OnLoadNewSceneWithPos.Invoke(s, v3);
    }


    public static void setSave()
    {
        OnSave.Invoke();
    }

    public static void setSecretText(bool b)
    {
        OnSecretText.Invoke(b);
    }
}
