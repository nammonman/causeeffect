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

    public bool isSelected = false;

    private void OnEnable()
    {
        deselectTimeline();

        bg = GameObject.Find("TLImage").GetComponent<Image>();
        bg.enabled = false;
        
    }

    private void OnDestroy()
    {
        bg.enabled = false;
    }

    public void setTimelineDisplay(TimelineEvent thisTimelineEvent)
    {
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
        else if (thisTimelineEvent.type == 3)
        {
            // present
            bg.color = new Color32(255, 255, 255, 25);
        }
        else
        {
            bg.color = new Color32(158, 184, 181, 255);
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
            Debug.LogError("File not found at: " + filePath);
        }
    }
    public void selectTimeline()
    {
        GameObject[] allTimelines = GameObject.FindGameObjectsWithTag ( "timeline event block" );
        foreach (GameObject t in allTimelines)
        {
            t.GetComponent<TimelineEventDisplay>().deselectTimeline();
        }
        isSelected = true;
        selectBg.SetActive(true);

        TimelineEvent thisTL = gameObject.GetComponent<TimelineEvent>();
        LoadAndDisplayImage(thisTL.screenshotPath);
        if (thisTL.id == GameStateManager.gameStates.currentEventId)
        {
            bg.enabled = false;
        }
    }

    public void deselectTimeline()
    {
        isSelected = false;
        selectBg.SetActive(false);
    }
}

