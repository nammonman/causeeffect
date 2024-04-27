using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class dialogueobject : ScriptableObject
{
    public DialogueSection[] sections;

    [System.Serializable]
    public struct DialogueSection
    {
        [TextArea]
        public List<string> dialogue;
        public bool endAfterDialogue;
        public PointBranch pointBranch;
    }

    [System.Serializable]
    public struct PointBranch
    {
        [TextArea]
        public string question;
        public List<Answer> answers;
    }

    [System.Serializable] 
    public struct Answer 
    {
        public string answerText;
        public int next;
    }
}
