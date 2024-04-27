using System;
using System.Collections.Generic;
using UnityEngine;

namespace Subtegral.DialogueSystem.DataContainers
{
    [Serializable]
    public class DialogueNodeData
    {
        public string NodeGUID;
        public string NPCNameText;
        public string DialogueText;
        //public List<string> choiceTexts;
        public Vector2 Position;
    }
}