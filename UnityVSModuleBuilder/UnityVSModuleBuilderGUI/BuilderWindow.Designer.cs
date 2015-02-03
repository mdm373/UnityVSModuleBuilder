namespace UnityVSModuleBuilder.GUI
{
    partial class BuilderWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuilderWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fieldsTable = new System.Windows.Forms.TableLayoutPanel();
            this.lblOutputLocation = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblCompanyShortName = new System.Windows.Forms.Label();
            this.lblRepoLocation = new System.Windows.Forms.Label();
            this.lblUnityInstall = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.txtCompanyShortName = new System.Windows.Forms.TextBox();
            this.formTitle = new System.Windows.Forms.Label();
            this.formTable = new System.Windows.Forms.TableLayoutPanel();
            this.titlePanel = new System.Windows.Forms.Panel();
            this.logoPicture = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.flpkrRepoLocation = new UnityVSModuleBuilder.GUI.FilePicker();
            this.flpkrUnityInstall = new UnityVSModuleBuilder.GUI.FilePicker();
            this.flpkrOutput = new UnityVSModuleBuilder.GUI.FilePicker();
            this.menuStrip1.SuspendLayout();
            this.fieldsTable.SuspendLayout();
            this.formTable.SuspendLayout();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPicture)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(634, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.resetToolStripMenuItem.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.ResetMenuClicked);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.exitToolStripMenuItem.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitMenuClicked);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutMenuClicked);
            // 
            // fieldsTable
            // 
            this.fieldsTable.BackColor = System.Drawing.SystemColors.Control;
            this.fieldsTable.ColumnCount = 2;
            this.fieldsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 146F));
            this.fieldsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.fieldsTable.Controls.Add(this.lblOutputLocation, 0, 5);
            this.fieldsTable.Controls.Add(this.lblProjectName, 0, 0);
            this.fieldsTable.Controls.Add(this.txtProjectName, 1, 0);
            this.fieldsTable.Controls.Add(this.lblCompanyName, 0, 1);
            this.fieldsTable.Controls.Add(this.lblCompanyShortName, 0, 2);
            this.fieldsTable.Controls.Add(this.lblRepoLocation, 0, 3);
            this.fieldsTable.Controls.Add(this.lblUnityInstall, 0, 4);
            this.fieldsTable.Controls.Add(this.txtCompanyName, 1, 1);
            this.fieldsTable.Controls.Add(this.txtCompanyShortName, 1, 2);
            this.fieldsTable.Controls.Add(this.flpkrRepoLocation, 1, 3);
            this.fieldsTable.Controls.Add(this.flpkrUnityInstall, 1, 4);
            this.fieldsTable.Controls.Add(this.flpkrOutput, 1, 5);
            this.fieldsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fieldsTable.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fieldsTable.Location = new System.Drawing.Point(3, 49);
            this.fieldsTable.Name = "fieldsTable";
            this.fieldsTable.RowCount = 6;
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.fieldsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.fieldsTable.Size = new System.Drawing.Size(628, 349);
            this.fieldsTable.TabIndex = 1;
            // 
            // lblOutputLocation
            // 
            this.lblOutputLocation.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblOutputLocation.AutoSize = true;
            this.lblOutputLocation.BackColor = System.Drawing.SystemColors.Control;
            this.lblOutputLocation.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputLocation.Location = new System.Drawing.Point(50, 312);
            this.lblOutputLocation.Name = "lblOutputLocation";
            this.lblOutputLocation.Size = new System.Drawing.Size(93, 15);
            this.lblOutputLocation.TabIndex = 9;
            this.lblOutputLocation.Text = "Output location";
            this.lblOutputLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProjectName
            // 
            this.lblProjectName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.BackColor = System.Drawing.SystemColors.Control;
            this.lblProjectName.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.Location = new System.Drawing.Point(64, 21);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(79, 15);
            this.lblProjectName.TabIndex = 2;
            this.lblProjectName.Text = "Project name";
            this.lblProjectName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProjectName
            // 
            this.txtProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProjectName.BackColor = System.Drawing.SystemColors.Control;
            this.txtProjectName.Location = new System.Drawing.Point(149, 17);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(476, 23);
            this.txtProjectName.TabIndex = 1;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.BackColor = System.Drawing.SystemColors.Control;
            this.lblCompanyName.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyName.Location = new System.Drawing.Point(52, 79);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(91, 15);
            this.lblCompanyName.TabIndex = 3;
            this.lblCompanyName.Text = "Company name";
            this.lblCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCompanyShortName
            // 
            this.lblCompanyShortName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCompanyShortName.AutoSize = true;
            this.lblCompanyShortName.BackColor = System.Drawing.SystemColors.Control;
            this.lblCompanyShortName.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyShortName.Location = new System.Drawing.Point(20, 137);
            this.lblCompanyShortName.Name = "lblCompanyShortName";
            this.lblCompanyShortName.Size = new System.Drawing.Size(123, 15);
            this.lblCompanyShortName.TabIndex = 4;
            this.lblCompanyShortName.Text = "Company short name";
            this.lblCompanyShortName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRepoLocation
            // 
            this.lblRepoLocation.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRepoLocation.AutoSize = true;
            this.lblRepoLocation.BackColor = System.Drawing.SystemColors.Control;
            this.lblRepoLocation.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRepoLocation.Location = new System.Drawing.Point(29, 195);
            this.lblRepoLocation.Name = "lblRepoLocation";
            this.lblRepoLocation.Size = new System.Drawing.Size(114, 15);
            this.lblRepoLocation.TabIndex = 5;
            this.lblRepoLocation.Text = "Repository location";
            this.lblRepoLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUnityInstall
            // 
            this.lblUnityInstall.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblUnityInstall.AutoSize = true;
            this.lblUnityInstall.BackColor = System.Drawing.SystemColors.Control;
            this.lblUnityInstall.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnityInstall.Location = new System.Drawing.Point(20, 253);
            this.lblUnityInstall.Name = "lblUnityInstall";
            this.lblUnityInstall.Size = new System.Drawing.Size(123, 15);
            this.lblUnityInstall.TabIndex = 6;
            this.lblUnityInstall.Text = "Unity install location";
            this.lblUnityInstall.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCompanyName.BackColor = System.Drawing.SystemColors.Control;
            this.txtCompanyName.Location = new System.Drawing.Point(149, 75);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(476, 23);
            this.txtCompanyName.TabIndex = 7;
            // 
            // txtCompanyShortName
            // 
            this.txtCompanyShortName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCompanyShortName.BackColor = System.Drawing.SystemColors.Control;
            this.txtCompanyShortName.Location = new System.Drawing.Point(149, 133);
            this.txtCompanyShortName.Name = "txtCompanyShortName";
            this.txtCompanyShortName.Size = new System.Drawing.Size(476, 23);
            this.txtCompanyShortName.TabIndex = 8;
            // 
            // formTitle
            // 
            this.formTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formTitle.BackColor = System.Drawing.Color.Black;
            this.formTitle.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formTitle.ForeColor = System.Drawing.Color.White;
            this.formTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.formTitle.Location = new System.Drawing.Point(0, 0);
            this.formTitle.Margin = new System.Windows.Forms.Padding(0);
            this.formTitle.Name = "formTitle";
            this.formTitle.Padding = new System.Windows.Forms.Padding(40, 3, 3, 3);
            this.formTitle.Size = new System.Drawing.Size(644, 41);
            this.formTitle.TabIndex = 2;
            this.formTitle.Text = "Unity VS Module Builder";
            this.formTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // formTable
            // 
            this.formTable.BackColor = System.Drawing.SystemColors.Control;
            this.formTable.ColumnCount = 1;
            this.formTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.formTable.Controls.Add(this.fieldsTable, 0, 1);
            this.formTable.Controls.Add(this.titlePanel, 0, 0);
            this.formTable.Controls.Add(this.panel3, 0, 2);
            this.formTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formTable.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.formTable.Location = new System.Drawing.Point(0, 24);
            this.formTable.Margin = new System.Windows.Forms.Padding(0);
            this.formTable.Name = "formTable";
            this.formTable.RowCount = 3;
            this.formTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.formTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.formTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.formTable.Size = new System.Drawing.Size(634, 447);
            this.formTable.TabIndex = 1;
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.SystemColors.Control;
            this.titlePanel.Controls.Add(this.logoPicture);
            this.titlePanel.Controls.Add(this.formTitle);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(634, 46);
            this.titlePanel.TabIndex = 2;
            // 
            // logoPicture
            // 
            this.logoPicture.BackColor = System.Drawing.SystemColors.Control;
            this.logoPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logoPicture.Image = global::UnityVSModuleBuilder.GUI.Properties.Resources.Logo50JPG;
            this.logoPicture.Location = new System.Drawing.Point(3, 3);
            this.logoPicture.Name = "logoPicture";
            this.logoPicture.Size = new System.Drawing.Size(35, 35);
            this.logoPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPicture.TabIndex = 3;
            this.logoPicture.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.button2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 401);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(634, 46);
            this.panel3.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button2.BackColor = System.Drawing.SystemColors.Control;
            this.button2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(546, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 27);
            this.button2.TabIndex = 0;
            this.button2.Text = "Build";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.BuildButtonClicked);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(912, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 34);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(274, 108);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(275, 36);
            this.panel2.TabIndex = 10;
            // 
            // flpkrRepoLocation
            // 
            this.flpkrRepoLocation.BackColor = System.Drawing.SystemColors.Control;
            this.flpkrRepoLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpkrRepoLocation.IsExistEnforced = true;
            this.flpkrRepoLocation.Location = new System.Drawing.Point(149, 177);
            this.flpkrRepoLocation.Name = "flpkrRepoLocation";
            this.flpkrRepoLocation.Size = new System.Drawing.Size(476, 52);
            this.flpkrRepoLocation.TabIndex = 9;
            // 
            // flpkrUnityInstall
            // 
            this.flpkrUnityInstall.BackColor = System.Drawing.SystemColors.Control;
            this.flpkrUnityInstall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpkrUnityInstall.IsExistEnforced = true;
            this.flpkrUnityInstall.Location = new System.Drawing.Point(149, 235);
            this.flpkrUnityInstall.Name = "flpkrUnityInstall";
            this.flpkrUnityInstall.Size = new System.Drawing.Size(476, 52);
            this.flpkrUnityInstall.TabIndex = 10;
            // 
            // flpkrOutput
            // 
            this.flpkrOutput.BackColor = System.Drawing.SystemColors.Control;
            this.flpkrOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpkrOutput.IsExistEnforced = true;
            this.flpkrOutput.Location = new System.Drawing.Point(149, 293);
            this.flpkrOutput.Name = "flpkrOutput";
            this.flpkrOutput.Size = new System.Drawing.Size(476, 53);
            this.flpkrOutput.TabIndex = 11;
            // 
            // BuilderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(634, 471);
            this.Controls.Add(this.formTable);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(581, 456);
            this.Name = "BuilderWindow";
            this.Text = "UVSModule Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.fieldsTable.ResumeLayout(false);
            this.fieldsTable.PerformLayout();
            this.formTable.ResumeLayout(false);
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPicture)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel formTable;
        private System.Windows.Forms.TableLayoutPanel fieldsTable;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label formTitle;
        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.PictureBox logoPicture;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.Label lblCompanyShortName;
        private System.Windows.Forms.Label lblRepoLocation;
        private System.Windows.Forms.Label lblUnityInstall;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.TextBox txtCompanyShortName;
        private System.Windows.Forms.Label lblOutputLocation;
        private System.Windows.Forms.Panel panel2;
        private FilePicker flpkrRepoLocation;
        private FilePicker flpkrUnityInstall;
        private FilePicker flpkrOutput;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        
    }
}