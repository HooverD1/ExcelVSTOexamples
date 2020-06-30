namespace HelloWorld
{
    partial class ExcelReference
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
            this.txtRange = new System.Windows.Forms.TextBox();
            this.btnGotRange = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtRange
            // 
            this.txtRange.Location = new System.Drawing.Point(13, 13);
            this.txtRange.Name = "txtRange";
            this.txtRange.Size = new System.Drawing.Size(393, 22);
            this.txtRange.TabIndex = 0;
            // 
            // btnGotRange
            // 
            this.btnGotRange.Location = new System.Drawing.Point(412, 11);
            this.btnGotRange.Name = "btnGotRange";
            this.btnGotRange.Size = new System.Drawing.Size(25, 23);
            this.btnGotRange.TabIndex = 1;
            this.btnGotRange.Text = "R";
            this.btnGotRange.UseVisualStyleBackColor = true;
            this.btnGotRange.Click += new System.EventHandler(this.btnGotRange_Click);
            // 
            // ExcelReference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 59);
            this.Controls.Add(this.btnGotRange);
            this.Controls.Add(this.txtRange);
            this.Name = "ExcelReference";
            this.Text = "ExcelReference";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGotRange;
        public System.Windows.Forms.TextBox txtRange;
    }
}