using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[ExecuteInEditMode]
public class TimelineEventDisplay : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI eventNameText;
    [SerializeField] public TextMeshProUGUI dayNumText;
    [SerializeField] public TextMeshProUGUI timeDayText;
    [SerializeField] public Image bg;
    [SerializeField] public GameObject selectBg;
    [SerializeField] public Button button;
    [SerializeField] public Image overlay1;
    [SerializeField] public Image overlay2;

    public bool isSelected = false;


    private void OnEnable()
    {
        bg = GameObject.Find("TLImage").GetComponent<Image>();
        bg.enabled = false;
        enableDarkOverlay(true);
    }

    private void OnDestroy()
    {
        bg.enabled = false;
    }

    public void setTimelineDisplay(TimelineEvent thisTimelineEvent)
    {
        eventNameText.text = thisTimelineEvent.title;
        dayNumText.text = "DAY " + thisTimelineEvent.day.ToString();
        string[] t = { "MORNING", "AFTERNOON", "EVENING", "NIGHT" };
        timeDayText.text = t[thisTimelineEvent.timeOfDay];
        if (thisTimelineEvent.id == 0)
        {
            // root
            bg.color = new Color32(0, 12, 28, 255);
        }
        else if (thisTimelineEvent.id == 1)
        {
            // get
            bg.color = new Color32(0, 12, 28, 255);
        }
        else if (thisTimelineEvent.name.EndsWith("ENDING"))
        {
            // ending event
            bg.color = new Color32(156, 183, 181, 255);
        }
        else if (thisTimelineEvent.name == "PRESENT")
        {
            // key event
            bg.color = new Color32(156, 183, 181, 155);
        }
    }
    public void LoadAndDisplayImage(string filePath)
    {
        bg.enabled = true;

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Load the image data into a byte array
            byte[] imageData = File.ReadAllBytes(filePath);

            // Create a new Texture2D and load the image data into it
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData); // Automatically resizes the texture

            // Create a Sprite from the texture and assign it to the UI Image component
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            bg.sprite = sprite;
            if (bg.color.a < 1)
            {
                bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 1);
            }
        }
        else
        {
            Debug.Log("File not found at: " + filePath);
        }
    }

    public void enableDarkOverlay(bool b)
    {
        overlay1.enabled = b;
        overlay2.enabled = b;
    }

    public void selectCurrentTimeline()
    {
        if (gameObject.GetComponent<TimelineEvent>().id == GameStateManager.gameStates.currentEventId && !gameObject.GetComponent<TimelineEvent>().name.EndsWith("ENDING"))
        {
            selectTimeline();
            //Debug.Log(gameObject.GetComponent<TimelineEvent>().id);
        }
    }
    public void selectTimeline()
    {
        GameObject[] allTimelines = GameObject.FindGameObjectsWithTag ( "timeline event block" );
        foreach (GameObject t in allTimelines)
        {
            t.GetComponent<TimelineEventDisplay>().deselectTimeline();
            t.GetComponent<TimelineEventDisplay>().enableDarkOverlay(true);
        }
        isSelected = true;
        selectBg.SetActive(true);

        TimelineEvent thisTL = gameObject.GetComponent<TimelineEvent>();
        LoadAndDisplayImage(thisTL.screenshotPath);
        if (thisTL.id == 999999) // present id
        {
            bg.enabled = false;
        }


        int currId = thisTL.id;
        GameObject g = gameObject;
        InGamePauseMenu.selectedGameObject = g;
        while (currId > 0) {
            g.GetComponent<TimelineEventDisplay>().enableDarkOverlay(false);
            currId = g.GetComponent<TimelineEvent>().lastEventId;
            g = GameObject.Find(currId.ToString());
        }
        g.GetComponent<TimelineEventDisplay>().enableDarkOverlay(false);

        
        
    }

    public void deselectTimeline()
    {
        isSelected = false;
        selectBg.SetActive(false);
    }
}

