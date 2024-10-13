using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FullTimelinePopulator : MonoBehaviour
{
    [SerializeField] GameObject TLPrefab;
    [SerializeField] GameObject contentRow;
    [SerializeField] GameObject contentColPrefab;

    private Dictionary<int, GameObject> cols = new Dictionary<int, GameObject>();

    private List<GameObject> trackRefs = new List<GameObject>();
    private void OnEnable()
    {
        instantiateTLPrefabsRecursive(0, 0);
        Debug.Log("fullTL on");
    }

    private void OnDisable()
    {
        clearTLPrefabs();
        Debug.Log("fullTL off");
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
    /*public void instantiateTLPrefabsRecursive(int id, int colNum = 0)
    {

        // do stuff

        GameObject newPrefabInstance = Instantiate(TLPrefab);
        newPrefabInstance.transform.SetParent(contentRow.transform, false);
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


        // do next stuff
        foreach (var nextId in MakeTL.TL[id].nextEventIds)
        {
            instantiateTLPrefabsRecursive(nextId, colNum + 1);
        }

        //make col
        if (!cols.ContainsKey(colNum))
        {
            GameObject newCol = Instantiate(contentColPrefab);
            newCol.transform.SetParent(contentRow.transform, false);
            cols.Add(colNum, newCol);
        }

        newPrefabInstance.transform.SetParent(cols[colNum].transform, false);
        
        if (id == GameStateManager.gameStates.currentEventId)
        {
            
            thisEventDisplay.selectTimeline();
        }
    }*/
    public void instantiateTLPrefabsRecursive(int id, int colNum = 0)
    {
        // Ensure column for current colNum is created before processing event prefab
        if (!cols.ContainsKey(colNum))
        {
            GameObject newCol = Instantiate(contentColPrefab);
            newCol.transform.SetParent(contentRow.transform, false);
            cols.Add(colNum, newCol);
        }

        // Instantiate the prefab for the current event
        GameObject newPrefabInstance = Instantiate(TLPrefab);
        newPrefabInstance.transform.SetParent(cols[colNum].transform, false); // Assign to correct column
        newPrefabInstance.name = id.ToString();

        // Set up the event data
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

        // Highlight the current event
        if (id == GameStateManager.gameStates.currentEventId)
        {
            thisEventDisplay.selectTimeline();
        }

        // Recursively instantiate the next events, incrementing the column number
        foreach (var nextId in MakeTL.TL[id].nextEventIds)
        {
            instantiateTLPrefabsRecursive(nextId, colNum + 1);
        }
    }


    public void clearTLPrefabs()
    {
        foreach (GameObject t in trackRefs)
        {
            t.GetComponent<TimelineEventDisplay>().deselectTimeline();
            Destroy(t);
        }
        trackRefs.Clear();

        foreach (var (k, t) in cols)
        {
            Destroy(t);
        }
        cols.Clear();

    }
}
