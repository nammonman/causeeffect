using System;
using System.Collections.Generic;
using UnityEngine;

namespace Subtegral.SceneGraphSystem.DataContainers
{
    [Serializable]
    public class SceneContainer : ScriptableObject
    {
        public List<SceneNodeLinkData> NodeLinks = new List<SceneNodeLinkData>();
        public List<SceneNodeData> SceneNodeData = new List<SceneNodeData>();
        public List<SceneExposedProperty> ExposedProperties = new List<SceneExposedProperty>();
        public List<SceneCommentBlockData> CommentBlockData = new List<SceneCommentBlockData>();
    }
}