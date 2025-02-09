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
        string[] t = { "MORNING", "AFTERNOON", "EVENING", "NIGHT" };
        timelineEventDisplay.dayNumText.text = "DAY " + thisTimelineEvent.day.ToString();
        timelineEventDisplay.timeDayText.text = t[thisTimelineEvent.timeOfDay];
        if (thisTimelineEvent.id == 0)
        {
            // root
            timelineEventDisplay.bg.color = new Color32(0, 12, 28, 255);
        }
        else if (thisTimelineEvent.id == 1)
        {
            // get
            timelineEventDisplay.bg.color = new Color32(0, 12, 28, 255);
        }
        else if (thisTimelineEvent.name.EndsWith("ENDING"))
        {
            // ending event
            timelineEventDisplay.bg.color = new Color32(156, 183, 181, 255);
        }
        else if (thisTimelineEvent.name == "PRESENT")
        {
            // key event
            timelineEventDisplay.bg.color = new Color32(156, 183, 181, 155);
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

    public void setUpPresentInstance(int id)
    {
        int ideez = 999999;
        GameObject newPrefabInstance = Instantiate(TLPrefab);
        newPrefabInstance.transform.SetParent(content.transform, false);
        newPrefabInstance.name = ideez.ToString();

        TimelineEvent thisEvent = newPrefabInstance.GetComponent<TimelineEvent>();
        thisEvent.id = ideez;
        thisEvent.type = 1;
        thisEvent.title = "PRESENT";
        thisEvent.day = GameStateManager.gameStates.currentDay;
        thisEvent.timeOfDay = GameStateManager.gameStates.currentTimeOfDay;
        thisEvent.screenshotPath = null;
        thisEvent.saveDataId = -1;
        thisEvent.isEventStarted = false;
        thisEvent.isEventFinished = false;
        thisEvent.state = null;
        thisEvent.nextEventIds = null;
        thisEvent.lastEventId = id;

        TimelineEventDisplay thisEventDisplay = newPrefabInstance.GetComponent<TimelineEventDisplay>();
        setTimelineDisplay(thisEventDisplay, thisEvent);

        trackRefs.Add(newPrefabInstance);
    }


    public void instantiateTLPrefabs()
    {
        TimelineEvent currentTimelineEvent = GameObject.Find("Persistent Scripts").GetComponent<TimelineEvent>();
        int id = currentTimelineEvent.id;
        setUpPresentInstance(id);

        int count = 0;
        while (id > 0 && count < 200)
        {
            // prevent infinite loop
            count++;

            setUpNewInstance(id);

            id = MakeTL.TL[id].lastEventId;

            
        }
        setUpNewInstance(0);
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
