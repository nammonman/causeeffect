using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodiceApp;
using Subtegral.DialogueSystem.DataContainers;
using Subtegral.DialogueSystem.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using Button = UnityEngine.UIElements.Button;


namespace Subtegral.DialogueSystem.Editor
{
    public class StoryGraphView : GraphView
    {
        public readonly Vector2 DefaultNodeSize = new Vector2(300, 200);
        public readonly Vector2 DefaultCommentBlockSize = new Vector2(300, 200);
        public DialogueNode EntryPointNode;
        public Blackboard Blackboard = new Blackboard();
        public List<ExposedProperty> ExposedProperties { get; private set; } = new List<ExposedProperty>();
        private NodeSearchWindow _searchWindow;

        public StoryGraphView(StoryGraph editorWindow)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("NarrativeGraph"));
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

            graphViewChanged = OnGraphViewChanged;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            if (change.elementsToRemove != null)
            {
                foreach (var element in change.elementsToRemove)
                {
                    if (element is Edge edge)
                    {
                        OnEdgeDisconnected(edge);
                    }
                }
            }

            return change;
        }

        private void AddSearchWindow(StoryGraph editorWindow)
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            _searchWindow.Configure(editorWindow, this);
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }


        public void ClearBlackBoardAndExposedProperties()
        {
            ExposedProperties.Clear();
            Blackboard.Clear();
        }

        public Group CreateCommentBlock(Rect rect, CommentBlockData commentBlockData = null)
        {
            if(commentBlockData==null)
                commentBlockData = new CommentBlockData();
            var group = new Group
            {
                autoUpdateGeometry = true,
                title = commentBlockData.Title
            };
            AddElement(group);
            group.SetPosition(rect);
            return group;
        }

        public void AddPropertyToBlackBoard(ExposedProperty property, bool loadMode = false)
        {
            var localPropertyName = property.PropertyName;
            var localPropertyValue = property.PropertyValue;
            if (!loadMode)
            {
                while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
                    localPropertyName = $"{localPropertyName}(1)";
            }

            var item = ExposedProperty.CreateInstance();
            item.PropertyName = localPropertyName;
            item.PropertyValue = localPropertyValue;
            ExposedProperties.Add(item);

            var container = new VisualElement();
            var field = new BlackboardField {text = localPropertyName, typeText = "string"};
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
                if (startPortView != portView && startPortView.node != portView.node && !portView.connected)
                    compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        public void OnEdgeDisconnected(Edge edge)
        {
            if (edge?.input?.node is DialogueNode inputNode)
            {
                
                markNodeUnsaved(inputNode);
                edge.input.portColor = Color.red;
                inputNode.RefreshPorts();
                inputNode.RefreshExpandedState();
            }

            if (edge?.output?.node is DialogueNode outputNode)
            {
                
                markNodeUnsaved(outputNode);
                edge.output.portColor = Color.red;
                outputNode.RefreshPorts();
                outputNode.RefreshExpandedState();
            }
        }

        public void CreateNewDialogueNode(string nodeName, Vector2 position)
        {
            AddElement(CreateNode("NEW NPC", nodeName, new List<string> { }, position, false));
        }

        
        public DialogueNode CreateNode(string NPCName, string nodeName, List<string> trigger, Vector2 position, bool fromSave)
        {
            var tempDialogueNode = new DialogueNode()
            {
                title = NPCName,
                NPCNameText = NPCName,
                DialogueText = nodeName,
                GUID = Guid.NewGuid().ToString()
            };
            if (fromSave)
            {
                tempDialogueNode.isSaved = true;
            }
            tempDialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));
            var inputPort = GetPortInstance(tempDialogueNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";

            
            if (!fromSave)
            {
                inputPort.portColor = Color.red;
            }
            tempDialogueNode.inputContainer.Add(inputPort);
            tempDialogueNode.RefreshExpandedState();
            tempDialogueNode.RefreshPorts();
            tempDialogueNode.SetPosition(new Rect(position,
                DefaultNodeSize)); //To-Do: implement screen center instantiation positioning

            var titleField = new TextField("npc name");
            titleField.RegisterValueChangedCallback(evt =>
            {
                titleField.label = "npc name*";
                tempDialogueNode.title = evt.newValue;
                tempDialogueNode.NPCNameText = evt.newValue;
                markNodeUnsaved(tempDialogueNode);
            });
            titleField.RegisterCallback<InputEvent>(evt =>
            {
                titleField.label = "npc name*";
                tempDialogueNode.title = titleField.value;
                tempDialogueNode.NPCNameText = titleField.value;
                markNodeUnsaved(tempDialogueNode);
            });
            titleField.value = NPCName;
            tempDialogueNode.mainContainer.Add(titleField);

            var textField = new TextField("dialogue");
            textField.RegisterValueChangedCallback(evt =>
            {
                textField.label = "dialogue*";
                tempDialogueNode.DialogueText = evt.newValue;
                markNodeUnsaved(tempDialogueNode);
            });
            textField.RegisterCallback<InputEvent>(evt =>
            {
                textField.label = "dialogue*";
                tempDialogueNode.DialogueText = textField.value;
                markNodeUnsaved(tempDialogueNode);
            });
            textField.multiline = true;
            textField.value = nodeName;
            tempDialogueNode.mainContainer.Add(textField);

            var triggerButton = new Button(() => { AddTrigger(tempDialogueNode); })
            {
                text = "Add Trigger"
            };
            tempDialogueNode.mainContainer.Add(triggerButton);
            if (fromSave && trigger != null && trigger.Count > 0)
            {
                for (int i = 0; i < trigger.Count; i++)
                {
                    AddTrigger(tempDialogueNode, trigger[i], i, fromSave);
                }
            }


            var button = new Button(() => { AddChoicePort(tempDialogueNode); })
            {
                text = "Add Choice"
            };
            tempDialogueNode.titleButtonContainer.Add(button);
            tempDialogueNode.contentContainer.style.flexGrow = 1;
            if (!fromSave)
            {
                markNodeUnsaved(tempDialogueNode);
            }
            return tempDialogueNode;
        }

        public void markNodeUnsaved(DialogueNode node)
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

        public void AddChoicePort(DialogueNode nodeCache, string overriddenPortName = "")
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
            generatedPort.contentContainer.style.height = 50;
            generatedPort.contentContainer.style.paddingTop = 10;
            generatedPort.contentContainer.style.paddingBottom = 10;
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
            textField.RegisterCallback<InputEvent>(evt =>
            {
                generatedPort.portName = textField.value;
                markNodeUnsaved(nodeCache);
            });
            generatedPort.portName = overriddenPortName;
            textField.multiline = true;
            textField.style.width = 300;
            textField.style.height = 50;
            textField.style.whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal);

            generatedPort.contentContainer.Add(textField);
            generatedPort.contentContainer.Add(deleteButton);

            nodeCache.outputContainer.style.flexGrow = 1;
            nodeCache.outputContainer.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            nodeCache.outputContainer.Add(generatedPort);
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

        private void AddTrigger(DialogueNode nodeCache, string trigger = "", int index = -1, bool fromSave = false)
        {
            // Ensure trigger list is initialized
            if (nodeCache.Trigger == null)
            {
                nodeCache.Trigger = new List<string>();
            }

            // Create a container for the trigger field and buttons
            var triggerContainer = new VisualElement() { name = "triggerContainer" };
            triggerContainer.style.flexDirection = FlexDirection.Row; // Arrange elements horizontally

            // Create the trigger field
            var triggerField = new TextField("") {};
            if (fromSave)
            {
                triggerField.value = trigger;
                triggerField.label = index.ToString();
            }
            else
            {
                triggerField.label = nodeCache.Trigger.Count.ToString();
                nodeCache.Trigger.Add("");
            }
            triggerField.RegisterValueChangedCallback(evt =>
            {
                int i = int.Parse(triggerField.label);
                markNodeUnsaved(nodeCache);
                nodeCache.Trigger[i] = triggerField.value;
            });

            // Add the buttons
            var deleteButton = new Button(() => RemoveTrigger(nodeCache, triggerField))
            {
                text = "X",
            };
            deleteButton.style.width = 20;

            var upButton = new Button(() => SwapTrigger(nodeCache, int.Parse(triggerField.label), int.Parse(triggerField.label) - 1))
            {
                text = "up",
            };
            upButton.style.width = 40;

            var downButton = new Button(() => SwapTrigger(nodeCache, int.Parse(triggerField.label), int.Parse(triggerField.label) + 1))
            {
                text = "down",
            };
            downButton.style.width = 40;

            // Add elements to the container
            triggerContainer.Add(deleteButton);
            triggerContainer.Add(upButton);
            triggerContainer.Add(downButton);
            triggerContainer.Add(triggerField);

            // Add the container to the main container
            nodeCache.mainContainer.Add(triggerContainer);

            
        }


        private void RemoveTrigger(DialogueNode nodeCache, TextField triggerField)
        {
            if (nodeCache.Trigger.Count() > int.Parse(triggerField.label))
            {
                nodeCache.Trigger.RemoveAt(int.Parse(triggerField.label));
            }

            RefreshTriggers(nodeCache);
        }

        private void SwapTrigger(DialogueNode nodeCache, int i, int j)
        {
            if (j < 0)
            {
                return;
            }
            if (j >= nodeCache.Trigger.Count())
            {
                return;
            }
            (nodeCache.Trigger[j], nodeCache.Trigger[i]) = (nodeCache.Trigger[i], nodeCache.Trigger[j]);
            RefreshTriggers(nodeCache);
        }

        private void RefreshTriggers(DialogueNode nodeCache)
        {
            // Remove all elements with the "TriggerField" or "TriggerButton" name
            var triggerElements = nodeCache.mainContainer.Children().Where(child =>
                child.name == "triggerContainer").ToList();

            foreach (var element in triggerElements)
            {
                nodeCache.mainContainer.Remove(element);
            }

            // Re-add trigger fields with updated indices
            for (int i = 0; i < nodeCache.Trigger.Count; i++)
            {
                AddTrigger(nodeCache, nodeCache.Trigger[i], i, true);
            }

            // Mark the node as unsaved and refresh its state
            markNodeUnsaved(nodeCache);
            nodeCache.RefreshPorts();
            nodeCache.RefreshExpandedState();
        }

        private Port GetPortInstance(DialogueNode node, Direction nodeDirection,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            var instantiatedPort = node.InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
            instantiatedPort.portColor = Color.red;
            return instantiatedPort;
        }

        private DialogueNode GetEntryPointNodeInstance()
        {
            var nodeCache = new DialogueNode()
            {
                title = "START",
                GUID = Guid.NewGuid().ToString(),
                DialogueText = "ENTRYPOINT",
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
