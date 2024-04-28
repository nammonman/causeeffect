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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                OnDialogueExit?.Invoke(); 
            }
        }
        public void ProceedToNarrative(string narrativeDataGUID)
        {
            //narrativeDataGUID = dialogue.NodeLinks.First().TargetNodeGUID;
            var text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).DialogueText;
            Debug.Log(text);    
            var name = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).NPCNameText;
            Debug.Log(name);
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
            TextMeshProUGUI[] dialoguePrefabs = dialogueBoxPrefab.GetComponentsInChildren<TextMeshProUGUI>();
            for (int i = 0; i < dialoguePrefabs.Length; i++)
            {
                if (dialoguePrefabs[i].name == "Name")
                    dialoguePrefabs[i].text = ProcessProperties(name);
                if (dialoguePrefabs[i].name == "Dialogue")
                    dialoguePrefabs[i].text = ProcessProperties(text);
            }
            Instantiate(dialogueBoxPrefab, dialogueContainer.transform);

            var buttons = buttonContainer.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }

            foreach (var choice in choices)
            {
                var button = Instantiate(choicePrefab, buttonContainer.transform);
                button.GetComponentInChildren<Text>().text = ProcessProperties(choice.PortName);
                button.onClick.AddListener(() => ProceedToNarrative(choice.TargetNodeGUID));
            }
        }

        private string ProcessProperties(string text)
        {
            foreach (var exposedProperty in dialogue.ExposedProperties)
            {
                text = text.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
            }
            return text;
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