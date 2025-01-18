using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ZFDialogue
{
    public List<(string speaker, string text, List<string> triggers)> conversation;
    public int fixLevel = 0;
    public bool scripted = false;

}

