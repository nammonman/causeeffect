using System;
using System.Collections.Generic;
using UnityEngine;

namespace Subtegral.SceneGraphSystem.DataContainers
{
    [Serializable]
    public class SceneCommentBlockData
    {
        public List<string> ChildNodes = new List<string>();
        public Vector2 Position;
        public string Title = "Comment Block";
    }
}
