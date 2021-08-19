namespace DNA_Test
{
    partial class FitSelectorForm
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
            this.button_SelectFit = new System.Windows.Forms.Button();
            this.flowLayoutPanel_Charts = new System.Windows.Forms.FlowLayoutPanel();
            this.comboBox_DisplayCount = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel_Options = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // button_SelectFit
            // 
            this.button_SelectFit.Location = new System.Drawing.Point(1127, 507);
            this.button_SelectFit.Name = "button_SelectFit";
            this.button_SelectFit.Size = new System.Drawing.Size(253, 36);
            this.button_SelectFit.TabIndex = 3;
            this.button_SelectFit.Text = "Select Fit";
            this.button_SelectFit.UseVisualStyleBackColor = true;
            this.button_SelectFit.Click += new System.EventHandler(this.button_SelectFit_Click);
            // 
            // flowLayoutPanel_Charts
            // 
            this.flowLayoutPanel_Charts.Location = new System.Drawing.Point(34, 12);
            this.flowLayoutPanel_Charts.Name = "flowLayoutPanel_Charts";
            this.flowLayoutPanel_Charts.Size = new System.Drawing.Size(1084, 531);
            this.flowLayoutPanel_Charts.TabIndex = 5;
            // 
            // comboBox_DisplayCount
            // 
            this.comboBox_DisplayCount.FormattingEnabled = true;
            this.comboBox_DisplayCount.Location = new System.Drawing.Point(1127, 477);
            this.comboBox_DisplayCount.Name = "comboBox_DisplayCount";
            this.comboBox_DisplayCount.Size = new System.Drawing.Size(253, 24);
            this.comboBox_DisplayCount.TabIndex = 6;
            this.comboBox_DisplayCount.SelectedIndexChanged += new System.EventHandler(this.comboBox_DisplayCount_SelectedIndexChanged);
            // 
            // flowLayoutPanel_Options
            // 
            this.flowLayoutPanel_Options.Location = new System.Drawing.Point(1124, 12);
            this.flowLayoutPanel_Options.Name = "flowLayoutPanel_Options";
            this.flowLayoutPanel_Options.Size = new System.Drawing.Size(256, 459);
            this.flowLayoutPanel_Options.TabIndex = 7;
            // 
            // FitSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1392, 555);
            this.Controls.Add(this.flowLayoutPanel_Options);
            this.Controls.Add(this.comboBox_DisplayCount);
            this.Controls.Add(this.flowLayoutPanel_Charts);
            this.Controls.Add(this.button_SelectFit);
            this.Enabled = false;
            this.Name = "FitSelectorForm";
            this.Text = "Select Desired Fit";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_SelectFit;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Charts;
        private System.Windows.Forms.ComboBox comboBox_DisplayCount;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Options;
    }
}