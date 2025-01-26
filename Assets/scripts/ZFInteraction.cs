using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZFInteraction : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Button enterButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button upButton;
    [SerializeField] Button downButton;
    [SerializeField] TextMeshProUGUI command;
    [SerializeField] TextMeshPro display;
    [SerializeField] TextMeshPro userInput;
    [SerializeField] GameObject passwordGameObject;
    public TMP_InputField password;
    [SerializeField] GameObject optionsGameObject;
    public TMP_Dropdown options;

    [SerializeField] Button termButton;
    private bool ZFTextsLoaded = false;

    public List<string> availableConversations = new List<string>();
    public List<int> playedTriggers = new List<int>();
    private int currentConversationIndex = -1; // Tracks the current conversation
    private int currentLineIndex = 0; // Tracks the current line in the conversation
    private string selectedConversationKey = "";
    public void AddConversation(string s)
    {
        availableConversations.Add(s);
    }

    private void RefreshDropdownOptions(bool newOnly = false)
    {
        options.ClearOptions();

        List<Dropdown.OptionData> prioritizedOptions = new List<Dropdown.OptionData>();
        List<Dropdown.OptionData> completedOptions = new List<Dropdown.OptionData>();

        foreach (var conversation in availableConversations)
        {
            if (GameStateManager.gameStates.completedZF.Contains(conversation))
            {
                if (!ZFTexts.ZFDialogues[conversation].scripted)
                {
                    // Add completed options with darker text color
                    var optionData = new Dropdown.OptionData
                    {
                        text = $"<color=#888888>{conversation}</color>" // Use a dark gray color for completed options
                    };
                    completedOptions.Add(optionData);
                }
                
            }
            else
            {
                // Add new options
                var optionData = new Dropdown.OptionData
                {
                    text = conversation
                };
                prioritizedOptions.Add(optionData);
            }
        }
        if (GameStateManager.gameStates.fixLevel == 0)
        {
            var pw = new Dropdown.OptionData
            {
                text = "enter password"
            };
            prioritizedOptions.Add(pw);
        }
        if (GameStateManager.gameStates.currentDay >= 6)
        {
            var pw = new Dropdown.OptionData
            {
                text = "<b>obtain government document<\\b>"
            };
            prioritizedOptions.Add(pw);
        }
        // Combine the prioritized options (new) at the top and completed options below
        prioritizedOptions.AddRange(completedOptions);
        options.AddOptions(prioritizedOptions.ConvertAll(option => option.text));
    }

    public void InitZFTexts()
    {
        if (!ZFTextsLoaded)
        {
            password = passwordGameObject.GetComponent<TMP_InputField>();
            options = optionsGameObject.GetComponent<TMP_Dropdown>();
            enterButton.onClick.AddListener(OnEnterPressed);
            upButton.onClick.AddListener(OnUpPressed);
            downButton.onClick.AddListener(OnDownPressed);
            foreach (string item in ZFTexts.ZFDialogues.Keys)
            {
                Debug.Log(item);
                Debug.Log((GameStateManager.gameStates.fixLevel, ZFTexts.ZFDialogues[item].fixLevel));
                if (ZFTexts.ZFDialogues[item].fixLevel <= GameStateManager.gameStates.fixLevel)
                    AddConversation(item);
            }
            RefreshDropdownOptions();
        }
        ZFTextsLoaded = true;
    }

    private void Start()
    {
        termButton.onClick.AddListener(InitZFTexts);
    }
    private void PasswordCheck(string s) 
    {
        if (GameStateManager.gameStates.currentDay >= 6 && GameStateManager.gameStates.globalFlags.Contains("approveLeave"))
        {
            if (s != "pr0j3ct_C")
            {
                display.text = "authentication failed. this incident will be reported";
                userInput.text = ">_";
                GameStateManager.gameStates.globalFlags.Add("FAIL HACK DOCUMENT");
                if (GameStateManager.gameStates.globalFlags.Contains("HACKED DOCUMENT"))
                {
                    GameStateManager.gameStates.globalFlags.Remove("HACKED DOCUMENT");
                }
            }
            else
            {
                GameStateManager.gameStates.globalFlags.Add("HACKED DOCUMENT");
                if (GameStateManager.gameStates.globalFlags.Contains("FAIL HACK DOCUMENT"))
                {
                    GameStateManager.gameStates.globalFlags.Remove("FAIL HACK DOCUMENT");
                }
                display.text = "level 10 document access granted\n" +
                    "copied data to local memory\n" +
                    "override logging system\n" +
                    "logged out";
                userInput.text = ">_";
                RefreshDropdownOptions();
            }
        }
        if (GameStateManager.gameStates.fixLevel == 0)
        {
            if (s != "90103141737")
            {
                display.text = "incorrect password";
                userInput.text = ">_";
            }
            else
            {
                GameStateManager.gameStates.fixLevel++;
                display.text = "you have unlocked the hidden module\nthe self repair module have been acelerated by 1 stage";
                userInput.text = ">_";
                RefreshDropdownOptions();
            }
        }
        
    }
    private void OnEnterPressed()
    {
        selectedConversationKey = command.GetParsedText();
        if (passwordGameObject.activeSelf)
        {
            PasswordCheck(password.text);
            optionsGameObject.SetActive(true);
            passwordGameObject.SetActive(false);
        }
        else if (selectedConversationKey == "enter password" || selectedConversationKey == "<b>obtain government document<\\b>")
        {
            optionsGameObject.SetActive(false);
            passwordGameObject.SetActive(true);
        }
        else if (optionsGameObject.activeSelf && ZFTexts.ZFDialogues.TryGetValue(selectedConversationKey, out ZFDialogue dialogue))
        {
            currentConversationIndex = options.value;
            currentLineIndex = 0;
            playedTriggers.Clear();
            if (dialogue.conversation.Count > 0)
            {
                enterButton.enabled = false;
                optionsGameObject.SetActive(false);
                DisplayCurrentLine(dialogue);
                OnDownPressed();
            }
                
        }
        else if (selectedConversationKey == "enter password")
        {
            display.text = "password: ";
        }
        else
        {
            display.text = "No conversation found for: " + selectedConversationKey + " please contact developer (this is a bug in the game not lore)";
        }
    }

    private void DisplayCurrentLine(ZFDialogue dialogue)
    {
        if (currentLineIndex >= 0 && currentLineIndex < dialogue.conversation.Count)
        {
            var (speaker, text, _) = dialogue.conversation[currentLineIndex];
            if (speaker == "user")
            {
                userInput.text = $">{text}_";
            }
            else
            {
                display.text = text;
            }
        }

    }

    private void OnUpPressed()
    {
        if (currentConversationIndex >= 0)
        {

            if (ZFTexts.ZFDialogues.TryGetValue(selectedConversationKey, out ZFDialogue dialogue))
            {
                if (currentLineIndex > 1)
                {
                    currentLineIndex--;
                    DisplayCurrentLine(dialogue);

                }
            }
        }
    }

    private void OnDownPressed()
    {
        if (currentConversationIndex >= 0)
        {

            if (ZFTexts.ZFDialogues.TryGetValue(selectedConversationKey, out ZFDialogue dialogue))
            {
                if (currentLineIndex < dialogue.conversation.Count - 1)
                {
                    
                    if (dialogue.conversation[currentLineIndex].triggers != null && !playedTriggers.Contains(currentLineIndex))
                    {
                        playedTriggers.Add(currentLineIndex);
                        StartCoroutine(ExecuteFuncsSequentially(dialogue.conversation[currentLineIndex].triggers));
                    }
                    currentLineIndex++;
                    DisplayCurrentLine(dialogue);
                }
                else
                {
                    if (command.text != "enter password")
                    {
                        if (!GameStateManager.gameStates.completedZF.Contains(selectedConversationKey))
                        {
                            GameStateManager.gameStates.completedZF.Add(selectedConversationKey);
                        }
                        enterButton.enabled = true;
                        optionsGameObject.SetActive(true);
                        RefreshDropdownOptions();
                    }
                    
                }
            }
        }
    }


    private bool pauseFlag = false;
    private void Update()
    {
        if (pauseFlag)
        {
            GameStateManager.setPausedState(true);
        }
    }
    private IEnumerator ExecuteFuncsSequentially(List<string> funcs)
    {
        canvas.enabled = false;
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
                else if (f[0] == "kill")
                {
                    //Destroy(gameObject);
                }
                Debug.Log("ran " + func);
            }

            if (parallelCoroutines.Count > 0)
            {
                yield return ExecuteParallelCoroutines(parallelCoroutines);
            }
        }
        canvas.enabled = true;
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
