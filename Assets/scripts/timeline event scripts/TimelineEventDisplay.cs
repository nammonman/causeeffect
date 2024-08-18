using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class TimelineEventDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI eventNameText;
    [SerializeField] TextMeshProUGUI dayNumText;
    [SerializeField] TextMeshProUGUI timeDayText;
    [SerializeField] Image bg;

    private void OnEnable()
    {
        TimelineEvent thisTimelineEvent = gameObject.GetComponent<TimelineEvent>();
        eventNameText.text = thisTimelineEvent.title;
        dayNumText.text = "DAY " + thisTimelineEvent.day.ToString();
        timeDayText.text = thisTimelineEvent.timeOfDay.ToString();
        if (thisTimelineEvent.type == 0)
        {
            // start day
            bg.color = new Color32(85, 122, 138, 255);
        }
        else if (thisTimelineEvent.type == 1)
        {
            // get
            bg.color = new Color32(119, 158, 160, 255);
        }
        else if (thisTimelineEvent.type == 2)
        {
            // key event
            bg.color = new Color32(158, 184, 181, 255);
        }
        else if(thisTimelineEvent.type == 3)
        {
            // present
            bg.color = new Color32(255, 255, 255, 25);
        }
        else
        {
            bg.color = new Color32(158, 184, 181, 255);
        }

    }
}
