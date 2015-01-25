using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityVSModuleEditor.UnityApis;

namespace UnityVSModuleEditor
{
    public class UnityVSModuleConfigWindow : EditorWindow
    {
        private const int COMMAND_WIDTH = 200;
        private const int LABEL_WIDTH = 150;
        private const int DEPENDENCY_HEIGHT = 200;
        private const String CONFIGURATION_LABEL = "Configuration";
        private const String REPO_LOCATION_LABEL = "Repo Location";
        private const string UNITY_INSTALL_DIR_LABEL = "Unity Install Dir";
        private const string APPLY_BUTTON_TEXT = "Apply";

        private VSModuleDelegate vsModuleDelegate;
        private Vector2 windowScrollPosition;
        private Vector2 dependencyScroll;
        private GUIStyle headerStyle;
        private GUILayoutOption commandWidth;
        private bool isGuiInitialized;
        private VSModuleSettingsTO vsModuleSettingsTO;
        private GUILayoutOption labelWidth;
        
        private GUILayoutOption dependencyHeight;

        private String companyNameLabel = String.Empty;
        private String projectNameLabel = String.Empty;
        private String companyShortNameLabel = String.Empty;
        
        private void InitGui()
        {
            if (!isGuiInitialized)
            {
                isGuiInitialized = true;
                CreateStyleOptions();
                PopulateVSModuleSettings();
                InitializeConfigLabels();
            }
        }

        private void InitializeConfigLabels()
        {
            projectNameLabel = "Project Name: " + vsModuleSettingsTO.GetProjectName();
            companyNameLabel = "Company Name: " + vsModuleSettingsTO.GetCompanyName(); 
            companyShortNameLabel = "Company Short Name: " + vsModuleSettingsTO.GetCompanyShortName();
        }

        private void PopulateVSModuleSettings()
        {
            vsModuleDelegate = new VSModuleDelegate(new UnityApiImpl());
            vsModuleSettingsTO = vsModuleDelegate.RetrieveModuleSettingsTO();    
        }

        private void CreateStyleOptions()
        {
            headerStyle = new GUIStyle(GUI.skin.label);
            headerStyle.padding = new RectOffset(0, 0, 0, 10);
            headerStyle.fontStyle = FontStyle.Bold;

            commandWidth = GUILayout.Width(COMMAND_WIDTH);
            labelWidth = GUILayout.Width(LABEL_WIDTH);
            dependencyHeight = GUILayout.MaxHeight(DEPENDENCY_HEIGHT);
        }

        public void OnGUI()
        {
            InitGui();
            windowScrollPosition = GUILayout.BeginScrollView(windowScrollPosition);
            DrawConfigurationArea();
            DrawModuleInfoArea();
            DrawDependencyInfoArea();
            GUILayout.EndScrollView();
        }

        private void DrawConfigurationArea()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(CONFIGURATION_LABEL, headerStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label(REPO_LOCATION_LABEL, labelWidth);
            vsModuleSettingsTO.SetRepoLocation(GUILayout.TextField(vsModuleSettingsTO.GetRepoLocation()));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label(UNITY_INSTALL_DIR_LABEL, labelWidth);
            vsModuleSettingsTO.SetUnityInstallLocation(GUILayout.TextField(vsModuleSettingsTO.GetUnityInstallLocation()));
            GUILayout.EndHorizontal();

            if (GUILayout.Button(APPLY_BUTTON_TEXT, commandWidth))
            {
                vsModuleDelegate.SaveModuleSettingsTO(vsModuleSettingsTO);
            }
            GUILayout.EndVertical();
        }

        private void DrawDependencyInfoArea()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Dependency Modules", headerStyle);
            GUILayout.Button("Import From Repo", commandWidth);
            GUILayout.BeginVertical(GUI.skin.box, dependencyHeight);
            dependencyScroll = GUILayout.BeginScrollView(dependencyScroll);
            GUILayout.BeginHorizontal();
            GUILayout.Button("Select All", GUILayout.Width(75));
            GUILayout.Button("Select None", GUILayout.Width(75));
            GUILayout.EndHorizontal();
            GUILayout.Toggle(true, "Dep");
            GUILayout.Toggle(true, "Dep");
            GUILayout.Toggle(true, "Dep");
            GUILayout.Toggle(true, "Dep");
            GUILayout.Toggle(true, "Dep");
            GUILayout.Toggle(true, "Dep");
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.Button("Update Selected", commandWidth);
            GUILayout.Button("Remove Selected", commandWidth);

            GUILayout.EndVertical();
        }

        private void DrawModuleInfoArea()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Module Info", headerStyle);
            GUILayout.Label(projectNameLabel);
            GUILayout.Label(companyNameLabel);
            GUILayout.Label(companyShortNameLabel);
            if (GUILayout.Button("Export To Repo", commandWidth))
            {

            }
            GUILayout.EndVertical();
        }

    }
}
