using Subtegral.DialogueSystem.DataContainers;
using Subtegral.DialogueSystem.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class raycastinteract : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Transform playerCamera;
    [SerializeField] public GameObject wholeDialogueContainer;
    [SerializeField] public TextMeshProUGUI promptText;
    [SerializeField] public float rayDist = 10f;

    public delegate void DialogueEnter(string GUID, bool E, Dialogue D);
    public static event DialogueEnter OnDialogueEnter;

    private bool objInteractable = false;

    public void Start()
    {
        promptText.text = "[E] to interact";
    }

    // Update is called once per frame
    public void Update()
    {
        if (GameStateManager.gameStates.canPlayerInteract)
        {
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward*rayDist, Color.blue);
            RaycastHit hitObject;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitObject, rayDist)) //raycast
            {
                // check if tag is interactable
                if (hitObject.collider.gameObject.tag == "Iobj" || hitObject.collider.gameObject.tag == "npc")
                {
                    objInteractable = true;
                }
                else
                {
                    objInteractable = false;
                }

                // set prompt text
                if (objInteractable && !promptText.gameObject.activeSelf)
                {
                    promptText.gameObject.SetActive(true);
                }
                else if (!objInteractable && promptText.gameObject.activeSelf)
                {
                    promptText.gameObject.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log(hitObject.transform.name);
                }

                // get input and run funcs
                if (objInteractable && Input.GetKeyDown(KeyCode.E))
                {
                    

                    if (hitObject.collider.gameObject.tag == "npc")
                    {
                        JoinConversation();
                        NpcDialogue npcDialogue = hitObject.collider.gameObject.GetComponent<NpcDialogue>();
                        if (npcDialogue != null)
                        {
                            if (npcDialogue.isFirstInteract)
                            {
                                OnDialogueEnter?.Invoke(npcDialogue.firstDialogue.NodeLinks[0].TargetNodeGUID, true, npcDialogue.firstDialogue);
                                npcDialogue.isFirstInteract = false;    
                            }
                            else
                            {
                                OnDialogueEnter?.Invoke(npcDialogue.secondDialogue.NodeLinks[0].TargetNodeGUID, true, npcDialogue.secondDialogue);
                            }
                        }
                    }
                    else if (hitObject.collider.gameObject.tag == "Iobj")
                    {
                        RunFuncs(hitObject.collider.gameObject.GetComponent<IObjProperties>().funcs);
                    }
                }

                if (GameStateManager.gameStates.canSeeSecretText && GameStateManager.gameStates.canReadSecretText && hitObject.collider.gameObject.tag == "secret text")
                {
                    string seenSecretText = hitObject.collider.gameObject.GetComponent<TMP_Text>().text;
                    int index = NotebookSwitcher.notes.IndexOf(seenSecretText);
                    if (index > -1 && !NotebookSwitcher.unlockedNotes.Contains(index))
                    {
                        NotebookSwitcher.unlockedNotes.Add(index);
                        StartCoroutine(SwitchPromptextForSeconds(2, "new entry added to notes"));
                        Debug.Log("new entry added to notes");
                        Debug.Log(seenSecretText);
                    }
                    
                }

            }
            else if (promptText.gameObject.activeSelf)
            {
                objInteractable = false;
                promptText.gameObject.SetActive(false);
            }
        }


    }

    IEnumerator SwitchPromptextForSeconds(float delay, string s)
    {
        promptText.text = s;
        promptText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        promptText.gameObject.SetActive(false);
        promptText.text = "[E] to interact";
    }

    IEnumerator PauseDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameStateManager.setPausedState(false);
        //Debug.Log("Enabled Pausing");
    }

    public void JoinConversation()
    {
        
        GameStateManager.setPausedState(true);
        GameStateManager.gameStates.canPause = false;
        GameStateManager.gameStates.isInDialogue = true;
        wholeDialogueContainer.SetActive(true);
    }

    public void LeaveConversation()
    {
        
        StartCoroutine(PauseDelay(0.1f));
        wholeDialogueContainer.SetActive(false);
        GameStateManager.gameStates.canPause = true;
        GameStateManager.gameStates.isInDialogue = false;
    }

    private void OnEnable()
    {
        DialogueParser.OnDialogueExit += LeaveConversation;
    }

    private void OnDisable()
    {
        DialogueParser.OnDialogueExit -= LeaveConversation;
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
            string[] parallelFuncs = item.Split(' '); // Split multiple functions by space
            List<IEnumerator> parallelCoroutines = new List<IEnumerator>();

            foreach (var func in parallelFuncs)
            {
                string[] f = func.Split('_'); // Split individual function and arguments

                if (f[0] == "FadeBlack")
                {
                    parallelCoroutines.Add(FadeBlackForSeconds(float.Parse(f[1])));
                }
                else if (f[0] == "BlackScreenText")
                {
                    parallelCoroutines.Add(BlackScreenTextCoroutine(f[1]));
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
                else if (f[0] == "NewTL")
                {
                    GameStateManager.setNewTL();
                }
                else if (f[0] == "NewTLTitle")
                {
                    GameStateManager.setNewTLTitle(f[1]);
                }
                else if (f[0] == "kill")
                {
                    //Destroy(gameObject);
                }
                Debug.Log("ran " + item);
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

    IEnumerator BlackScreenTextCoroutine(string name)
    {
        bool isDone = false;

        GameStateManager.setBlackScreenText(name);

        DreamTextSwitcher.OnEnd += () => isDone = true;
        yield return new WaitUntil(() => isDone);
    }

    IEnumerator LoadSceneSettingCoroutine(string name)
    {
        bool isDone = false;

        GameStateManager.setLoadSceneSetting(name);

        MakeTL.OnEnd += () => isDone = true;
        yield return new WaitUntil(() => isDone);
    }
}
