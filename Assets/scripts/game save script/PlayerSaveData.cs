using Subtegral.DialogueSystem.DataContainers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveGame
{
    [Serializable]
    public class PlayerSaveData
    {
        public int id;
        public Vector3 playerPos;
        public Quaternion playerRot;
        public Vector3 cameraPos;
        public Vector3 cameraRot;
        public List<NpcData> npcs;
        public List<int> puzzles;
        public Dictionary<int, int> unlockedItems; // ID, amount
        public GameStates gameStates;
        public string sceneName;
        public string sceneSetting;
    }

}
