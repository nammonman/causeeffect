using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveGame
{
    [Serializable]
    public class PuzzleData
    {
        public int type;
        public string name;
        public int state;
        public int outcome;
        public bool isCompleted;
    }

}