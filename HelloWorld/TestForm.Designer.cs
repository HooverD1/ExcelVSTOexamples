namespace HelloWorld
{
    partial class TestForm
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
            this.lblTestLabel = new System.Windows.Forms.Label();
            this.btnSelectRange = new System.Windows.Forms.Button();
            this.txtRangeSelect = new System.Windows.Forms.TextBox();
            this.btnOkay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTestLabel
            // 
            this.lblTestLabel.AutoSize = true;
            this.lblTestLabel.Location = new System.Drawing.Point(12, 9);
            this.lblTestLabel.Name = "lblTestLabel";
            this.lblTestLabel.Size = new System.Drawing.Size(53, 17);
            this.lblTestLabel.TabIndex = 0;
            this.lblTestLabel.Text = "Default";
            // 
            // btnSelectRange
            // 
            this.btnSelectRange.Location = new System.Drawing.Point(389, 41);
            this.btnSelectRange.Name = "btnSelectRange";
            this.btnSelectRange.Size = new System.Drawing.Size(38, 23);
            this.btnSelectRange.TabIndex = 1;
            this.btnSelectRange.Text = "R";
            this.btnSelectRange.UseVisualStyleBackColor = true;
            this.btnSelectRange.Click += new System.EventHandler(this.btnSelectRange_Click);
            // 
            // txtRangeSelect
            // 
            this.txtRangeSelect.Location = new System.Drawing.Point(15, 41);
            this.txtRangeSelect.Name = "txtRangeSelect";
            this.txtRangeSelect.Size = new System.Drawing.Size(368, 22);
            this.txtRangeSelect.TabIndex = 2;
            // 
            // btnOkay
            // 
            this.btnOkay.Location = new System.Drawing.Point(279, 82);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(104, 55);
            this.btnOkay.TabIndex = 3;
            this.btnOkay.Text = "Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 149);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.txtRangeSelect);
            this.Controls.Add(this.btnSelectRange);
            this.Controls.Add(this.lblTestLabel);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTestLabel;
        private System.Windows.Forms.Button btnSelectRange;
        public System.Windows.Forms.TextBox txtRangeSelect;
        private System.Windows.Forms.Button btnOkay;
    }
}