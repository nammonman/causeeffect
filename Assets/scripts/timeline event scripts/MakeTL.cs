using SaveGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MakeTL : MonoBehaviour
{
    public static Dictionary<int, TimelineEvent> TL;
    public static Dictionary<int, PlayerSaveData> PS;

    [SerializeField] TMP_InputField titleInput;
    [SerializeField] TMP_InputField dayNumInput;
    [SerializeField] TMP_InputField timeinput;
    [SerializeField] TextMeshProUGUI idText;

    private void Start()
    {
        // init
        TL = new Dictionary<int, TimelineEvent>();
        PS = new Dictionary<int, PlayerSaveData>();

        // make root event
        newFromCurrentTL();

    }
    public void makePS(int id)
    {
        // init
        PlayerSaveData playerSaveData = new PlayerSaveData();


        // set id
        playerSaveData.id = id;


        // get player position and write to SaveFileData class
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.GetPositionAndRotation(out playerSaveData.playerPos, out playerSaveData.playerRot);

        PS.Add(id, playerSaveData);

    }

    public void newFromCurrentTL()
    {

        TimelineEvent currentTimelineEvent = GameObject.Find("Persistent Scripts").GetComponent<TimelineEvent>();
        int eventId = TL.Count;
        int prevEventId = currentTimelineEvent.id;

        // add next event id to current's list
        if (eventId > 0)
        {
            TL[prevEventId].nextEventIds.Add(eventId);
            Debug.Log("PREV: " + JsonUtility.ToJson(TL[prevEventId]));
        }

        // take screenshot
        string filePath = Application.persistentDataPath + "/" +  eventId.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        ScreenCapture.CaptureScreenshot(filePath);
        currentTimelineEvent.screenshotPath = filePath;

        if (eventId > 0)
        {
            // set properties
            currentTimelineEvent.title = titleInput.text;
            currentTimelineEvent.id = eventId;
            currentTimelineEvent.lastEventId = prevEventId;
            currentTimelineEvent.day = int.Parse(dayNumInput.text);
            currentTimelineEvent.timeOfDay = int.Parse(timeinput.text);
            currentTimelineEvent.saveDataId = eventId;
        }
        

        // save player and npc data (no scenes for now)
        makePS(eventId);

        // add new TL to dict
        TL.Add(eventId, currentTimelineEvent.Clone());
        TL[eventId].nextEventIds.Clear();

        idText.text = currentTimelineEvent.id.ToString();

        Debug.Log("NEW: " + JsonUtility.ToJson(TL[eventId]));
        GameStateManager.gameStates.currentEventId = eventId;
    }
}
