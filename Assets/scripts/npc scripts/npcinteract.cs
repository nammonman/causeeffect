using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class npcinteract : MonoBehaviour
{
    [SerializeField] bool firstInteraction = true;
    [SerializeField] int repeatStartAt;

    public string npcName;
    public dialogueobject dialogueObject;

    [HideInInspector]
    public int StartAt
    {
        get
        {
            if (firstInteraction)
            {
                firstInteraction = false;
                return 0;
            }
            else
            {
                return repeatStartAt;
            }
        }
    }
}
