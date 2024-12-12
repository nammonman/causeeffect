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

    }

    private void OnEnable()
    {
        GameStateManager.OnNewTL += newFromCurrentTL;
    }

    private void OnDisable()
    {
        GameStateManager.OnNewTL -= newFromCurrentTL;
    }

    public static Vector3 MyVec3Convert(MyVec3 v3)
    {
        return new Vector3(v3.x, v3.y, v3.z);
    }

    public static Quaternion MyQuatConvert(MyQuat q)
    {
        return new Quaternion(q.x, q.y, q.z, q.w);
    }

    public MyVec3 Vec3Convert(Vector3 v3)
    {
        MyVec3 mv3 = new MyVec3();
        mv3.x = v3.x;
        mv3.y = v3.y;
        mv3.z = v3.z;
        return mv3;
    }

    public MyQuat QuatConvert(Quaternion q)
    {
        MyQuat mq = new MyQuat();
        mq.x = q.x;
        mq.y = q.y;
        mq.z = q.z;
        mq.w = q.w;
        return mq;
    }

    private IEnumerator LoadAndCompressScreenshot(string filePath)
    {
        // Wait until the next frame to ensure the file has been written
        yield return new WaitForSeconds(1);

        // Load the screenshot as a Texture2D
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2); // Create a new Texture2D
        texture.LoadImage(fileData); // Load the image data into the texture

        // Compress to JPG with desired quality (0-100)
        byte[] compressedData = texture.EncodeToJPG(10); // Adjust quality as needed

        // Save compressed data back to PNG format
        File.WriteAllBytes(filePath, compressedData);

        // Optionally, clean up the texture
        Destroy(texture);
    }

    public void makePS(int id)
    {

        // init
        PlayerSaveData playerSaveData = new PlayerSaveData();


        // set id
        playerSaveData.id = id;


        // get player position and write to SaveFileData class
        GameObject player = GameObject.FindGameObjectWithTag("player prefab");
        Vector3 pp;
        Quaternion pr;
        player.transform.GetPositionAndRotation(out pp, out pr);
        playerSaveData.playerPos = Vec3Convert(pp);
        playerSaveData.playerRot = QuatConvert(pr);
        playerSaveData.cameraRot = Vec3Convert(Camera.main.transform.eulerAngles);
        playerSaveData.cameraPos = Vec3Convert(Camera.main.transform.position);

        // get scene name
        playerSaveData.sceneName = GameStateManager.gameStates.CurrentSceneName;

        PS.Add(id, playerSaveData);
        Debug.Log("saved: " + JsonUtility.ToJson(PS[id]));
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
        string filePath = Application.persistentDataPath + "/" +  eventId.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg"; 
        ScreenCapture.CaptureScreenshot(filePath);
        currentTimelineEvent.screenshotPath = filePath;
        StartCoroutine(LoadAndCompressScreenshot(filePath));

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
        else
        {
            currentTimelineEvent.title = "root event";
        }


        // save player and npc data (no scenes for now)
        makePS(eventId);

        // add new TL to dict
        TL.Add(eventId, currentTimelineEvent.Clone());
        TL[eventId].nextEventIds.Clear();

        idText.text = currentTimelineEvent.id.ToString();

        Debug.Log("NEW: " + JsonUtility.ToJson(TL[eventId]));
        GameStateManager.gameStates.currentEventId = eventId;
        GameStateManager.setSave();
    }

    public static void LoadPS(int id)
    {
        GameStateManager.setPausedState(true);
        GameObject player = GameObject.FindGameObjectWithTag("player prefab");
        Transform cam = Camera.main.transform;
        //playermovement pm = player.GetComponent<playermovement>();
        Rigidbody rb = player.GetComponent<Rigidbody>();
        
        player.GetComponent<LoadScene>().LoadSceneByNameAndPos(PS[id].sceneName, MyVec3Convert(PS[id].playerPos));
        playermovement.xRotation = PS[id].cameraRot.x;
        playermovement.yRotation = PS[id].cameraRot.y;
        
        GameStateManager.setPausedState(false);
        Debug.Log("loaded: " + JsonUtility.ToJson(PS[id]));
        
    }

    public static void LoadTL(int id)
    {

        TimelineEvent currentTimelineEvent = GameObject.Find("Persistent Scripts").GetComponent<TimelineEvent>();
        currentTimelineEvent.id = TL[id].id;
        currentTimelineEvent.type = TL[id].type;
        currentTimelineEvent.title = TL[id].title;
        currentTimelineEvent.day = TL[id].day;
        currentTimelineEvent.timeOfDay = TL[id].timeOfDay;
        currentTimelineEvent.screenshotPath = TL[id].screenshotPath;
        currentTimelineEvent.saveDataId = TL[id].saveDataId;
        currentTimelineEvent.isEventStarted = TL[id].isEventStarted;
        currentTimelineEvent.isEventFinished = TL[id].isEventFinished;
        currentTimelineEvent.state = TL[id].state;
        currentTimelineEvent.nextEventIds = TL[id].nextEventIds;
        currentTimelineEvent.lastEventId = TL[id].lastEventId;
        GameStateManager.gameStates.currentEventId = currentTimelineEvent.id;
    }

    public static void LoadTLPS(int id)
    {
        LoadTL(id);
        LoadPS(id);
        GameStateManager.setSave();
    }
}
