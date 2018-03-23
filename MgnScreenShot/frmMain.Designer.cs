namespace MgnScreenShot
{
    partial class frmMain
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
            this.tbDestinationFolder = new System.Windows.Forms.TextBox();
            this.btnSelectDestination = new System.Windows.Forms.Button();
            this.dlgSelectDestination = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.lbScreenshots = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbFormatFilename = new System.Windows.Forms.TextBox();
            this.tbPrefixFilename = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbDestinationFolder
            // 
            this.tbDestinationFolder.Location = new System.Drawing.Point(12, 35);
            this.tbDestinationFolder.Name = "tbDestinationFolder";
            this.tbDestinationFolder.Size = new System.Drawing.Size(476, 20);
            this.tbDestinationFolder.TabIndex = 0;
            // 
            // btnSelectDestination
            // 
            this.btnSelectDestination.Location = new System.Drawing.Point(494, 33);
            this.btnSelectDestination.Name = "btnSelectDestination";
            this.btnSelectDestination.Size = new System.Drawing.Size(56, 23);
            this.btnSelectDestination.TabIndex = 1;
            this.btnSelectDestination.Text = "select";
            this.btnSelectDestination.UseVisualStyleBackColor = true;
            this.btnSelectDestination.Click += new System.EventHandler(this.btnSelectDestination_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select destination folder";
            // 
            // lbScreenshots
            // 
            this.lbScreenshots.FormattingEnabled = true;
            this.lbScreenshots.Location = new System.Drawing.Point(12, 87);
            this.lbScreenshots.Name = "lbScreenshots";
            this.lbScreenshots.ScrollAlwaysVisible = true;
            this.lbScreenshots.Size = new System.Drawing.Size(276, 355);
            this.lbScreenshots.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "List of screenshots";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(305, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Prefix";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(305, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Format of files";
            // 
            // tbFormatFilename
            // 
            this.tbFormatFilename.Location = new System.Drawing.Point(308, 135);
            this.tbFormatFilename.Name = "tbFormatFilename";
            this.tbFormatFilename.Size = new System.Drawing.Size(242, 20);
            this.tbFormatFilename.TabIndex = 8;
            // 
            // tbPrefixFilename
            // 
            this.tbPrefixFilename.Location = new System.Drawing.Point(308, 87);
            this.tbPrefixFilename.Name = "tbPrefixFilename";
            this.tbPrefixFilename.Size = new System.Drawing.Size(242, 20);
            this.tbPrefixFilename.TabIndex = 9;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 459);
            this.Controls.Add(this.tbPrefixFilename);
            this.Controls.Add(this.tbFormatFilename);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbScreenshots);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectDestination);
            this.Controls.Add(this.tbDestinationFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScreenShot (magnum)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDestinationFolder;
        private System.Windows.Forms.Button btnSelectDestination;
        private System.Windows.Forms.FolderBrowserDialog dlgSelectDestination;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbScreenshots;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbFormatFilename;
        private System.Windows.Forms.TextBox tbPrefixFilename;
    }
}

