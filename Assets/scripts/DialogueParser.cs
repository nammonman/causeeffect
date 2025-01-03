using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;
using static UnityEditor.Progress;

namespace Subtegral.DialogueSystem.Runtime
{
    public class DialogueParser : MonoBehaviour
    {
        [SerializeField] private Dialogue dialogue;
        [SerializeField] private Button choicePrefab;
        [SerializeField] private GameObject buttonContainer;
        [SerializeField] private GameObject dialogueContainer;
        [SerializeField] private GameObject dialogueBoxPrefab;
        [SerializeField] private GameObject yourDialogueBoxPrefab;

        public delegate void DialogueExit();
        public static event DialogueExit OnDialogueExit;
        public Transform[] dialogues;
        public Button[] buttons;

        private bool autoScroll = false;

        void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.Escape) && GameStateManager.gameStates.isInDialogue) 
            {
                //destroyExistingButtons(buttons);
                //destroyExistingDialogueBoxes(dialogues);
                OnDialogueExit?.Invoke(); 
            }*/
            if (autoScroll)
            {
                Vector3 temp = dialogueContainer.transform.position;
                dialogueContainer.transform.position = new Vector3(temp.x, temp.y + 10000, temp.z);
            }
            //Debug.Log(autoScroll);
        }

        private void destroyExistingDialogueBoxes(Transform[] dialogues)
        {
            for (int i = 1; i < dialogues.Length; i++)
            {
                Destroy(dialogues[i].gameObject);
                //dialogueContainer.transform.position.Set(dialogueContainer.transform.position.x, dialogueContainer.transform.position.y - 40, dialogueContainer.transform.position.z);
            }
        }

        private void destroyExistingButtons(Button[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
                //buttonContainer.transform.position.Set(buttonContainer.transform.position.x, buttonContainer.transform.position.y - 20, buttonContainer.transform.position.z);
            }
        }

        private void leaveNarrative()
        {
            buttons = buttonContainer.GetComponentsInChildren<Button>();
            destroyExistingButtons(buttons);
            OnDialogueExit?.Invoke();
        }

        public IEnumerator ProceedToNarrative(string narrativeDataGUID, bool fromInteract, Dialogue npcDialogue)
        {
            dialogue = npcDialogue;
            var text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).DialogueText;
            var name = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).NPCNameText;
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
            var triggers = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).Trigger;
            bool leaveFlag = false;

            // Create a list to store all trigger coroutines
            List<Coroutine> triggerCoroutines = new List<Coroutine>();

            // Start all triggers and store their coroutines
            foreach (var trigger in triggers)
            {
                if (trigger != null)
                {
                    if (trigger == "leave")
                    {
                        leaveFlag = true;
                    }
                    triggerCoroutines.Add(StartCoroutine(triggerEvent(trigger)));
                }
            }

            // Wait for all triggers to complete
            foreach (var coroutine in triggerCoroutines)
            {
                yield return coroutine;
            }

            TextMeshProUGUI[] dialoguePrefabs = dialogueBoxPrefab.GetComponentsInChildren<TextMeshProUGUI>();
            dialogues = dialogueContainer.GetComponentsInChildren<Transform>();
            if (fromInteract)
            {
                destroyExistingDialogueBoxes(dialogues);
            }
            for (int i = 0; i < dialoguePrefabs.Length; i++)
            {
                if (dialoguePrefabs[i].name == "Name")
                    dialoguePrefabs[i].text = ProcessProperties(name);
                if (dialoguePrefabs[i].name == "Dialogue")
                    dialoguePrefabs[i].text = ProcessProperties(text);
            }
            Instantiate(dialogueBoxPrefab, dialogueContainer.transform);
            
            if (!leaveFlag)
            {
                foreach (var choice in choices)
                {
                    var button = Instantiate(choicePrefab, buttonContainer.transform);
                    button.GetComponentInChildren<TextMeshProUGUI>().text = ProcessProperties(choice.PortName);
                    button.onClick.AddListener(() => answerChoice(choice.TargetNodeGUID, choice.PortName));
                    //buttonContainer.transform.position.Set(buttonContainer.transform.position.x, buttonContainer.transform.position.y - 60, buttonContainer.transform.position.z);
                }
                if (choices.Count() < 1)
                {
                    var button = Instantiate(choicePrefab, buttonContainer.transform);
                    button.GetComponentInChildren<TextMeshProUGUI>().text = "I won’t keep you any longer";
                    button.onClick.AddListener(() => leaveNarrative());
                }
            }
        }

        private void answerChoice(string GUID, string PortName)
        {
            TextMeshProUGUI[] yourDialoguePrefabs = yourDialogueBoxPrefab.GetComponentsInChildren<TextMeshProUGUI>();
            yourDialoguePrefabs[1].text = ProcessProperties(PortName);
            Instantiate(yourDialogueBoxPrefab, dialogueContainer.transform);
            autoScroll = true;
            StartCoroutine(DelayedResponse(1f, GUID, false));

        }

        private string ProcessProperties(string text)
        {
            foreach (var exposedProperty in dialogue.ExposedProperties)
            {
                text = text.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
            }
            return text;
        }
 
        private IEnumerator triggerEvent(string e) // TRIGGER EVENT FROM ANSWER BUTTON
        {

            string[] parallelFuncs = e.Split(' '); // Split multiple functions by space
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
                else if (f[0] == "leave")
                {
                    leaveNarrative();
                }
                Debug.Log("ran " + func);
            }

            if (parallelCoroutines.Count > 0)
            {
                yield return ExecuteParallelCoroutines(parallelCoroutines);
            }
            yield return null;
        }

        IEnumerator DelayedResponse(float seconds, string GUID, bool fromInteract)
        {
            autoScroll = true;
            buttons = buttonContainer.GetComponentsInChildren<Button>();
            destroyExistingButtons(buttons);
            yield return new WaitForSeconds(seconds);
            ProceedToNarrativeCaller(GUID, false, dialogue);
            yield return new WaitForSeconds(0.1f);
            autoScroll = false;
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

        private void OnEnable()
        {
            raycastinteract.OnDialogueEnter += ProceedToNarrativeCaller;
        }

        private void OnDisable()
        {
            raycastinteract.OnDialogueEnter -= ProceedToNarrativeCaller;
        }

        private void ProceedToNarrativeCaller(string narrativeDataGUID, bool fromInteract, Dialogue npcDialogue)
        {
            StartCoroutine(ProceedToNarrative(narrativeDataGUID, fromInteract, npcDialogue));
        }
    }
}