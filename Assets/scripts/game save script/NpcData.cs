using Subtegral.DialogueSystem.DataContainers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveGame
{
    [Serializable]
    public class NpcData
    {
        public Vector3 pos;
        public Quaternion rot;
        public string name;
        public string firstStartNode;
        public string secondStartNode;
        public bool isFirstInteract;
        public bool canInteract;
        public Dialogue firstDialogue;
        public Dialogue secondDialogue;
    }
    
}
