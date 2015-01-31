using System;
using UnityEditor;
using UnityEngine;

namespace UnityVSModuleEditor.UI
{
    delegate void HandleWindowClose(DependencyPromptWindow closed);

    internal class DependencyPromptWindow : EditorWindow
    {
        public event HandleWindowClose onClose;

        private const string WINDOW_TITLE_TEXT = "Enter Dependency Info";
        private const string COMPANY_SHORT_NAME_LABEL_TEXT = "Company short name";
        private const string PROJECT_NAME_LABEL_TEXT = "Project name";
        private const string ADD_BUTTON_TEXT = "Add";
        private const string CANCEL_BUTTON_TEXT = "Cancel";
        private const int HALF_WIDTH = 150;
        private const int HALF_HEIGHT = 60;

        private string companyName = String.Empty;
        private string projectName = String.Empty;
        private bool isAdd = false;
        
        public void Prompt()
        {
            ShowPopup();
            Vector2 mousePosition = Event.current.mousePosition;
            Vector2 converted = GUIUtility.GUIToScreenPoint(mousePosition);
            Rect centeredOnMousePosition = new Rect(converted.x - HALF_WIDTH, converted.y - HALF_HEIGHT, HALF_WIDTH * 2, HALF_HEIGHT * 2);
            position = centeredOnMousePosition;
        }

        public void OnGUI()
        {
            GUIStyle labelStyle = GetLabelStyle();
            GUIStyle textFieldStyle = GetTextFieldStyle();
            GUIStyle buttonStyle = GetButtonStyle();
            
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label(WINDOW_TITLE_TEXT, GetTitleStyle());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label(COMPANY_SHORT_NAME_LABEL_TEXT, labelStyle);
            companyName = GUILayout.TextField(companyName, textFieldStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(PROJECT_NAME_LABEL_TEXT, labelStyle);
            projectName = GUILayout.TextField(projectName, textFieldStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(ADD_BUTTON_TEXT, buttonStyle))
            {
                isAdd = true;
                Close();
            }
            if (GUILayout.Button(CANCEL_BUTTON_TEXT, buttonStyle))
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

        private GUIStyle GetTitleStyle()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleCenter;
            style.padding = new RectOffset(0, 0, 5, 5);
            return style;
        }

        private GUIStyle GetButtonStyle()
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fixedWidth = 75;
            return buttonStyle;
        }

        private GUIStyle GetTextFieldStyle()
        {
            GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField);
            textFieldStyle.stretchWidth = false;
            textFieldStyle.stretchHeight = true;
            textFieldStyle.alignment = TextAnchor.MiddleLeft;
            textFieldStyle.fixedWidth = HALF_WIDTH;
            return textFieldStyle;
        }

        private GUIStyle GetLabelStyle()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.stretchWidth = true;
            labelStyle.stretchHeight = true;
            labelStyle.alignment = TextAnchor.MiddleLeft;
            return labelStyle;
        }
    }
}
