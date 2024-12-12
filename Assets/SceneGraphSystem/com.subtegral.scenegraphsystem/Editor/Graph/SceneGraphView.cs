using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodiceApp;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Subtegral.SceneGraphSystem.DataContainers;

namespace Subtegral.SceneGraphSystem.Editor
{
    public class SceneGraphView : GraphView
    {
        public readonly Vector2 DefaultNodeSize = new Vector2(300, 200);
        public readonly Vector2 DefaultCommentBlockSize = new Vector2(300, 200);
        public SceneNode EntryPointNode;
        public Blackboard Blackboard = new Blackboard();
        public List<SceneExposedProperty> ExposedProperties { get; private set; } = new List<SceneExposedProperty>();
        private SceneNodeSearchWindow _searchWindow;

        public SceneGraphView(SceneGraph editorWindow)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("SceneGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddElement(GetEntryPointNodeInstance());

            AddSearchWindow(editorWindow);
        }

        private void AddSearchWindow(SceneGraph editorWindow)
        {
            _searchWindow = ScriptableObject.CreateInstance<SceneNodeSearchWindow>();
            _searchWindow.Configure(editorWindow, this);
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }


        public void ClearBlackBoardAndExposedProperties()
        {
            ExposedProperties.Clear();
            Blackboard.Clear();
        }

        public Group CreateCommentBlock(Rect rect, SceneCommentBlockData commentBlockData = null)
        {
            if (commentBlockData == null)
                commentBlockData = new SceneCommentBlockData();
            var group = new Group
            {
                autoUpdateGeometry = true,
                title = commentBlockData.Title
            };
            AddElement(group);
            group.SetPosition(rect);
            return group;
        }

        public void AddPropertyToBlackBoard(SceneExposedProperty property, bool loadMode = false)
        {
            var localPropertyName = property.PropertyName;
            var localPropertyValue = property.PropertyValue;
            if (!loadMode)
            {
                while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
                    localPropertyName = $"{localPropertyName}(1)";
            }

            var item = SceneExposedProperty.CreateInstance();
            item.PropertyName = localPropertyName;
            item.PropertyValue = localPropertyValue;
            ExposedProperties.Add(item);

            var container = new VisualElement();
            var field = new BlackboardField { text = localPropertyName, typeText = "string" };
            container.Add(field);

            var propertyValueTextField = new TextField("Value:")
            {
                value = localPropertyValue
            };
            propertyValueTextField.RegisterValueChangedCallback(evt =>
            {
                var index = ExposedProperties.FindIndex(x => x.PropertyName == item.PropertyName);
                ExposedProperties[index].PropertyValue = evt.newValue;
            });
            var sa = new BlackboardRow(field, propertyValueTextField);
            container.Add(sa);
            Blackboard.Add(container);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            var startPortView = startPort;

            ports.ForEach((port) =>
            {
                var portView = port;
                if (startPortView != portView && startPortView.node != portView.node)
                    compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        public void CreateNewSceneNode(string dialogueTreeName, Vector2 position)
        {
            AddElement(CreateNode("NEW SCENE", "DEFAULT SET", dialogueTreeName, position, false));
        }


        public SceneNode CreateNode(string sceneName, string sceneSettings, string dialogueTreeName, Vector2 position, bool fromSave)
        {
            var tempSceneNode = new SceneNode()
            {
                title = sceneName,
                SceneName = sceneName,
                SceneSettings = sceneSettings,
                DialogueTreeName = dialogueTreeName,
                GUID = Guid.NewGuid().ToString()
            };
            if (fromSave)
            {
                tempSceneNode.isSaved = true;
            }
            tempSceneNode.styleSheets.Add(Resources.Load<StyleSheet>("SceneNode"));
            var inputPort = GetPortInstance(tempSceneNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";


            if (!fromSave)
            {
                inputPort.portColor = Color.red;
            }
            tempSceneNode.inputContainer.Add(inputPort);
            tempSceneNode.RefreshExpandedState();
            tempSceneNode.RefreshPorts();
            tempSceneNode.SetPosition(new Rect(position, DefaultNodeSize));

            // create text fields that update on input
                // text field for scene name and title text
            var titleField = new TextField("scene name");
            titleField.RegisterValueChangedCallback(evt =>
            {
                tempSceneNode.title = evt.newValue;
                tempSceneNode.SceneName = evt.newValue;
                markNodeUnsaved(tempSceneNode);
            });
            titleField.value = sceneName;
            tempSceneNode.mainContainer.Add(titleField);
                
                // text field for scene settings
            var settingsField = new TextField("scene settings");
            settingsField.RegisterValueChangedCallback(evt =>
            {
                tempSceneNode.SceneSettings = evt.newValue;
                markNodeUnsaved(tempSceneNode);
            });
            settingsField.value = sceneSettings;
            tempSceneNode.mainContainer.Add(settingsField);

                // text filed for dialogue tree name
            var textField = new TextField("dialogue tree name");
            textField.RegisterValueChangedCallback(evt =>
            {
                tempSceneNode.DialogueTreeName = evt.newValue;
                markNodeUnsaved(tempSceneNode);
            });
            textField.multiline = true;
            textField.value = dialogueTreeName;
            tempSceneNode.mainContainer.Add(textField);

            var button = new Button(() => { AddChoicePort(tempSceneNode); })
            {
                text = "Add Branch"
            };
            tempSceneNode.titleButtonContainer.Add(button);
            tempSceneNode.contentContainer.style.flexGrow = 1;
            if (!fromSave)
            {
                markNodeUnsaved(tempSceneNode);
            }
            return tempSceneNode;
        }

        public void markNodeUnsaved(SceneNode node)
        {
            if (!node.isSaved)
            {
                return;
            }
            else
            {
                var notSavedText = new TextElement();
                notSavedText.text = "UNSAVED NODE";
                notSavedText.style.color = new StyleColor(Color.red);
                node.contentContainer.Add(notSavedText);
                node.isSaved = false;
            }


        }

        public void markEdgeUnsaved(Port socket)
        {
            var targetEdge = edges.ToList()
                .Where(x => x.output.portName == socket.portName && x.output.node == socket.node);
            if (targetEdge.Any())
            {
                var edge = targetEdge.First();
                edge.edgeControl.edgeColor = Color.red;
            }
        }

        public void AddChoicePort(SceneNode nodeCache, string overriddenPortName = "")
        {
            if (!nodeCache.isSaved)
            {
                markNodeUnsaved(nodeCache);
            }


            var generatedPort = GetPortInstance(nodeCache, Direction.Output);
            generatedPort.style.alignContent = new StyleEnum<Align>(Align.FlexStart);

            var portLabel = generatedPort.contentContainer.Q<Label>("type");
            generatedPort.contentContainer.Remove(portLabel);
            //generatedPort.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            generatedPort.style.flexGrow = 1;

            var outputPortCount = nodeCache.outputContainer.Query("connector").ToList().Count();
            var outputPortName = string.IsNullOrEmpty(overriddenPortName)
                ? $"Choice {outputPortCount + 1}"
                : overriddenPortName;

            var deleteButton = new Button(() => RemovePort(nodeCache, generatedPort))
            {
                text = "X"
            };

            var textField = new TextField()
            {
                name = string.Empty,
                value = overriddenPortName
            };
            textField.RegisterValueChangedCallback(evt => {
                generatedPort.portName = evt.newValue;
                markNodeUnsaved(nodeCache);
            });
            generatedPort.portName = overriddenPortName;
            textField.multiline = true;
            textField.style.width = 150;
            textField.style.whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal);
            //textField.StretchToParentWidth();

            //generatedPort.contentContainer.Add(new Label("choice"));
            generatedPort.contentContainer.Add(textField);
            generatedPort.contentContainer.Add(deleteButton);

            nodeCache.outputContainer.style.flexGrow = 1;
            nodeCache.outputContainer.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            nodeCache.outputContainer.Add(generatedPort);
            /*if (textField.text.Length > 2)
            {
                Color nodeColor = new Color(Convert.ToInt32((byte)textField.text[0]), Convert.ToInt32((byte)textField.text[1]), Convert.ToInt32((byte)textField.text[2]));
                nodeCache.style.borderTopColor = new StyleColor(nodeColor);
            }*/
            nodeCache.RefreshPorts();
            nodeCache.RefreshExpandedState();
        }

        private void RemovePort(Node node, Port socket)
        {
            var targetEdge = edges.ToList()
                .Where(x => x.output.portName == socket.portName && x.output.node == socket.node);
            if (targetEdge.Any())
            {
                var edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());
            }

            node.outputContainer.Remove(socket);
            node.RefreshPorts();
            node.RefreshExpandedState();
        }

        private Port GetPortInstance(SceneNode node, Direction nodeDirection,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            var instantiatedPort = node.InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
            instantiatedPort.portColor = Color.red;
            return instantiatedPort;
        }

        private SceneNode GetEntryPointNodeInstance()
        {
            var nodeCache = new SceneNode()
            {
                title = "START",
                GUID = Guid.NewGuid().ToString(),
                DialogueTreeName = "ENTRYPOINT",
                EntyPoint = true
            };

            var generatedPort = GetPortInstance(nodeCache, Direction.Output);
            generatedPort.portName = "Next";
            nodeCache.outputContainer.Add(generatedPort);

            nodeCache.capabilities &= ~Capabilities.Movable;
            nodeCache.capabilities &= ~Capabilities.Deletable;

            nodeCache.RefreshExpandedState();
            nodeCache.RefreshPorts();
            nodeCache.SetPosition(new Rect(100, 200, 100, 150));
            return nodeCache;
        }
    }
}


