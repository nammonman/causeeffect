using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveGame
{
    [Serializable]
    public class TimelineEventData
    {
        public int id;
        public int type;
        public string title;
        public int day;
        public int timeOfDay;
        public string description;
        public string screenshotPath;
        public int saveDataId;
        
        // for full timeline display
        public List<int> nextEventIds;

        // for current timeline display
        // only need previous event because you can't time travel in to the future
        public int lastEventId;
    }

}
