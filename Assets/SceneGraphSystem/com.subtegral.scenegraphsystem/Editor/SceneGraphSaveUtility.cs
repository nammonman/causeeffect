using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Subtegral.SceneGraphSystem.DataContainers;
using UnityEngine.UIElements;

namespace Subtegral.SceneGraphSystem.Editor
{
    public class SceneGraphSaveUtility
    {
        private List<Edge> Edges => _graphView.edges.ToList();
        private List<SceneNode> Nodes => _graphView.nodes.ToList().Cast<SceneNode>().ToList();

        private List<Group> CommentBlocks =>
            _graphView.graphElements.ToList().Where(x => x is Group).Cast<Group>().ToList();

        private SceneContainer _dialogueContainer;
        private SceneGraphView _graphView;

        public static SceneGraphSaveUtility GetInstance(SceneGraphView graphView)
        {
            return new SceneGraphSaveUtility
            {
                _graphView = graphView
            };
        }

        public void SaveGraph(string fileName)
        {
            var dialogueContainerObject = ScriptableObject.CreateInstance<SceneContainer>();
            if (!SaveNodes(fileName, dialogueContainerObject)) return;
            SaveExposedProperties(dialogueContainerObject);
            SaveCommentBlocks(dialogueContainerObject);

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");

            UnityEngine.Object loadedAsset = AssetDatabase.LoadAssetAtPath($"Assets/Resources/{fileName}.asset", typeof(SceneContainer));

            if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset))
            {
                AssetDatabase.CreateAsset(dialogueContainerObject, $"Assets/Resources/{fileName}.asset");
            }
            else
            {
                SceneContainer container = loadedAsset as SceneContainer;
                container.NodeLinks = dialogueContainerObject.NodeLinks;
                container.SceneNodeData = dialogueContainerObject.SceneNodeData;
                container.ExposedProperties = dialogueContainerObject.ExposedProperties;
                container.CommentBlockData = dialogueContainerObject.CommentBlockData;
                EditorUtility.SetDirty(container);
            }

            AssetDatabase.SaveAssets();
        }

        private bool SaveNodes(string fileName, SceneContainer dialogueContainerObject)
        {
            if (!Edges.Any()) return false;

            var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();

            for (var i = 0; i < connectedSockets.Count(); i++)
            {
                var outputNode = (connectedSockets[i].output.node as SceneNode);
                var inputNode = (connectedSockets[i].input.node as SceneNode);
                dialogueContainerObject.NodeLinks.Add(new SceneNodeLinkData
                {
                    BaseNodeGUID = outputNode.GUID,
                    PortName = connectedSockets[i].output.portName,
                    TargetNodeGUID = inputNode.GUID
                });
                if (connectedSockets[i].output.portName == "Single")
                {
                    Debug.Log($"connectedSockets {i} lost data");
                }

            }

            foreach (var node in Nodes.Where(node => !node.EntyPoint))
            {
                dialogueContainerObject.SceneNodeData.Add(new SceneNodeData
                {
                    NodeGUID = node.GUID,
                    SceneName = node.SceneName,
                    SceneSettings = node.SceneSettings,
                    DialogueTreeName = node.DialogueTreeName,
                    Position = node.GetPosition().position
                });

            }

            return true;
        }

        private void SaveExposedProperties(SceneContainer dialogueContainer)
        {
            dialogueContainer.ExposedProperties.Clear();
            dialogueContainer.ExposedProperties.AddRange(_graphView.ExposedProperties);
        }

        private void SaveCommentBlocks(SceneContainer dialogueContainer)
        {
            foreach (var block in CommentBlocks)
            {
                var nodes = block.containedElements.Where(x => x is SceneNode).Cast<SceneNode>().Select(x => x.GUID)
                    .ToList();

                dialogueContainer.CommentBlockData.Add(new SceneCommentBlockData
                {
                    ChildNodes = nodes,
                    Title = block.title,
                    Position = block.GetPosition().position
                });
            }
        }

        public void LoadGraph(string fileName)
        {
            _dialogueContainer = Resources.Load<SceneContainer>(fileName);
            if (_dialogueContainer == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target Narrative Data does not exist!", "OK");
                return;
            }

            ClearGraph();
            GenerateSceneNodes();
            ConnectSceneNodes();
            AddExposedProperties();
            GenerateCommentBlocks();
        }

        /// <summary>
        /// Set Entry point GUID then Get All Nodes, remove all and their edges. Leave only the entrypoint node. (Remove its edge too)
        /// </summary>
        private void ClearGraph()
        {
            Nodes.Find(x => x.EntyPoint).GUID = _dialogueContainer.NodeLinks[0].BaseNodeGUID;
            foreach (var perNode in Nodes)
            {
                if (perNode.EntyPoint) continue;
                Edges.Where(x => x.input.node == perNode).ToList()
                    .ForEach(edge => _graphView.RemoveElement(edge));
                _graphView.RemoveElement(perNode);
            }
        }

        /// <summary>
        /// Create All serialized nodes and assign their guid and dialogue text to them
        /// </summary>
        private void GenerateSceneNodes()
        {
            foreach (var perNode in _dialogueContainer.SceneNodeData)
            {
                var tempNode = _graphView.CreateNode(perNode.SceneName, perNode.SceneSettings, perNode.DialogueTreeName, Vector2.zero, true);
                tempNode.GUID = perNode.NodeGUID;
                tempNode.SceneName = perNode.SceneName;
                tempNode.SceneSettings = perNode.SceneSettings;
                tempNode.DialogueTreeName = perNode.DialogueTreeName;
                
                _graphView.AddElement(tempNode);

                var nodePorts = _dialogueContainer.NodeLinks.Where(x => x.BaseNodeGUID == perNode.NodeGUID).ToList();
                nodePorts.ForEach(x => _graphView.AddChoicePort(tempNode, x.PortName));
            }
        }

        private void ConnectSceneNodes()
        {
            for (var i = 0; i < Nodes.Count; i++)
            {
                var k = i; //Prevent access to modified closure
                var connections = _dialogueContainer.NodeLinks.Where(x => x.BaseNodeGUID == Nodes[k].GUID).ToList();
                for (var j = 0; j < connections.Count(); j++)
                {
                    var targetNodeGUID = connections[j].TargetNodeGUID;
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGUID);
                    LinkNodesTogether(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(
                        _dialogueContainer.SceneNodeData.First(x => x.NodeGUID == targetNodeGUID).Position,
                        _graphView.DefaultNodeSize));
                }
            }
        }

        private void LinkNodesTogether(Port outputSocket, Port inputSocket)
        {
            var tempEdge = new Edge()
            {
                output = outputSocket,
                input = inputSocket
            };
            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            outputSocket.portColor = Color.cyan;
            inputSocket.portColor = Color.cyan;
            _graphView.Add(tempEdge);
        }

        private void AddExposedProperties()
        {
            _graphView.ClearBlackBoardAndExposedProperties();
            foreach (var exposedProperty in _dialogueContainer.ExposedProperties)
            {
                _graphView.AddPropertyToBlackBoard(exposedProperty);
            }
        }

        private void GenerateCommentBlocks()
        {
            foreach (var commentBlock in CommentBlocks)
            {
                _graphView.RemoveElement(commentBlock);
            }

            foreach (var commentBlockData in _dialogueContainer.CommentBlockData)
            {
                var block = _graphView.CreateCommentBlock(new Rect(commentBlockData.Position, _graphView.DefaultCommentBlockSize),
                     commentBlockData);
                block.AddElements(Nodes.Where(x => commentBlockData.ChildNodes.Contains(x.GUID)));
            }
        }
    }
}