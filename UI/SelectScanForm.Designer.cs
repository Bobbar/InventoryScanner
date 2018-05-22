namespace InventoryScanner.UI
{
    partial class SelectScanForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ScansCombo = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.AcceptScanButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.AcceptScanButton);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.ScansCombo);
            this.groupBox1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(617, 216);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose A Scan";
            // 
            // ScansCombo
            // 
            this.ScansCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScansCombo.FormattingEnabled = true;
            this.ScansCombo.Location = new System.Drawing.Point(32, 72);
            this.ScansCombo.Name = "ScansCombo";
            this.ScansCombo.Size = new System.Drawing.Size(552, 27);
            this.ScansCombo.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(28, 50);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(135, 19);
            this.label14.TabIndex = 5;
            this.label14.Text = "Previous Scans";
            // 
            // AcceptScanButton
            // 
            this.AcceptScanButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AcceptScanButton.BackColor = System.Drawing.Color.DimGray;
            this.AcceptScanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AcceptScanButton.Location = new System.Drawing.Point(153, 133);
            this.AcceptScanButton.Name = "AcceptScanButton";
            this.AcceptScanButton.Size = new System.Drawing.Size(310, 44);
            this.AcceptScanButton.TabIndex = 10;
            this.AcceptScanButton.Text = "Accept";
            this.AcceptScanButton.UseVisualStyleBackColor = false;
            this.AcceptScanButton.Click += new System.EventHandler(this.AcceptScanButton_Click);
            // 
            // SelectScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.ClientSize = new System.Drawing.Size(641, 240);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(657, 279);
            this.Name = "SelectScanForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Previous Scan";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox ScansCombo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button AcceptScanButton;
    }
}