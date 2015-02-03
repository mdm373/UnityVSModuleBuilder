using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace UnityVSModuleBuilder.GUI
{
    public partial class FilePicker : UserControl
    {
        private DirectoryInfo chosen = null;
        private const string NONE = "None";
        
        
        public FilePicker()
        {
            IsExistEnforced = true;
            InitializeComponent();
            UpdateLabel();
        }

        public Boolean IsExistEnforced { get; set; }

        public String GetChosen()
        {
            String path = String.Empty;
            if (chosen != null)
            {
                path = chosen.FullName;
            }
            return path;
        }

        public void SetChosen(String path)
        {
            this.chosen = null;
            if (!String.IsNullOrEmpty(path))
            {
                this.chosen = new DirectoryInfo(path);
                if (!this.chosen.Exists && this.IsExistEnforced)
                {
                    this.chosen = null;
                }
            }
            UpdateLabel();
        }

        private void ChooseClicked(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.ShowNewFolderButton = true;
            if (chosen != null)
            {
                browser.RootFolder = Environment.SpecialFolder.MyComputer;
                browser.SelectedPath = chosen.FullName;
            }
            DialogResult result = browser.ShowDialog();
            if (result == DialogResult.OK)
            {
                String path = browser.SelectedPath;
                chosen = new DirectoryInfo(path);
                UpdateLabel();
            }
        }

        private void UpdateLabel()
        {
            if (chosen != null)
            {
                this.selectionLabel.Text = chosen.FullName;
            }
            else
            {
                this.selectionLabel.Text = NONE;
            }
        }
    }
}
