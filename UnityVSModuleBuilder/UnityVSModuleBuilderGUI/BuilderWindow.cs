using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityVSModuleCommon;

namespace UnityVSModuleBuilder.GUI
{
    public partial class BuilderWindow : Form
    {
        private readonly LoggingFileService loggingService;
        private readonly FieldConfig config;
        private readonly TemplateProjectBuilder projectBuilder;
        
        public BuilderWindow()
        {
            this.loggingService = new LoggingFileService();
            Logger.SetService(loggingService);
            this.projectBuilder = TemplateProjectFactory.GetNewTemplateProjectBuilder();
            InitializeComponent();
            config = FieldConfig.ReadFromFile();
            InitializeConfiguration();
            UpdateFieldsFromConfig();
        }

        private void InitializeConfiguration()
        {
            flpkrOutput.IsExistEnforced = false;
            flpkrRepoLocation.IsExistEnforced = false;
            flpkrOutput.IsExistEnforced = false;
            
        }

        private void ExitMenuClicked(object sender, EventArgs e)
        {
            Close();
        }

        private void ResetMenuClicked(object sender, EventArgs e)
        {
            config.Reset();
            UpdateFieldsFromConfig();
        }

        private void UpdateFieldsFromConfig()
        {
            txtProjectName.Text = config.ProjectName;
            txtCompanyName.Text = config.CompanyName;
            txtCompanyShortName.Text = config.CompanyShortName;
            flpkrRepoLocation.SetChosen(config.RepositoryLocation);
            flpkrUnityInstall.SetChosen(config.UnityInstallLocation);
            flpkrOutput.SetChosen(config.ProjectGenerationLocation);

        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateAndSaveConfig();
            loggingService.CloseLog();
        }

        private void UpdateAndSaveConfig()
        {
            if (config != null)
            {
                config.ProjectName = txtProjectName.Text;
                config.CompanyName = txtCompanyName.Text;
                config.CompanyShortName = txtCompanyShortName.Text;
                config.RepositoryLocation = flpkrRepoLocation.GetChosen();
                config.UnityInstallLocation = flpkrUnityInstall.GetChosen();
                config.ProjectGenerationLocation = flpkrOutput.GetChosen();
                config.SaveToFile();
            }
        }

        private void AboutMenuClicked(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void BuildButtonClicked(object sender, EventArgs e)
        {
            UpdateAndSaveConfig();
            BuildProjectResponse response = projectBuilder.DoBuild(config);
            DisplayBuildResults(response);
        }

        private void DisplayBuildResults(BuildProjectResponse response)
        {
            String message = null;
            String title = null;
            MessageBoxIcon icon = MessageBoxIcon.None;
            if (response.IsSuccess())
            {
                message = "Great Success! Your Project has been generated. See output folder for results.";
                title = "Project Successfully Generated";
                icon = MessageBoxIcon.Information;
            }
            else
            {
                message = "Ohh My! Project generation failed. See log for error details.";
                title = "Project Generation Failed";
                icon = MessageBoxIcon.Error;
            }
            MessageBox.Show(this, message, title, MessageBoxButtons.OK, icon);
        }



    }
}
