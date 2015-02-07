using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityVSModuleCommon.Application;

namespace UnityVSModuleBuilder.GUI
{
    public partial class SettingsWindow : Form
    {
        private readonly ApplicationManager settingsManager;
        private string repoLocation = String.Empty;
        

        public SettingsWindow()
        {
            InitializeComponent();
            settingsManager = ApplicationFactory.GetNewApplicationManager();
        }

        private void LoadWindow(object sender, EventArgs e)
        {
            flpkrRepoLocation.IsExistEnforced = false;
            QueryManagerForFieldValues();
            PopulateFields();
        }

        private void QueryManagerForFieldValues()
        {
            ApplicationSettingsResponse response = settingsManager.RetrieveApplicationSettings();
            if (response.GetCode() == AppSettingsCode.SUCCESS)
            {
                ApplicationSettings settings = response.GetApplicationSettings();
                repoLocation = settings.GetRepoLocation();
            }
            else if (response.GetCode() == AppSettingsCode.INSTALL_NOT_FOUND)
            {
                MessageBox.Show("Error Locating Application Settings. Please Reinstall Unity VS Module Builder.");
                Close();
            }
            else if (response.GetCode() == AppSettingsCode.UNKNOWN_ERROR)
            {
                MessageBox.Show("Error Reading Application Settings. See Log For Details.");
                Close();
            }
        }

        private void PopulateFields()
        {
            flpkrRepoLocation.SetChosen(repoLocation);
        }

        private void ApplyButtonClicked(object sender, EventArgs e)
        {
            QueryFields();
            ApplicationSettings settings = ApplicationFactory.GetNewApplicationSettings(repoLocation);
            settingsManager.SaveApplicationSettings(settings);
            Close();
        }

        private void QueryFields()
        {
            repoLocation = flpkrRepoLocation.GetChosen();
        }

        private void CancelButtonClicked(object sender, EventArgs e)
        {
            Close();
        }

    }
}
