using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodiceApp;
using Subtegral.DialogueSystem.DataContainers;
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
                foreach (string triggerName in trigger)
                {
                    AddTrigger(tempDialogueNode, triggerName, fromSave);
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

        public void AddTrigger(DialogueNode nodeCache, string trigger = "", bool fromSave = false)
        {
            /// TRY nodeCache.mainContainer.IndexOf(triggerfield) TO GET INDEX OF EACH TEXTBOX

            var triggerField = new TextField("");
            if (fromSave)
            {
                triggerField.value = trigger;
                triggerField.label = trigger;
            }
            else
            {
                triggerField.label = "press enter";
            }
            triggerField.RegisterValueChangedCallback(evt =>
            {
                /*Debug.Log("======================================");
                for (int i = 0; i < nodeCache.Trigger.Count; i++)
                {
                    Debug.Log(nodeCache.Trigger[i]);
                }*/
                markNodeUnsaved(nodeCache);
            });
            nodeCache.mainContainer.Add(triggerField);
            if (nodeCache.Trigger == null)
            {
                nodeCache.Trigger = new List<string>();
            }

            triggerField.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.Return && triggerField.value != "" && nodeCache.Trigger.IndexOf(triggerField.value) < 0 )
                {
                    try {
                        nodeCache.Trigger.Remove(triggerField.label);
                    } catch (Exception ex) { }
                    nodeCache.Trigger.Add(triggerField.value);
                    triggerField.label = triggerField.value;
                    evt.StopPropagation();
                    Debug.Log("======================================");
                    for (int i = 0; i < nodeCache.Trigger.Count; i++)
                    {
                        Debug.Log(nodeCache.Trigger[i]);    
                    }
                    
                }
            });

            var deleteButton = new Button(() => RemoveTrigger(nodeCache, triggerField))
            {
                text = "X"
            };
            deleteButton.style.width = 10;
            nodeCache.mainContainer.Add(deleteButton);
        }

        private void RemoveTrigger(DialogueNode nodeCache, TextField triggerField)
        {
            nodeCache.Trigger.Remove(triggerField.value);
            Debug.Log("======================================");
            Debug.Log($"removed {triggerField.value}");
            Debug.Log("======================================");
            for (int i = 0; i < nodeCache.Trigger.Count; i++)
            {
                Debug.Log(nodeCache.Trigger[i]);
            }
            nodeCache.mainContainer.RemoveAt(nodeCache.mainContainer.IndexOf(triggerField) + 1);
            nodeCache.mainContainer.Remove(triggerField);
            
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