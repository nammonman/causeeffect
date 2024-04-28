using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "NpcProperties")]
public class NpcProperties : ScriptableObject
{
    [SerializeField] public string npcName;
    [SerializeField] public bool canTalkTo;
    [SerializeField] public string dialogueStartGUID;
}
