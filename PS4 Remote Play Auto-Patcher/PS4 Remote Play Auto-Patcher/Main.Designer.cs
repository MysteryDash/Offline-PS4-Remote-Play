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
            this.patchButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // patchButton
            // 
            this.patchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.patchButton.Location = new System.Drawing.Point(12, 12);
            this.patchButton.Name = "patchButton";
            this.patchButton.Size = new System.Drawing.Size(377, 23);
            this.patchButton.TabIndex = 0;
            this.patchButton.Text = "Click me";
            this.patchButton.UseVisualStyleBackColor = true;
            this.patchButton.Click += new System.EventHandler(this.patchButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 43);
            this.Controls.Add(this.patchButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "PS4 Offline Remote Play Auto-Patcher";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button patchButton;
    }
}

