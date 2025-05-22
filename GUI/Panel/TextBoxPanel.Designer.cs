namespace GUI {
    partial class TextBoxPanel {
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
            this.Panel = new System.Windows.Forms.Panel();
            this.TextBox = new System.Windows.Forms.TextBox();
            this.Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel
            // 
            this.Panel.BackColor = System.Drawing.Color.Transparent;
            this.Panel.Controls.Add(this.TextBox);
            this.Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel.Location = new System.Drawing.Point(0, 0);
            this.Panel.Margin = new System.Windows.Forms.Padding(0);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(265, 21);
            this.Panel.TabIndex = 0;
            // 
            // TextBox
            // 
            this.TextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.TextBox.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.TextBox.Location = new System.Drawing.Point(0, 0);
            this.TextBox.Margin = new System.Windows.Forms.Padding(0);
            this.TextBox.Name = "TextBox";
            this.TextBox.Size = new System.Drawing.Size(265, 16);
            this.TextBox.TabIndex = 1;
            // 
            // TextBoxPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Controls.Add(this.Panel);
            this.Name = "TextBoxPanel";
            this.Size = new System.Drawing.Size(265, 21);
            this.Load += new System.EventHandler(this.TextBoxPanelNew_Load);
            this.Resize += new System.EventHandler(this.TextBoxPanelNew_Resize);
            this.Panel.ResumeLayout(false);
            this.Panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel;
        private System.Windows.Forms.TextBox TextBox;
    }
}
