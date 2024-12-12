using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Subtegral.SceneGraphSystem.Editor
{
    public class SceneNodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private EditorWindow _window;
        private SceneGraphView _graphView;

        private Texture2D _indentationIcon;

        public void Configure(EditorWindow window, SceneGraphView graphView)
        {
            _window = window;
            _graphView = graphView;

            //Transparent 1px indentation icon as a hack
            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            _indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
                new SearchTreeGroupEntry(new GUIContent("New Scene"), 1),
                new SearchTreeEntry(new GUIContent("Scene Node", _indentationIcon))
                {
                    level = 2, userData = new SceneNode()
                },
                new SearchTreeEntry(new GUIContent("Comment Block",_indentationIcon))
                {
                    level = 1,
                    userData = new Group()
                }
            };

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            //Editor window-based mouse position
            var mousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent,
                context.screenMousePosition - _window.position.position);
            var graphMousePosition = _graphView.contentViewContainer.WorldToLocal(mousePosition);
            switch (SearchTreeEntry.userData)
            {
                case SceneNode dialogueNode:
                    _graphView.CreateNewSceneNode("Scene Node", graphMousePosition);
                    return true;
                case Group group:
                    var rect = new Rect(graphMousePosition, _graphView.DefaultCommentBlockSize);
                    _graphView.CreateCommentBlock(rect);
                    return true;
            }
            return false;
        }
    }
}