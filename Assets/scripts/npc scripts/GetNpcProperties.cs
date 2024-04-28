using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNpcProperties : MonoBehaviour
{
    [SerializeField] private NpcProperties npcProperties;

    public NpcProperties GetNpc()
    {
        return npcProperties;
    }
}
