using System;
using System.Linq;

namespace Subtegral.SceneGraphSystem.DataContainers
{
    [Serializable]
    public class SceneNodeLinkData
    {
        public string BaseNodeGUID;
        public string PortName;
        public string TargetNodeGUID;
    }
}
