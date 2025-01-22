using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public bool canReadSecretText;

    //loaded scene
    public bool canLoadNewScene;
    public string CurrentSceneName;
    public string CurrentSceneSetting;

    //zf
    public int fixLevel;
    public List<string> completedZF;
    public int causeEffectPower;

    //story flags
    public List<string> localFlags;
    public List<string> globalFlags;

}

public class GameStateManager : MonoBehaviour
{
    public static GameStates gameStates;
    [SerializeField] TextMeshProUGUI DEBUGTEXT;
    [SerializeField] Slider debugFixLevelSlider;
    [SerializeField] TMP_InputField debugFlagField;
    

    public static event Action<string> OnSceneUpdate;
    public static event Action OnTimeUpdate;
    public static event Action<bool> OnUIEnable;
    public static event Action OnFadeIn;
    public static event Action OnFadeOut;
    public static event Action<int> OnDream;
    public static event Action OnRandomDream;
    public static event Action<string> OnBlackScreenText;
    public static event Action<string> OnCauseeffectText;
    public static event Action OnStartGlitch;
    public static event Action OnStopGlitch;
    public static event Action<bool> OnFreezePlayer;
    public static event Action OnNewTL;
    public static event Action<string> OnNewTLTitle;
    public static event Action<string> OnLoadNewScene;
    public static event Action<string, Vector3> OnLoadNewSceneWithPos;
    public static event Action<string> OnLoadSceneSetting;
    public static event Action OnSave;
    public static event Action<bool> OnSecretText;
    public static event Action<string> OnMonologue;

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
        gameStates.canReadSecretText = false;
        gameStates.completedZF = new List<string>();
        gameStates.globalFlags = new List<string>();
        debugFixLevelSlider.onValueChanged.AddListener(setFixLevelDebug);
    }


    public void DebugAddFlags()
    {
        gameStates.globalFlags.Add(debugFlagField.text);
    }
    public void DebugRemoveFlags()
    {
        gameStates.globalFlags.Remove(debugFlagField.text);
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

    public static void setFixLevel(int i)
    {
        gameStates.fixLevel = i;
    }
    public void setFixLevelDebug(float f)
    {
        gameStates.fixLevel = (int) f;
    }
    public static void setDateTime(int date, int time)
    {
        //Debug.Log(b);
        gameStates.currentDay = date;
        gameStates.currentTimeOfDay = time;
        OnTimeUpdate.Invoke();
    }
    public static void setIncrementTime()
    {
        gameStates.currentTimeOfDay++;
        if (gameStates.currentTimeOfDay > 3)
        {
            gameStates.currentTimeOfDay = 3;
        }
        OnTimeUpdate.Invoke();
    }
    public static void setIncrementDate()
    {
        gameStates.currentDay++;
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

    public static void setRandomDream()
    {
        OnRandomDream.Invoke();
    }
    public static void setBlackScreenText(string s)
    {
        OnBlackScreenText.Invoke(s);
    }
    public static void setCauseeffectText(string s)
    {
        OnCauseeffectText.Invoke(s);
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
    public static void setNewTLTitle(string s)
    {
        Debug.Log("new tl title: "+s);
        OnNewTLTitle.Invoke(s);
    }
    public static void setLoadNewScene(string s)
    {
        OnLoadNewScene.Invoke(s);
    }
    public static void setLoadNewSceneWithPos(string s, Vector3 v3)
    {
        OnLoadNewSceneWithPos.Invoke(s, v3);
    }

    public static void setLoadSceneSetting(string s)
    {
        OnLoadSceneSetting.Invoke(s);
    }
    public static void setSave()
    {
        OnSave.Invoke();
    }

    public static void setSecretText(bool b)
    {
        OnSecretText.Invoke(b);
    }

    public static void setMonologue(string s)
    {
        OnMonologue.Invoke(s); 
    }
}
