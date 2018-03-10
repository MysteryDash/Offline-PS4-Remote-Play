namespace Dash.RemotePlayPatcher
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.currentOperationBox = new System.Windows.Forms.GroupBox();
            this.currentOperationLabel = new System.Windows.Forms.Label();
            this.patchButton = new System.Windows.Forms.Button();
            this.authorLinkLabel = new Dash.Windows.Forms.AutoLinkLabel();
            this.autoLinkLabel1 = new Dash.Windows.Forms.AutoLinkLabel();
            this.currentOperationBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // currentOperationBox
            // 
            this.currentOperationBox.Controls.Add(this.currentOperationLabel);
            this.currentOperationBox.Location = new System.Drawing.Point(12, 12);
            this.currentOperationBox.Name = "currentOperationBox";
            this.currentOperationBox.Size = new System.Drawing.Size(306, 51);
            this.currentOperationBox.TabIndex = 2;
            this.currentOperationBox.TabStop = false;
            this.currentOperationBox.Text = "Current Operation";
            // 
            // currentOperationLabel
            // 
            this.currentOperationLabel.Location = new System.Drawing.Point(6, 16);
            this.currentOperationLabel.Name = "currentOperationLabel";
            this.currentOperationLabel.Size = new System.Drawing.Size(294, 32);
            this.currentOperationLabel.TabIndex = 0;
            this.currentOperationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // patchButton
            // 
            this.patchButton.Enabled = false;
            this.patchButton.Location = new System.Drawing.Point(21, 69);
            this.patchButton.Name = "patchButton";
            this.patchButton.Size = new System.Drawing.Size(291, 23);
            this.patchButton.TabIndex = 3;
            this.patchButton.Text = "Patch it !";
            this.patchButton.UseVisualStyleBackColor = true;
            this.patchButton.Click += new System.EventHandler(this.patchButton_Click);
            // 
            // authorLinkLabel
            // 
            this.authorLinkLabel.AutoSize = true;
            this.authorLinkLabel.Location = new System.Drawing.Point(12, 95);
            this.authorLinkLabel.Name = "authorLinkLabel";
            this.authorLinkLabel.Size = new System.Drawing.Size(111, 13);
            this.authorLinkLabel.TabIndex = 4;
            this.authorLinkLabel.TabStop = true;
            this.authorLinkLabel.Text = "MysteryDash\'s GitHub";
            this.authorLinkLabel.Uri = new System.Uri("https://github.com/MysteryDash/", System.UriKind.Absolute);
            // 
            // autoLinkLabel1
            // 
            this.autoLinkLabel1.AutoSize = true;
            this.autoLinkLabel1.Location = new System.Drawing.Point(214, 95);
            this.autoLinkLabel1.Name = "autoLinkLabel1";
            this.autoLinkLabel1.Size = new System.Drawing.Size(104, 13);
            this.autoLinkLabel1.TabIndex = 5;
            this.autoLinkLabel1.TabStop = true;
            this.autoLinkLabel1.Text = "Project GitHub Page";
            this.autoLinkLabel1.Uri = new System.Uri("https://github.com/MysteryDash/Offline-PS4-Remote-Play", System.UriKind.Absolute);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 117);
            this.Controls.Add(this.autoLinkLabel1);
            this.Controls.Add(this.authorLinkLabel);
            this.Controls.Add(this.patchButton);
            this.Controls.Add(this.currentOperationBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "PS4 Remote Play Patcher";
            this.Load += new System.EventHandler(this.Main_Load);
            this.currentOperationBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox currentOperationBox;
        private System.Windows.Forms.Label currentOperationLabel;
        private System.Windows.Forms.Button patchButton;
        private Windows.Forms.AutoLinkLabel authorLinkLabel;
        private Windows.Forms.AutoLinkLabel autoLinkLabel1;
    }
}

