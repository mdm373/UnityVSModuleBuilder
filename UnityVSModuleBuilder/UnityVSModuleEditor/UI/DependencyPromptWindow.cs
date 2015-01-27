using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UnityVSModuleEditor.UI
{
    delegate void HandleWindowClose(DependencyPromptWindow closed);

    class DependencyPromptWindow : EditorWindow
    {
        public event HandleWindowClose onClose;

        private string companyName = String.Empty;
        private string projectName = String.Empty;
        private float halfWidth = 150;
        private float halfHeight = 125;
        private bool isAdd = false;

        public void Prompt()
        {
            ShowPopup();
            Vector2 mousePosition = Event.current.mousePosition;
            Vector2 converted = GUIUtility.GUIToScreenPoint(mousePosition);
            Debug.Log(converted);
            Rect centeredOnMousePosition = new Rect(converted.x - halfWidth, converted.y - halfHeight, halfWidth * 2, halfHeight * 2);
            position = centeredOnMousePosition;
        }

        public void OnGUI()
        {
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label("Enter Dependency Info");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Company Name:");
            companyName = GUILayout.TextField(companyName);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Project Name:");
            projectName = GUILayout.TextField(projectName);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add", GUILayout.Width(50)))
            {
                isAdd = true;
                Close();
            }
            if (GUILayout.Button("Cancel", GUILayout.Width(50)))
            {
                Close();
            }
            GUILayout.EndHorizontal();
        }

        public void OnDestroy()
        {
            if (isAdd && onClose != null)
            {
                onClose(this);
            }
        }

        public void OnLostFocus()
        {
            Focus();
        }

        public String GetProjectName()
        {
            return projectName;
        }

        public String GetCompanyName()
        {
            return companyName;
        }

    }
}
