using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Subtegral.DialogueSystem.Editor
{
    public class DialogueNode : Node
    {
        public string GUID;
        public string NPCNameText;
        public string DialogueText;
        public List<string> Trigger;
        public bool EntyPoint = false;
        public bool isSaved = true;
    }
}