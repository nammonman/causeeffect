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
        public MyVec3 playerPos;
        public MyQuat playerRot;
        public MyVec3 cameraPos;
        public MyVec3 cameraRot;
        public List<NpcData> npcs;
        public List<int> puzzles;
        public Dictionary<int, int> unlockedItems; // ID, amount
        //public GameStates gameStates;
        public string sceneName;
        public string sceneSetting;
    }

    public class MyVec3
    {
        public float x;
        public float y;
        public float z;
    }
    public class MyQuat
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }

}
