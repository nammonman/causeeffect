using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dream : MonoBehaviour
{
    public string dreamName;
    public string BGSoundFileName;
    public List<DreamText> dreamTexts;

}

public class DreamText
{
    public string text { get; set; }
    public string playSoundFileName { get; set; }
    public string playEffectName { get; set; }

}