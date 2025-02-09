using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveGame;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

namespace SaveGame
{
    public class SaveFileData
    {
        public Dictionary<int, PlayerSaveData> playerSaveDatas;
        public Dictionary<int, SerializableTimelineEvent> timelineEventDatas;
        public int currentTimelineEventId;
        public int fixLevel;
        public int causeEffectPower;
        public List<string> globalFlags;
        public List<string> completedZF;
    }

    public class SaveFile : MonoBehaviour
    {
        string saveFilePath;
        SaveFileData save;

        public static bool saveFileExists;

        Dictionary<int, PlayerSaveData> playerSaveDatas;
        Dictionary<int, SerializableTimelineEvent> timelineEventDatas;

        void Start()
        {
            // Initialize save file
            saveFilePath = Application.persistentDataPath + $"/{GameStateManager.gameStates.saveFileName}.json";
            playerSaveDatas = new Dictionary<int, PlayerSaveData>();
            timelineEventDatas = new Dictionary<int, SerializableTimelineEvent>();
        }

        private float timer = 0f; 
        private const float interval = 60f; 
        private void Update()
        {
            // auto save
            timer += Time.deltaTime; 

            if (timer >= interval)
            {
                if (GameStateManager.gameStates.currentEventId > 0 && !(SceneManager.GetSceneByName("MainMenu").isLoaded || SceneManager.GetSceneByName("player scene").isLoaded))
                {
                    writeSaveFile(); 
                }
                timer = 0f; 
            }
        }

        private void OnEnable()
        {
            GameStateManager.OnSave += writeSaveFile;
        }

        private void OnDisable()
        {
            GameStateManager.OnSave -= writeSaveFile;
        }

        public static IEnumerator delaySave() { yield return new WaitForSeconds(5); }


        public void saveButton()
        {
            writeSaveFile();
        }


        public void writeSaveFile()
        {
            GameStateManager.gameStates.isSaving = true;
            save = new SaveFileData();
            

            save.currentTimelineEventId = GameStateManager.gameStates.currentEventId;
            save.fixLevel = GameStateManager.gameStates.fixLevel;
            save.causeEffectPower = GameStateManager.gameStates.causeEffectPower;
            if (GameStateManager.gameStates.globalFlags != null)
            {
                save.globalFlags = new List<string>();
                foreach (var item in GameStateManager.gameStates.globalFlags)
                {
                    save.globalFlags.Add(item);
                }
            }

            if (GameStateManager.gameStates.completedZF != null)
            {
                save.completedZF = new List<string>();
                foreach (var item in GameStateManager.gameStates.completedZF)
                {
                    save.completedZF.Add(item);
                }
            }
            
            save.playerSaveDatas = MakeTL.PS;

            save.timelineEventDatas = new Dictionary<int, SerializableTimelineEvent>();
            foreach (int k in MakeTL.TL.Keys) {
                SerializableTimelineEvent sv = new SerializableTimelineEvent();
                sv.id = MakeTL.TL[k].id;
                sv.type = MakeTL.TL[k].type;
                sv.title = MakeTL.TL[k].title;
                sv.day = MakeTL.TL[k].day;
                sv.timeOfDay = MakeTL.TL[k].timeOfDay;
                sv.screenshotPath = MakeTL.TL[k].screenshotPath;
                sv.saveDataId = MakeTL.TL[k].saveDataId;
                sv.isEventStarted = MakeTL.TL[k].isEventStarted;
                sv.isEventFinished = MakeTL.TL[k].isEventFinished;
                sv.state = MakeTL.TL[k].state;
                sv.nextEventIds = MakeTL.TL[k].nextEventIds;
                sv.lastEventId = MakeTL.TL[k].lastEventId;
                save.timelineEventDatas.Add(k, sv);
            }

            string JSONData = JsonConvert.SerializeObject(save, Formatting.Indented);
            try
            {
                File.WriteAllText(saveFilePath, JSONData);
                Debug.Log($"file saved at {saveFilePath}");
            }
            catch (System.Exception e)
            {
                Debug.Log(e.ToString());
            }
            GameStateManager.gameStates.isSaving = false;
        }

        public void loadSaveFile()
        {
            if (File.Exists(saveFilePath))
            {
                string JSONData = File.ReadAllText(saveFilePath);
                save = JsonConvert.DeserializeObject<SaveFileData>(JSONData);
                GameStateManager.gameStates.currentEventId = save.currentTimelineEventId;
                GameStateManager.gameStates.fixLevel = save.fixLevel;
                GameStateManager.gameStates.causeEffectPower = save.causeEffectPower;
                save.fixLevel = GameStateManager.gameStates.fixLevel;
                if (save.globalFlags != null)
                {
                    foreach (var item in save.globalFlags)
                    {
                        GameStateManager.gameStates.globalFlags.Add(item);
                    }
                }

                if (save.completedZF != null)
                {
                    foreach (var item in save.completedZF)
                    {
                        GameStateManager.gameStates.completedZF.Add(item);
                    }
                }

                MakeTL.PS.Clear();
                MakeTL.TL.Clear();

                foreach (int k in save.timelineEventDatas.Keys)
                {
                    TimelineEvent v = new TimelineEvent();
                    v.id = save.timelineEventDatas[k].id;
                    v.type = save.timelineEventDatas[k].type;
                    v.title = save.timelineEventDatas[k].title;
                    v.day = save.timelineEventDatas[k].day;
                    v.timeOfDay = save.timelineEventDatas[k].timeOfDay;
                    v.screenshotPath = save.timelineEventDatas[k].screenshotPath;
                    v.saveDataId = save.timelineEventDatas[k].saveDataId;
                    v.isEventStarted = save.timelineEventDatas[k].isEventStarted;
                    v.isEventFinished = save.timelineEventDatas[k].isEventFinished;
                    v.state = save.timelineEventDatas[k].state;
                    v.nextEventIds = save.timelineEventDatas[k].nextEventIds;
                    v.lastEventId = save.timelineEventDatas[k].lastEventId;
                    MakeTL.TL.Add(k, v);
                }
                
                MakeTL.PS = save.playerSaveDatas;
                MakeTL.LoadTLPS(save.currentTimelineEventId);
            }
            else
            {
                

            }
        }


        public void NewGameProcedure()
        {
            MakeTL.TL = new Dictionary<int, TimelineEvent>();
            MakeTL.PS = new Dictionary<int, PlayerSaveData>();
            TimelineEvent currentTimelineEvent = GameObject.Find("Persistent Scripts").GetComponent<TimelineEvent>();
            currentTimelineEvent.id = 0;
            currentTimelineEvent.type = 0;
            currentTimelineEvent.title = "";
            currentTimelineEvent.day = 0;
            currentTimelineEvent.timeOfDay = 0;
            currentTimelineEvent.screenshotPath = "";
            currentTimelineEvent.saveDataId = 0;
            currentTimelineEvent.isEventStarted = false;
            currentTimelineEvent.isEventFinished = false;
            currentTimelineEvent.state = "";
            currentTimelineEvent.nextEventIds = new List<int>();
            currentTimelineEvent.lastEventId = 0;
            GameStateManager.gameStates.currentEventId = 0;
            GameStateManager.gameStates.fixLevel = 0;
            GameStateManager.gameStates.causeEffectPower = 10;
            GameStateManager.setDateTime(7, 2);
            if (GameStateManager.gameStates.completedZF == null)
            {
                GameStateManager.gameStates.completedZF = new List<string>();
            }
            GameStateManager.gameStates.completedZF.Clear();
            if (GameStateManager.gameStates.globalFlags == null)
            {
                GameStateManager.gameStates.globalFlags = new List<string>();
            }
            GameStateManager.gameStates.globalFlags.Clear();
            GameStateManager.gameStates.globalFlags.Add("RECIPE_Overheat");
            GameStateManager.gameStates.globalFlags.Add("RECIPE_Explosive");
            GameStateManager.gameStates.globalFlags.Add("RECIPE_Corrosion");
            GameStateManager.gameStates.globalFlags.Add("RECIPE_Overheat");
            GameStateManager.gameStates.globalFlags.Add("RECIPE_RadiationPoisoning");
            StartCoroutine(DeleteAllFilesInPersistentDataPath());
        }

        public IEnumerator DeleteAllFilesInPersistentDataPath()
        {
            string startingSceneName = "presentation tutorial";
            string path = Application.persistentDataPath;

            // Check if the directory exists
            if (Directory.Exists(path))
            {
                // Get all files in the directory
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    if (file.EndsWith(".jpg") || file == "slot1.json")
                    {
                        File.Delete(file);
                        Debug.Log("Deleted file: " + file);
                    }
                }

                Debug.Log("All save files in persistent data path deleted.");
            }
            else
            {
                Debug.LogWarning("Persistent data path does not exist.");
            }

            // Load the new scene asynchronously
            GameObject player = GameObject.FindGameObjectWithTag("player prefab");
            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            GameStateManager.setPausedState(true);
            GameStateManager.gameStates.canPause = false;
            GameStateManager.setLoadNewScene(startingSceneName);


            // Now the scene is fully loaded, we can call the next line
            while (GameStateManager.gameStates.CurrentSceneName == "MainMenu")
            {
                yield return null;
            }
            GameStateManager.setNewTL();

            yield return null; 
        }

    }  

    

}
