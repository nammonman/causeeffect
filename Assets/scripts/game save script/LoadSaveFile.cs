using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveGame;
using System.IO;
using Subtegral.SceneGraphSystem.Editor;
using UnityEngine.SceneManagement;


namespace SaveGame 
{
    public class LoadSaveFile : MonoBehaviour
    {
        [SerializeField] GameObject npcPrefab;
        [SerializeField] GameObject playerPrefab;
        string saveFilePath;
        SaveFileData save;
        PlayerSaveData currentPlayerSaveData;
        TimelineEventData currrentTimelineEventData;
        int currentTimelineEventDataId;

        GameObject playerGameObject;
        GameObject npcGameObject;
        
        ItemStorage itemStorage;
        TimelineEvent timelineEvent; 
        NpcDialogue npcDialogue;

        void Start()
        {
            // Initialize save file
            saveFilePath = Application.persistentDataPath + $"/{GameStateManager.saveFileName}.json";
        }

        bool loadSave()
        {
            save = JsonUtility.FromJson<SaveFileData>(File.ReadAllText(saveFilePath));
            currentPlayerSaveData = save.playerSaveDatas[currentTimelineEventDataId];
            currrentTimelineEventData = save.timelineEventDatas[currentTimelineEventDataId];

            StartCoroutine(loadSceneAsync(currentPlayerSaveData.sceneName));
            return true;
        }

        private IEnumerator loadSceneAsync(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }

            placePlayerAndConstructScene(); // place player first because everything is children of player
        }
        bool constructNpcs()
        {
            foreach (var npc in currentPlayerSaveData.npcs)
            {
                // instantiate each npc prefab and set the values for NpcDialogue component
                npcGameObject = Instantiate(npcPrefab, npc.pos, npc.rot);
                npcDialogue = npcGameObject.gameObject.GetComponent<NpcDialogue>();
                if (npc.canInteract)
                {
                    npcDialogue.firstStartNode = npc.firstStartNode;
                    npcDialogue.secondDialogue = npc.secondDialogue;
                    npcDialogue.isFirstInteract = npc.isFirstInteract;
                    npcDialogue.canInteract = npc.canInteract;
                    npcDialogue.firstDialogue = npc.firstDialogue;
                    npcDialogue.secondDialogue= npc.secondDialogue;
                }
            }

            return true;
        }

        bool constructPuzzles()
        {
            foreach (var puzzle in currentPlayerSaveData.puzzles)
            {

            }
            return true;
        }

        bool setGameStates()
        {
            var b = currentPlayerSaveData.gameStateBools;
            var i = currentPlayerSaveData.gameStateInts;
            var s = currentPlayerSaveData.gameStateStrings;

            GameStateManager.isInDialogue = b[0];
            GameStateManager.isPaused = b[1];
            GameStateManager.canLoadNewScene = b[2];
            GameStateManager.canPause = b[3];
            GameStateManager.canPlayerInteract = b[4];
            GameStateManager.canPlayerJump = b[5];
            GameStateManager.canPlayerMove = b[6];
            GameStateManager.canPlayerMoveCamera = b[7];

            GameStateManager.currentEventId = i[0];

            GameStateManager.CurrentSceneName = s[0];
            GameStateManager.saveFileName = s[1];


            return true;
        }

        

        bool placePlayerAndConstructScene()
        {
            // instantiate player prefab
            playerGameObject = Instantiate(playerPrefab, currentPlayerSaveData.playerPos, currentPlayerSaveData.playerRot);
            itemStorage = playerGameObject.GetComponent<ItemStorage>();
            timelineEvent = playerGameObject.GetComponent<TimelineEvent>();

            setGameStates();
            constructNpcs();
            constructPuzzles();
            setItems();
            setTimelineEvent();
            return true;
        }

        bool setItems()
        {
            foreach (var (amount, item) in currentPlayerSaveData.unlockedItems)
            {

            }
            return true;
        }
        bool setTimelineEvent()
        {
            timelineEvent.type = currrentTimelineEventData.type;
            timelineEvent.title = currrentTimelineEventData.title;
            timelineEvent.day = currrentTimelineEventData.day;
            timelineEvent.timeOfDay = currrentTimelineEventData.timeOfDay;
            timelineEvent.description = currrentTimelineEventData.description;
            timelineEvent.lastEventId = currrentTimelineEventData.lastEventId;
            timelineEvent.isEventStarted = currrentTimelineEventData.isEventStarted;
            timelineEvent.isEventFinished = currrentTimelineEventData.isEventFinished;
            timelineEvent.state = currrentTimelineEventData.state;
            timelineEvent.screenShotPath = currrentTimelineEventData.screenshotPath;
            return true; 
        }
    }

}

