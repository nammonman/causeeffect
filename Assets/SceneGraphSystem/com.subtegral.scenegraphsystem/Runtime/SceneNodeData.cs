using System;
using System.Collections.Generic;
using UnityEngine;

namespace Subtegral.SceneGraphSystem.DataContainers
{
    [Serializable]
    public class SceneNodeData
    {
        public string NodeGUID;
        public string SceneName;
        public string SceneSettings;
        public string DialogueTreeName;
        //public List<string> choiceTexts;
        public Vector2 Position;
    }
}