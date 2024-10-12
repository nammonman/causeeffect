using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveGame;
using Unity.VisualScripting;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine.SceneManagement;

namespace SaveGame
{
    public class SaveFileData
    {
        public Dictionary<int, PlayerSaveData> playerSaveDatas;
        public Dictionary<int, TimelineEvent> timelineEventDatas;
        public int currentTimelineEventDataId;
    }

    public class SaveFile : MonoBehaviour
    {
        int saveId;
        string saveFilePath;
        SaveFileData save;

        Dictionary<int, PlayerSaveData> playerSaveDatas;
        Dictionary<int, TimelineEvent> timelineEventDatas;

        void Start()
        {
            // Initialize save file
            saveFilePath = Application.persistentDataPath + $"/{GameStateManager.gameStates.saveFileName}.json";
            playerSaveDatas = new Dictionary<int, PlayerSaveData>();
            timelineEventDatas = new Dictionary<int, TimelineEvent>();
        }

        public static IEnumerator delaySave() { yield return new WaitForSeconds(5); }


        public void saveButton()
        {
            writeSaveFile();
        }


        bool writeSaveFile()
        {
            StartCoroutine(delaySave());
            saveId = GameStateManager.gameStates.currentEventId;

            if (File.Exists(saveFilePath))
            {
                save = JsonUtility.FromJson<SaveFileData>(File.ReadAllText(saveFilePath));
            }

            PlayerSaveData p = getCurrentPlayerSaveData();
            TimelineEvent t = getCurrentTimelineEventData();

            if (save.timelineEventDatas[saveId] != null) // check if duplicate
            {
                save.playerSaveDatas.Add(saveId, p);
                save.timelineEventDatas.Add(saveId, t);
            }
            else
            {
                save.playerSaveDatas[saveId] = p;
            }

            string JSONData = JsonUtility.ToJson(save);
            try
            {
                File.WriteAllText(saveFilePath, JSONData);
                Debug.Log($"file saved at {saveFilePath}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.Log(e.ToString());
            }

            return false;
        }

        PlayerSaveData getCurrentPlayerSaveData()
        {
            // init
            PlayerSaveData playerSaveData = new PlayerSaveData();
            

            // set id
            playerSaveData.id = saveId;


            // get player position and write to SaveFileData class
            GameObject player = GameObject.FindGameObjectWithTag("player");
            player.transform.GetPositionAndRotation(out playerSaveData.playerPos, out playerSaveData.playerRot);


            // go through all GameObjects with npc tag and save data in a list
            // get all data for Npc class and write to SaveFileData class
            List<NpcData> npcDatas = new List<NpcData>();
            GameObject[] npcs = GameObject.FindGameObjectsWithTag("npc");
            for (int i = 0; i < npcs.Length; i++)
            {
                npcs[i].transform.GetPositionAndRotation(out npcDatas[i].pos, out npcDatas[i].rot);
                npcDatas[i].name = npcs[i].name;
                NpcDialogue npcDialogue = npcs[i].GetComponent<NpcDialogue>();
                npcDatas[i].isFirstInteract = npcDialogue.isFirstInteract;
                npcDatas[i].canInteract = npcDialogue.canInteract;
                npcDatas[i].firstStartNode = npcDialogue.firstStartNode;
                npcDatas[i].secondStartNode = npcDialogue.secondStartNode;
                npcDatas[i].firstDialogue = npcDialogue.firstDialogue;
                npcDatas[i].secondDialogue = npcDialogue.secondDialogue;
            }
            playerSaveData.npcs = npcDatas;


            // go through all GameObjects with puzzle tag and save data in a list
            // get all data for Puzzle class and write to SaveFileData class
            List<PuzzleData> puzzleDatas = new List<PuzzleData>();
            GameObject[] puzzles = GameObject.FindGameObjectsWithTag("puzzle");
            for (int i = 0; i < puzzles.Length; i++)
            {
                break;
            }
            /*playerSaveData.puzzles = */

            // get item data and write to SaveFileData class
            Dictionary<int, int> unlockedItems = new Dictionary<int, int>();
            ItemStorage itemStorage = GameObject.FindGameObjectWithTag("item storage").GetComponent<ItemStorage>();
            for (int i = 0;i < 0; i++)
            {
                break;
            }
            /*playerSaveData.items = */

            // get game states and write to SaveFileData class
            playerSaveData.gameStates = GameStateManager.gameStates;


            // get scene data and write to SaveFileData class
            playerSaveData.sceneName = GameStateManager.gameStates.CurrentSceneName;
            playerSaveData.sceneSetting = GameStateManager.gameStates.CurrentSceneSetting;


            return playerSaveData;
        }

        TimelineEvent getCurrentTimelineEventData()
        {
            // init
            TimelineEvent timelineEventData = new TimelineEvent();

            // get current timeline event
            TimelineEvent currentTimelineEvent = GameObject.Find("Persistent Scripts").GetComponent<TimelineEvent>();
            timelineEventData = currentTimelineEvent;

            // set id
            timelineEventData.id = saveId;

            //screenshot
            string filename = timelineEventData.id.ToString() + "_" + timelineEventData.title + "_" + timelineEventData.day.ToString();
            ScreenCapture.CaptureScreenshot(filename);
            timelineEventData.screenshotPath = Application.persistentDataPath + $"/{filename}.png";

            return timelineEventData;
        }

    }

}
