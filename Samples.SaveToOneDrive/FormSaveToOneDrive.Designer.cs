namespace OneDriveSamples.SaveToOneDrive
{
    partial class FormSaveToOneDrive
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
            this.buttonUploadToOneDrive = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLocalFile = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFileIdentifer = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonUploadToOneDrive
            // 
            this.buttonUploadToOneDrive.Location = new System.Drawing.Point(294, 50);
            this.buttonUploadToOneDrive.Margin = new System.Windows.Forms.Padding(2);
            this.buttonUploadToOneDrive.Name = "buttonUploadToOneDrive";
            this.buttonUploadToOneDrive.Size = new System.Drawing.Size(112, 31);
            this.buttonUploadToOneDrive.TabIndex = 0;
            this.buttonUploadToOneDrive.Text = "Save to OneDrive";
            this.buttonUploadToOneDrive.UseVisualStyleBackColor = true;
            this.buttonUploadToOneDrive.Click += new System.EventHandler(this.buttonUploadToOneDrive_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "&File To Upload:";
            // 
            // textBoxLocalFile
            // 
            this.textBoxLocalFile.Location = new System.Drawing.Point(118, 26);
            this.textBoxLocalFile.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxLocalFile.Name = "textBoxLocalFile";
            this.textBoxLocalFile.Size = new System.Drawing.Size(288, 20);
            this.textBoxLocalFile.TabIndex = 2;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(407, 26);
            this.buttonBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(31, 20);
            this.buttonBrowse.TabIndex = 3;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 222);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(460, 23);
            this.progressBar.TabIndex = 4;
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(262, 118);
            this.buttonDownload.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(144, 31);
            this.buttonDownload.TabIndex = 6;
            this.buttonDownload.Text = "Download from OneDrive";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 97);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "File Id:";
            // 
            // textBoxFileIdentifer
            // 
            this.textBoxFileIdentifer.Location = new System.Drawing.Point(118, 94);
            this.textBoxFileIdentifer.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxFileIdentifer.Name = "textBoxFileIdentifer";
            this.textBoxFileIdentifer.Size = new System.Drawing.Size(288, 20);
            this.textBoxFileIdentifer.TabIndex = 8;
            // 
            // FormSaveToOneDrive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 245);
            this.Controls.Add(this.textBoxFileIdentifer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxLocalFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonUploadToOneDrive);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormSaveToOneDrive";
            this.Text = "Save To OneDrive";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonUploadToOneDrive;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLocalFile;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFileIdentifer;
    }
}

