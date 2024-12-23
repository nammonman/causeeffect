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

        public void ProceedToNarrative(string narrativeDataGUID, bool fromInteract, Dialogue npcDialogue)
        {
            dialogue = npcDialogue;
            var text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).DialogueText;
            //Debug.Log(text);    
            var name = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).NPCNameText;
            //Debug.Log(name);
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);

            var triggers = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).Trigger;
            foreach (var trigger in triggers)
            {
                if (trigger != null) { triggerEvent(trigger); }
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
            
            foreach (var choice in choices)
            {
                var button = Instantiate(choicePrefab, buttonContainer.transform);
                button.GetComponentInChildren<Text>().text = ProcessProperties(choice.PortName);
                button.onClick.AddListener(() => answerChoice(choice.TargetNodeGUID, choice.PortName));
                //buttonContainer.transform.position.Set(buttonContainer.transform.position.x, buttonContainer.transform.position.y - 60, buttonContainer.transform.position.z);
            }
            if (choices.Count() < 1)
            {
                var button = Instantiate(choicePrefab, buttonContainer.transform);
                button.GetComponentInChildren<Text>().text = "I won’t keep you any longer";
                button.onClick.AddListener(() => leaveNarrative()); 
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
 
        private void triggerEvent(string e) // TRIGGER EVENT FROM ANSWER BUTTON
        {
            // trigger event named e
            Debug.Log("trigger event " + e + " from dialogue");

            string[] f = e.Split('_');
            // funcname_arg1_arg2_arg3_...
            
            if (f[0] == "ChaneScene")
            {
                GameStateManager.setLoadNewScene(f[1]);
            }
            else if (f[0] == "ChaneSceneSetPos")
            {
                GameStateManager.setLoadNewSceneWithPos(f[1], new Vector3(int.Parse(f[2]), int.Parse(f[3]), int.Parse(f[4])));
            }
            else if (f[0] == "ChaneSetting")
            {
                GameStateManager.setLoadSceneSetting(f[1]);
            }
        }

        IEnumerator DelayedResponse(float seconds, string GUID, bool fromInteract)
        {
            autoScroll = true;
            buttons = buttonContainer.GetComponentsInChildren<Button>();
            destroyExistingButtons(buttons);
            yield return new WaitForSeconds(seconds);
            ProceedToNarrative(GUID, false, dialogue);
            yield return new WaitForSeconds(0.1f);
            autoScroll = false;
        }



        private void OnEnable()
        {
            raycastinteract.OnDialogueEnter += ProceedToNarrative;
        }

        private void OnDisable()
        {
            raycastinteract.OnDialogueEnter -= ProceedToNarrative;
        }
    }
}