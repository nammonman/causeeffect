using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimelineEvent : MonoBehaviour
{
    public int type;
    public string title = "default title";
    public int day;
    public int timeOfDay;
    public string description = "default description";
    public int lastEventId;
    public bool isEventStarted;
    public bool isEventFinished;
    public string state;
    public string screenShotPath;
}

