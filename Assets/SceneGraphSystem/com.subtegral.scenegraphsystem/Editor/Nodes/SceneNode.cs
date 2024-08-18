using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Subtegral.SceneGraphSystem.Editor
{
    public class SceneNode : Node
    {
        public string SceneName;
        public string SceneSettings;
        public string DialogueTreeName;
        public string GUID;
        public bool EntyPoint = false;
        public bool isSaved = true;
    }
}