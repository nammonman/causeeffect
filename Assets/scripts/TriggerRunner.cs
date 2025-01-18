using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRunner : MonoBehaviour
{
    public static TriggerRunner Instance { get; private set; }
    private bool pauseFlag = false;

    private void Awake()
    {
        if (Instance == null)
        {
            // If no instance exists, set this as the instance
            Instance = this;
            // Optionally make this instance persistent across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (pauseFlag)
        {
            GameStateManager.setPausedState(true);
        }
    }
    public static void RunFuncsCaller(List<string> funcs)
    {
        Instance.RunFuncs(funcs);
    }

    public void RunFuncs(List<string> funcs)
    {
        if (funcs.Count > 0)
        {
            StartCoroutine(ExecuteFuncsSequentially(funcs));
        }
    }

    private IEnumerator ExecuteFuncsSequentially(List<string> funcs)
    {
        foreach (var item in funcs)
        {
            string[] parallelFuncs = item.Split('|'); // Split multiple functions
            List<IEnumerator> parallelCoroutines = new List<IEnumerator>();

            foreach (var func in parallelFuncs)
            {
                string[] f = func.Split('_'); // Split individual function and arguments

                if (f[0] == "FadeBlack")
                {
                    parallelCoroutines.Add(FadeBlackForSeconds(float.Parse(f[1])));
                }
                else if (f[0] == "RandomDreamText")
                {
                    parallelCoroutines.Add(RandomDreamTextCoroutine());
                }
                else if (f[0] == "BlackScreenText")
                {
                    parallelCoroutines.Add(BlackScreenTextCoroutine(f[1]));
                }
                else if (f[0] == "CauseeffectText")
                {
                    parallelCoroutines.Add(CauseeffectTextCoroutine(f[1]));
                }
                else if (f[0] == "Monologue")
                {
                    parallelCoroutines.Add(MonologueCoroutine(f[1]));
                }
                else if (f[0] == "Glitch")
                {
                    parallelCoroutines.Add(GlitchForSeconds(float.Parse(f[1])));
                }
                else if (f[0] == "Wait")
                {
                    parallelCoroutines.Add(WaitSeconds(float.Parse(f[1])));
                }
                else if (f[0] == "ChangeScene")
                {
                    parallelCoroutines.Add(LoadSceneCoroutine(f[1]));
                }
                else if (f[0] == "ChangeSceneSetPos")
                {
                    parallelCoroutines.Add(LoadSceneWithPosCoroutine(f[1], new Vector3(int.Parse(f[2]), int.Parse(f[3]), int.Parse(f[4]))));
                }
                else if (f[0] == "ChangeSetting")
                {
                    parallelCoroutines.Add(LoadSceneSettingCoroutine(f[1]));
                }
                else if (f[0] == "pause")
                {
                    pauseFlag = true;
                }
                else if (f[0] == "unpause")
                {
                    pauseFlag = false;
                    GameStateManager.setPausedState(false);
                }
                else if (f[0] == "freeze")
                {
                    GameStateManager.setFreeze(true);
                }
                else if (f[0] == "unfreeze")
                {
                    GameStateManager.setFreeze(false);
                }
                else if (f[0] == "NewTL")
                {
                    GameStateManager.setNewTL();
                }
                else if (f[0] == "NewTLTitle")
                {
                    GameStateManager.setNewTLTitle(f[1]);
                }
                else if (f[0] == "IncrementTime")
                {
                    GameStateManager.setIncrementTime();
                }
                else if (f[0] == "SetDateTime")
                {
                    GameStateManager.setDateTime(int.Parse(f[1]), int.Parse(f[2]));
                }
                else if (f[0] == "IncrementFix")
                {
                    GameStateManager.gameStates.fixLevel++;
                }
                else if (f[0] == "AddFlag")
                {
                    if (!GameStateManager.gameStates.globalFlags.Contains(f[1]))
                    {
                        GameStateManager.gameStates.globalFlags.Add(f[1]);
                    }
                }
                else if (f[0] == "RemoveFlag")
                {
                    if (GameStateManager.gameStates.globalFlags.Contains(f[1]))
                    {
                        GameStateManager.gameStates.globalFlags.Remove(f[1]);
                    }
                }
                else if (f[0] == "UnlockNote")
                {
                    if (NotebookSwitcher.unlockedNotes.Contains(int.Parse(f[1])))
                    {
                        NotebookSwitcher.AddNote(int.Parse(f[1]));
                    }
                }
                else if (f[0] == "ChangeSettingCurrentDayHallway")
                {
                    parallelCoroutines.Add(LoadSceneSettingCoroutine($"WorkHallwayNPC{GameStateManager.gameStates.currentDay}-1"));
                }
                Debug.Log("ran " + func);
            }

            if (parallelCoroutines.Count > 0)
            {
                yield return ExecuteParallelCoroutines(parallelCoroutines);
            }
        }
    }

    private IEnumerator ExecuteParallelCoroutines(List<IEnumerator> coroutines)
    {
        List<Coroutine> runningCoroutines = new List<Coroutine>();

        foreach (var coroutine in coroutines)
        {
            runningCoroutines.Add(StartCoroutine(coroutine));
        }

        foreach (var runningCoroutine in runningCoroutines)
        {
            yield return runningCoroutine;
        }
    }

    IEnumerator FadeBlackForSeconds(float delay)
    {
        GameStateManager.setPausedState(true);
        GameStateManager.setScreenFadeIn();
        yield return new WaitForSeconds(delay);
        GameStateManager.setScreenFadeOut();
        GameStateManager.setPausedState(false);
    }

    IEnumerator GlitchForSeconds(float delay)
    {
        GameStateManager.setStartGlitch();
        yield return new WaitForSeconds(delay);
        GameStateManager.setStopGlitch();
    }

    IEnumerator WaitSeconds(float delay) { yield return new WaitForSeconds(delay); }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        bool isDone = false;

        GameStateManager.setLoadNewScene(sceneName);

        LoadScene.OnEnd += () => isDone = true;
        yield return new WaitUntil(() => isDone);
    }

    IEnumerator LoadSceneWithPosCoroutine(string sceneName, Vector3 position)
    {
        bool isDone = false;

        GameStateManager.setLoadNewSceneWithPos(sceneName, position);

        LoadScene.OnEnd += () => isDone = true;
        yield return new WaitUntil(() => isDone);
    }
    IEnumerator RandomDreamTextCoroutine()
    {
        bool isDone = false;

        GameStateManager.setRandomDream();

        DreamTextSwitcher.OnEnd += () => isDone = true;
        yield return new WaitUntil(() => isDone);
    }
    IEnumerator BlackScreenTextCoroutine(string name)
    {
        bool isDone = false;

        GameStateManager.setBlackScreenText(name);

        DreamTextSwitcher.OnEnd += () => isDone = true;
        yield return new WaitUntil(() => isDone);
    }
    IEnumerator CauseeffectTextCoroutine(string name)
    {
        bool isDone = false;

        GameStateManager.setCauseeffectText(name);

        CauseeffectTextSwitcher.OnEnd += () => isDone = true;
        yield return new WaitUntil(() => isDone);
    }
    IEnumerator MonologueCoroutine(string text)
    {
        bool isDone = false;

        GameStateManager.setMonologue(text);

        ShowMonologue.OnEnd += () => isDone = true;
        yield return new WaitUntil(() => isDone);
        yield return new WaitForSeconds(0.9f);
    }
    IEnumerator LoadSceneSettingCoroutine(string name)
    {
        bool isDone = false;

        GameStateManager.setLoadSceneSetting(name);

        MakeTL.OnEnd += () => isDone = true;
        yield return new WaitUntil(() => isDone);
    }
}


