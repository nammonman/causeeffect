using Subtegral.DialogueSystem.DataContainers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NpcDialogue : MonoBehaviour
{
    public bool isFirstInteract = true;
    public bool canInteract = true;
    public Dialogue firstDialogue;
    public Dialogue secondDialogue;
}
