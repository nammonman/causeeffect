using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;

namespace Subtegral.DialogueSystem.Runtime
{
    public class DialogueParser : MonoBehaviour
    {
        [SerializeField] private DialogueContainer dialogue;
        [SerializeField] private Button choicePrefab;
        [SerializeField] private GameObject buttonContainer;
        [SerializeField] private GameObject dialogueContainer;
        [SerializeField] private GameObject dialogueBoxPrefab;
        [SerializeField] private GameObject yourDialogueBoxPrefab;

        public delegate void DialogueExit();
        public static event DialogueExit OnDialogueExit;
        public Transform[] dialogues;
        public Button[] buttons;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                //destroyExistingButtons(buttons);
                //destroyExistingDialogueBoxes(dialogues);
                OnDialogueExit?.Invoke(); 
            }
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

        public void ProceedToNarrative(string narrativeDataGUID, bool fromInteract)
        {
            //StartCoroutine(DelayResponse(1.5f));  
            //narrativeDataGUID = dialogue.NodeLinks.First().TargetNodeGUID;
            var text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).DialogueText;
            //Debug.Log(text);    
            var name = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).NPCNameText;
            //Debug.Log(name);
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
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
            //dialogueContainer.transform.position.Set(dialogueContainer.transform.position.x, dialogueContainer.transform.position.y + 120, dialogueContainer.transform.position.z);

            buttons = buttonContainer.GetComponentsInChildren<Button>();
            destroyExistingButtons(buttons);
            
            foreach (var choice in choices)
            {
                var button = Instantiate(choicePrefab, buttonContainer.transform);
                button.GetComponentInChildren<Text>().text = ProcessProperties(choice.PortName);
                button.onClick.AddListener(() => answerChoice(choice.TargetNodeGUID, choice.PortName));
                //buttonContainer.transform.position.Set(buttonContainer.transform.position.x, buttonContainer.transform.position.y - 60, buttonContainer.transform.position.z);
            }
            
        }

        private void answerChoice(string GUID, string PortName)
        {
            TextMeshProUGUI[] yourDialoguePrefabs = yourDialogueBoxPrefab.GetComponentsInChildren<TextMeshProUGUI>();
            yourDialoguePrefabs[1].text = ProcessProperties(PortName);
            Instantiate(yourDialogueBoxPrefab, dialogueContainer.transform);
            ProceedToNarrative(GUID, false);
        }

        private string ProcessProperties(string text)
        {
            foreach (var exposedProperty in dialogue.ExposedProperties)
            {
                text = text.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
            }
            return text;
        }

        IEnumerator DelayResponse(float seconds)
        {
            yield return new WaitForSeconds(seconds);
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