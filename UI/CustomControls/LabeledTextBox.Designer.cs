namespace InventoryScanner.UI.CustomControls
{
    partial class LabeledTextBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TextBoxLabel = new System.Windows.Forms.Label();
            this.TextBoxControl = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TextBoxLabel
            // 
            this.TextBoxLabel.AutoSize = true;
            this.TextBoxLabel.Location = new System.Drawing.Point(3, 0);
            this.TextBoxLabel.Name = "TextBoxLabel";
            this.TextBoxLabel.Size = new System.Drawing.Size(50, 13);
            this.TextBoxLabel.TabIndex = 0;
            this.TextBoxLabel.Text = "labelText";
            // 
            // TextBoxControl
            // 
            this.TextBoxControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxControl.Location = new System.Drawing.Point(6, 16);
            this.TextBoxControl.Name = "TextBoxControl";
            this.TextBoxControl.Size = new System.Drawing.Size(191, 20);
            this.TextBoxControl.TabIndex = 1;
            // 
            // LabeledTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TextBoxControl);
            this.Controls.Add(this.TextBoxLabel);
            this.Name = "LabeledTextBox";
            this.Size = new System.Drawing.Size(204, 38);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TextBoxLabel;
        private System.Windows.Forms.TextBox TextBoxControl;
    }
}
