using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static dialogueobject;

public class dialoguecontroller : MonoBehaviour
{
    public static dialoguecontroller instance;

    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI npcNameText;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject ansBox;
    [SerializeField] List<Button> ansButton;

    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;

    bool skipLineTriggered;
    bool ansTriggered;
    int ansIndex;
    int marginHeight = 0;
    RectTransform rectTransform;
    // check that only one instance is active
    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void startDialogue(dialogueobject dialogueTree, int startAt, string npcName)
    {
        rectTransform = dialogueText.GetComponent<RectTransform>();
        npcNameText.text = npcName;
        dialogueBox.SetActive(true);
        ResetBox();
        StopAllCoroutines();
        StartCoroutine(RunDialogue(dialogueTree, startAt));
    }
    public void addTextNewLine(string s)
    {
        rectTransform.sizeDelta += new Vector2(0, 20);
        dialogueText.text += '\n';
        dialogueText.text += s;
    }
    IEnumerator RunDialogue(dialogueobject dialogueTree, int startAt)
    {
        skipLineTriggered = false;
        OnDialogueStarted?.Invoke();

        for (int i = startAt; i < dialogueTree.sections[startAt].dialogue.Count; i++)
        {
            
            addTextNewLine(dialogueTree.sections[startAt].dialogue[i]);
            while (skipLineTriggered == false)
            {
                // Wait for the current line to be skipped
                yield return null;
            }
            skipLineTriggered = false;
        }

        if (dialogueTree.sections[startAt].endAfterDialogue)
        {
            OnDialogueEnded?.Invoke();
            dialogueBox.SetActive(false);
            dialogueText.text = "";
            yield break;
        }

        addTextNewLine(dialogueTree.sections[startAt].pointBranch.question);
        showAns(dialogueTree.sections[startAt].pointBranch);

        while (ansTriggered == false)
        {
            yield return null;
        }
        Debug.Log("go to" + dialogueTree.sections[startAt].pointBranch.answers[ansIndex].next);
        addTextNewLine("> " + dialogueTree.sections[startAt].pointBranch.answers[ansIndex].answerText);
        ansBox.SetActive(false);
        ansTriggered = false;

        StartCoroutine(RunDialogue(dialogueTree, dialogueTree.sections[startAt].pointBranch.answers[ansIndex].next));

    }

    // set default state
    void ResetBox()
    {
        StopAllCoroutines();
        dialogueBox.SetActive(false);
        ansBox.SetActive(false);
        skipLineTriggered = false;
        ansTriggered = false;
    }

    void showAns(PointBranch pointBranch)
    {
        ansBox.SetActive(true);
        for (int i = 0; i < pointBranch.answers.Count; i++)
        {
            ansButton[i].GetComponentInChildren<TextMeshProUGUI>().text = pointBranch.answers[i].answerText;
            ansButton[i].gameObject.SetActive(true);
        }
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
    }

    public void ans(int choice)
    {
        ansIndex = choice;
        ansTriggered = true;
    }
}
