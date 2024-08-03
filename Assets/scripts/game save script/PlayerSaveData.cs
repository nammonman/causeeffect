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
        public List<NpcData> npcs;
        public List<int> puzzles;
        public Dictionary<int, int> unlockedItems; // ID, amount
        public List<bool> gameStateBools;
        public List<int> gameStateInts;
        public List<string> gameStateStrings;
        public string sceneName;
        public string sceneSetting;
    }

}
