using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimelineEvent : MonoBehaviour
{
    public int id;
    public int type;
    public string title = "default title";
    public int day;
    public int timeOfDay;
    public string description = "default description";
    public string screenshotPath;
    public int saveDataId;
    public bool isEventStarted;
    public bool isEventFinished;
    public string state;

    // for full timeline display
    public List<int> nextEventIds;

    // for current timeline display
    // only need previous event because you can't time travel in to the future
    public int lastEventId;

    public TimelineEvent Clone()
    {
        return new TimelineEvent
        {
            id = this.id,
            type = this.type,
            title = this.title,
            day = this.day,
            timeOfDay = this.timeOfDay,
            description = this.description,
            screenshotPath = this.screenshotPath,
            saveDataId = this.saveDataId,
            isEventStarted = this.isEventStarted,
            isEventFinished = this.isEventFinished,
            state = this.state,
            nextEventIds = new List<int>(this.nextEventIds), // Deep copy the list
            lastEventId = this.lastEventId
        };
    }
}

