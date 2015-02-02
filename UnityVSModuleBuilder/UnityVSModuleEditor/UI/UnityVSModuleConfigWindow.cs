using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityVSModuleCommon;
using UnityVSModuleEditor.MiddleTier;

namespace UnityVSModuleEditor.UI
{
    public class UnityVSModuleConfigWindow : EditorWindow
    {
        private const int COMMAND_WIDTH = 175;
        private const int LABEL_WIDTH = 150;
        private const int DEPENDENCY_HEIGHT = 200;
        private const string CONFIGURATION_LABEL = "Configuration";
        private const string REPO_LOCATION_LABEL = "Repo location";
        private const string UNITY_INSTALL_DIR_LABEL = "Unity install location";
        private const string APPLY_BUTTON_TEXT = "Apply";
        private const string MODULE_INFO_HEADER_TEXT = "Module info";
        private const string EXPORT_TO_REPO_BUTTON_TEXT = "Export to repo";
        private const string  DEPENDENCY_HEADER_TEXT = "Dependency modules";
        private const string DEPENDENCY_ADD_BUTTON_TEXT = "Import from repo";
        private const string DEPENDENCY_SELECT_ALL_BUTTON_TEXT = "Select all";
        private const string DEPENDENCY_SELECT_NONE_BUTTON_TEXT = "Select none";
        private const string DEPENDENCY_UPDATE_BUTTON_TEXT = "Update selected";
        private const string DEPENCENY_REMOVE_BUTTON_TEXT = "Remove selected";
        

        private VSModuleDelegate vsModuleDelegate;
        private Vector2 windowScrollPosition;
        private Vector2 dependencyScroll;
        private GUIStyle headerStyle;
        private GUILayoutOption commandWidth;
        private VSModuleSettingsTO vsModuleSettingsTO;
        private GUILayoutOption labelWidth;
        private GUILayoutOption dependencyHeight;

        private String companyNameLabel = String.Empty;
        private String projectNameLabel = String.Empty;
        private String companyShortNameLabel = String.Empty;
        private VSModuleDependencyTO vsDependencyTO;
        private List<DependencySelection> dependencySelections;
        
        public UnityVSModuleConfigWindow()
        {
            Logger.SetService(UnityLoggingService.INSTANCE);
            UnityApi api = UnityApiFactory.GetUnityApi();
            vsModuleDelegate = VSModuleFactory.GetNewDelegate(api);
        }

        private void InitGui()
        {
            CreateStyleOptions();
            PopulateVSModuleTOs();
            InitializeConfigLabels();
            PopulateUIState();
        }

        private void PopulateUIState()
        {
            if (dependencySelections == null)
            {
                PopulateDependencySelections();
            }
            
        }

        private void PopulateDependencySelections()
        {
            dependencySelections = new List<DependencySelection>();
            vsDependencyTO = vsModuleDelegate.RetrieveModuleDependenciesTO();
            List<VSModuleDependencyItem>.Enumerator items =  vsDependencyTO.GetDependencies();
            while(items.MoveNext()){
                DependencySelection selection = new DependencySelection();
                selection.item = items.Current;
                dependencySelections.Add(selection);
            }
        }

        private void InitializeConfigLabels()
        {
            projectNameLabel = "Project Name: " + vsModuleSettingsTO.GetProjectName();
            companyNameLabel = "Company Name: " + vsModuleSettingsTO.GetCompanyName(); 
            companyShortNameLabel = "Company Short Name: " + vsModuleSettingsTO.GetCompanyShortName();
        }

        private void PopulateVSModuleTOs()
        {
            if (vsModuleSettingsTO == null)
            {
                vsModuleSettingsTO = vsModuleDelegate.RetrieveModuleSettingsTO();    
            }
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
            try
            {
                InitGui();
                windowScrollPosition = GUILayout.BeginScrollView(windowScrollPosition);
                DrawConfigurationArea();
                DrawModuleInfoArea();
                DrawDependencyInfoArea();
                GUILayout.EndScrollView();
            }
            catch (Exception e)
            {
                Debug.LogError("Unexpected Exception Rendering VSModule UI. See Logged Error For Details.");
                Debug.LogException(e);
            }
            
        }

        private void DrawConfigurationArea()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(CONFIGURATION_LABEL, headerStyle);
            GUILayout.BeginHorizontal();
            GUILayout.Label(REPO_LOCATION_LABEL, labelWidth);
            String repoLocation = GUILayout.TextField(vsModuleSettingsTO.GetRepoLocation());
            if(repoLocation != null){
                vsModuleSettingsTO.SetRepoLocation(repoLocation);
            }
            
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label(UNITY_INSTALL_DIR_LABEL, labelWidth);
            String installLocation = GUILayout.TextField(vsModuleSettingsTO.GetUnityInstallLocation());
            if (installLocation != null)
            {
                vsModuleSettingsTO.SetUnityInstallLocation(installLocation);
            }
                
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(APPLY_BUTTON_TEXT, commandWidth))
            {
                vsModuleDelegate.SaveModuleSettingsTO(vsModuleSettingsTO);
                vsModuleDelegate.UpdateUnitySettings();
            }
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
        }

        private void HandleDependencyWindowClosed(DependencyPromptWindow window)
        {
            String companyName = window.GetCompanyName().Trim();
            String projectName = window.GetProjectName().Trim();
            vsModuleDelegate.AddModuleDependency(companyName, projectName);
            vsDependencyTO = vsModuleDelegate.RetrieveModuleDependenciesTO();
            PopulateDependencySelections();
        }

        private void DrawDependencyInfoArea()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(DEPENDENCY_HEADER_TEXT, headerStyle);
            if (GUILayout.Button(DEPENDENCY_ADD_BUTTON_TEXT, commandWidth))
            {
                DependencyPromptWindow window = ScriptableObject.CreateInstance<DependencyPromptWindow>();
                window.onClose += HandleDependencyWindowClosed;
                window.Prompt();
            }
            GUILayout.BeginVertical(GUI.skin.box, dependencyHeight);
            dependencyScroll = GUILayout.BeginScrollView(dependencyScroll);

            int dependencyCount = vsDependencyTO.GetDependencyCount();
            if (dependencyCount > 0)
            {
                DrawDependencyTree();
            }
            else
            {
                GUILayout.Label("VSModule has no dependencies.");
            }
            
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            if (GUILayout.Button(DEPENDENCY_UPDATE_BUTTON_TEXT, commandWidth))
            {
                UpdateSelectedDependencies();
            }
            if (GUILayout.Button(DEPENCENY_REMOVE_BUTTON_TEXT, commandWidth))
            {
                RemoveSelectedDependencies();
                
            }

            GUILayout.EndVertical();
        }

        private void RemoveSelectedDependencies()
        {
            if (PopConfirmRemoveDependencyDialog())
            {
                List<VSModuleDependencyItem> selected =  GetSelectedDependencies();
                vsModuleDelegate.RemoveDependencies(selected.GetEnumerator());
                PopulateDependencySelections();
            }
        }

        private bool PopConfirmRemoveDependencyDialog()
        {
            return EditorUtility.DisplayDialog("Confirm Dependency Removal", "Are you sure you want to remove the selected dependencies?", "Yes", "No");
        }

        private void UpdateSelectedDependencies()
        {
            List<VSModuleDependencyItem> selected = GetSelectedDependencies();
            vsModuleDelegate.UpdateModuleDependencies(selected.GetEnumerator());
        }

        private List<VSModuleDependencyItem> GetSelectedDependencies()
        {
            List<VSModuleDependencyItem> selected = new List<VSModuleDependencyItem>();
            foreach (DependencySelection selection in dependencySelections)
            {
                if (selection.isSelected)
                {
                    selected.Add(selection.item);
                }
            }
            return selected;
        }

        private void DrawDependencyTree()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(DEPENDENCY_SELECT_ALL_BUTTON_TEXT, GUILayout.Width(75)))
            {
                SelectAllDependencies();
            }
            if (GUILayout.Button(DEPENDENCY_SELECT_NONE_BUTTON_TEXT, GUILayout.Width(100)))
            {
                SelectNoDependencies();
            }
            GUILayout.EndHorizontal();
            List<VSModuleDependencyItem>.Enumerator items = this.vsDependencyTO.GetDependencies();
            GUILayout.BeginVertical(GUI.skin.box);
            foreach (DependencySelection selection in dependencySelections)
            {
                String label = selection.item.GetDisplayValue();
                selection.isSelected = GUILayout.Toggle(selection.isSelected, label);
            }
            GUILayout.EndVertical();
        }

        private void SelectNoDependencies()
        {
            SetAllDependenciesSelected(false);
        }

        private void SelectAllDependencies()
        {
            SetAllDependenciesSelected(true);
        }

        private void SetAllDependenciesSelected(bool isSelected)
        {
            for (int dependencyIndex = 0; dependencyIndex < dependencySelections.Count; dependencyIndex++)
            {
                dependencySelections[dependencyIndex].isSelected = isSelected;
            }
        }

        private void DrawModuleInfoArea()
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(MODULE_INFO_HEADER_TEXT, headerStyle);
            GUILayout.Label(projectNameLabel);
            GUILayout.Label(companyNameLabel);
            GUILayout.Label(companyShortNameLabel);
            if (GUILayout.Button(EXPORT_TO_REPO_BUTTON_TEXT, commandWidth))
            {
                vsModuleDelegate.ExportModuleToRepository();
            }
            GUILayout.EndVertical();
        }

    }


    class DependencySelection
    {
        public VSModuleDependencyItem item;
        public bool isSelected;
    }
}
