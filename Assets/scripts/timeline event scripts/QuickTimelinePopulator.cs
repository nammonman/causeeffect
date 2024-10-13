using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTimelinePopulator : MonoBehaviour
{
    [SerializeField] GameObject TLPrefab;
    [SerializeField] GameObject content;

    private List<GameObject> trackRefs = new List<GameObject>();
    private void OnEnable()
    {
        instantiateTLPrefabs();
        Debug.Log("quickTL on");
    }

    private void OnDisable()
    {
        clearTLPrefabs();
        Debug.Log("quickTL off");
    }

    public void setTimelineDisplay(TimelineEventDisplay timelineEventDisplay, TimelineEvent thisTimelineEvent)
    {
        timelineEventDisplay.eventNameText.text = thisTimelineEvent.title;
        timelineEventDisplay.dayNumText.text = "DAY " + thisTimelineEvent.day.ToString();
        timelineEventDisplay.timeDayText.text = thisTimelineEvent.timeOfDay.ToString();
        if (thisTimelineEvent.type == 0)
        {
            // start day
            timelineEventDisplay.bg.color = new Color32(85, 122, 138, 255);
        }
        else if (thisTimelineEvent.type == 1)
        {
            // get
            timelineEventDisplay.bg.color = new Color32(119, 158, 160, 255);
        }
        else if (thisTimelineEvent.type == 2)
        {
            // key event
            timelineEventDisplay.bg.color = new Color32(158, 184, 181, 255);
        }
        else if (thisTimelineEvent.type == 3)
        {
            // present
            timelineEventDisplay.bg.color = new Color32(255, 255, 255, 25);
        }
        else
        {
            timelineEventDisplay.bg.color = new Color32(158, 184, 181, 255);
        }
    }

    public void setUpNewInstance(int id)
    {
        GameObject newPrefabInstance = Instantiate(TLPrefab);
        newPrefabInstance.transform.SetParent(content.transform, false);
        newPrefabInstance.name = id.ToString();

        TimelineEvent thisEvent = newPrefabInstance.GetComponent<TimelineEvent>();
        thisEvent.id = MakeTL.TL[id].id;
        thisEvent.type = MakeTL.TL[id].type;
        thisEvent.title = MakeTL.TL[id].title;
        thisEvent.day = MakeTL.TL[id].day;
        thisEvent.timeOfDay = MakeTL.TL[id].timeOfDay;
        thisEvent.screenshotPath = MakeTL.TL[id].screenshotPath;
        thisEvent.saveDataId = MakeTL.TL[id].saveDataId;
        thisEvent.isEventStarted = MakeTL.TL[id].isEventStarted;
        thisEvent.isEventFinished = MakeTL.TL[id].isEventFinished;
        thisEvent.state = MakeTL.TL[id].state;
        thisEvent.nextEventIds = MakeTL.TL[id].nextEventIds;
        thisEvent.lastEventId = MakeTL.TL[id].lastEventId;

        TimelineEventDisplay thisEventDisplay = newPrefabInstance.GetComponent<TimelineEventDisplay>();
        setTimelineDisplay(thisEventDisplay, thisEvent);

        trackRefs.Add(newPrefabInstance);
    }
    public void instantiateTLPrefabs()
    {
        TimelineEvent currentTimelineEvent = GameObject.Find("Persistent Scripts").GetComponent<TimelineEvent>();
        int id = currentTimelineEvent.id;

        int count = 0;
        while (id > 0 && count < 200)
        {
            // prevent infinite loop
            count++;

            setUpNewInstance(id);

            id = MakeTL.TL[id].lastEventId;

            
        }
        setUpNewInstance(0);
        GameObject.Find(GameStateManager.gameStates.currentEventId.ToString()).GetComponent<TimelineEventDisplay>().selectTimeline();
    }

    public void clearTLPrefabs()
    {
        foreach (GameObject t in trackRefs)
        {
            t.GetComponent<TimelineEventDisplay>().deselectTimeline();
            Destroy(t);
        }
        trackRefs.Clear();
    }
}
