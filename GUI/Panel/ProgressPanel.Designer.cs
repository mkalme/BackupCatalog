namespace GUI {
    partial class ProgressPanel {
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
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.AmountPanel = new System.Windows.Forms.Panel();
            this.AmountLabel = new System.Windows.Forms.Label();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.ProgressContainer = new System.Windows.Forms.Panel();
            this.ProgressContainerPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.AmountPanel.SuspendLayout();
            this.ProgressContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // PictureBox
            // 
            this.PictureBox.Image = global::GUI.Properties.Resources._64pxDocument;
            this.PictureBox.Location = new System.Drawing.Point(8, 46);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(20, 20);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox.TabIndex = 11;
            this.PictureBox.TabStop = false;
            // 
            // AmountPanel
            // 
            this.AmountPanel.Controls.Add(this.AmountLabel);
            this.AmountPanel.Location = new System.Drawing.Point(265, 6);
            this.AmountPanel.Name = "AmountPanel";
            this.AmountPanel.Size = new System.Drawing.Size(127, 16);
            this.AmountPanel.TabIndex = 10;
            // 
            // AmountLabel
            // 
            this.AmountLabel.AutoSize = true;
            this.AmountLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.AmountLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.AmountLabel.Location = new System.Drawing.Point(97, 0);
            this.AmountLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.AmountLabel.Name = "AmountLabel";
            this.AmountLabel.Size = new System.Drawing.Size(30, 13);
            this.AmountLabel.TabIndex = 4;
            this.AmountLabel.Text = "0 / 0";
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.HeaderLabel.Location = new System.Drawing.Point(9, 6);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(42, 13);
            this.HeaderLabel.TabIndex = 9;
            this.HeaderLabel.Text = "Update";
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.NameLabel.Location = new System.Drawing.Point(28, 49);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(0, 13);
            this.NameLabel.TabIndex = 8;
            // 
            // ProgressContainer
            // 
            this.ProgressContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ProgressContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProgressContainer.Controls.Add(this.ProgressContainerPanel);
            this.ProgressContainer.Location = new System.Drawing.Point(12, 23);
            this.ProgressContainer.Margin = new System.Windows.Forms.Padding(0);
            this.ProgressContainer.Name = "ProgressContainer";
            this.ProgressContainer.Size = new System.Drawing.Size(377, 23);
            this.ProgressContainer.TabIndex = 7;
            // 
            // ProgressContainerPanel
            // 
            this.ProgressContainerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(209)))), ((int)(((byte)(1)))));
            this.ProgressContainerPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ProgressContainerPanel.Location = new System.Drawing.Point(0, 0);
            this.ProgressContainerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ProgressContainerPanel.Name = "ProgressContainerPanel";
            this.ProgressContainerPanel.Size = new System.Drawing.Size(0, 21);
            this.ProgressContainerPanel.TabIndex = 0;
            // 
            // ProgressPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.AmountPanel);
            this.Controls.Add(this.HeaderLabel);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.ProgressContainer);
            this.Name = "ProgressPanel";
            this.Size = new System.Drawing.Size(400, 76);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.AmountPanel.ResumeLayout(false);
            this.AmountPanel.PerformLayout();
            this.ProgressContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.Panel AmountPanel;
        private System.Windows.Forms.Label AmountLabel;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Panel ProgressContainer;
        private System.Windows.Forms.Panel ProgressContainerPanel;
    }
}
