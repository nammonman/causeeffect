using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveGame;
using Unity.VisualScripting;
using System.IO;

namespace SaveGame
{
    public class SaveFileData
    {
        public Dictionary<int, PlayerSaveData> saveDatas;
        public Dictionary<int, TimelineEventData> timelineEvents;
        public int lastEventId;
    }

    public class SaveFile : MonoBehaviour
    {
        string saveFilePath;
        SaveFileData save;
        
        void Start()
        {
            // Initialize save file
            saveFilePath = Application.persistentDataPath + $"/{GameStateManager.saveFileName}.json";
            save.lastEventId = GameStateManager.currentEventId;
        }

        public void saveButton()
        {
            writeSaveFile();
        }

        bool writeSaveFile()
        {
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
                return false;
            }

        }

        bool getSaveDatas()
        {
            // go through all GameObjects with npc tag and save data in a list
            // if possible: check with the scene if the npc found is the same with what the scene is supposed to have
            // throw up a warning if check did not pass
            // get all data for Npc class and write to SaveFileData class

            // go through all GameObjects with puzzle tag and save data in a list
            // if possible: check with the scene if the puzzle found is the same with what the scene is supposed to have
            // throw up a warning if check did not pass
            // get all data for Puzzle class and write to SaveFileData class


            // get player position and write to SaveFileData class

            // get item data and write to SaveFileData class

            // get game states and write to SaveFileData class

            // get scene data and write to SaveFileData class

            // get dialogue and write to SaveFileData class

            // if successful
            return true;
        }

        bool getTimelineEvents()
        {
            // if successful
            return true;
        }

    }

}
