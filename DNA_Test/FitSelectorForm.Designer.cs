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
            this.flowLayoutPanel_Checkboxes = new System.Windows.Forms.FlowLayoutPanel();
            this.comboBox_PredictAt = new System.Windows.Forms.ComboBox();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.datePicker_PredictAt = new DNA_Test.DatePicker_Custom();
            this.SuspendLayout();
            // 
            // button_SelectFit
            // 
            this.button_SelectFit.Location = new System.Drawing.Point(1127, 549);
            this.button_SelectFit.Name = "button_SelectFit";
            this.button_SelectFit.Size = new System.Drawing.Size(316, 36);
            this.button_SelectFit.TabIndex = 3;
            this.button_SelectFit.Text = "Select Fit";
            this.button_SelectFit.UseVisualStyleBackColor = true;
            this.button_SelectFit.Click += new System.EventHandler(this.button_SelectFit_Click);
            // 
            // flowLayoutPanel_Charts
            // 
            this.flowLayoutPanel_Charts.Location = new System.Drawing.Point(44, 12);
            this.flowLayoutPanel_Charts.Name = "flowLayoutPanel_Charts";
            this.flowLayoutPanel_Charts.Size = new System.Drawing.Size(1074, 531);
            this.flowLayoutPanel_Charts.TabIndex = 5;
            // 
            // comboBox_DisplayCount
            // 
            this.comboBox_DisplayCount.FormattingEnabled = true;
            this.comboBox_DisplayCount.Location = new System.Drawing.Point(865, 556);
            this.comboBox_DisplayCount.Name = "comboBox_DisplayCount";
            this.comboBox_DisplayCount.Size = new System.Drawing.Size(253, 24);
            this.comboBox_DisplayCount.TabIndex = 6;
            this.comboBox_DisplayCount.SelectedIndexChanged += new System.EventHandler(this.comboBox_DisplayCount_SelectedIndexChanged);
            // 
            // flowLayoutPanel_Options
            // 
            this.flowLayoutPanel_Options.Location = new System.Drawing.Point(1124, 12);
            this.flowLayoutPanel_Options.Name = "flowLayoutPanel_Options";
            this.flowLayoutPanel_Options.Size = new System.Drawing.Size(319, 531);
            this.flowLayoutPanel_Options.TabIndex = 7;
            // 
            // flowLayoutPanel_Checkboxes
            // 
            this.flowLayoutPanel_Checkboxes.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel_Checkboxes.Name = "flowLayoutPanel_Checkboxes";
            this.flowLayoutPanel_Checkboxes.Size = new System.Drawing.Size(26, 531);
            this.flowLayoutPanel_Checkboxes.TabIndex = 0;
            // 
            // comboBox_PredictAt
            // 
            this.comboBox_PredictAt.FormattingEnabled = true;
            this.comboBox_PredictAt.Items.AddRange(new object[] {
            "Predict at Next Interval",
            "Predict at Mean",
            "Predict at Value"});
            this.comboBox_PredictAt.Location = new System.Drawing.Point(44, 556);
            this.comboBox_PredictAt.Name = "comboBox_PredictAt";
            this.comboBox_PredictAt.Size = new System.Drawing.Size(172, 24);
            this.comboBox_PredictAt.TabIndex = 8;
            this.comboBox_PredictAt.SelectedIndexChanged += new System.EventHandler(this.comboBox_PredictAt_SelectedIndexChanged);
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(222, 557);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(283, 24);
            this.elementHost1.TabIndex = 9;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.datePicker_PredictAt;
            // 
            // FitSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1455, 593);
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.comboBox_PredictAt);
            this.Controls.Add(this.flowLayoutPanel_Checkboxes);
            this.Controls.Add(this.flowLayoutPanel_Options);
            this.Controls.Add(this.comboBox_DisplayCount);
            this.Controls.Add(this.flowLayoutPanel_Charts);
            this.Controls.Add(this.button_SelectFit);
            this.Name = "FitSelectorForm";
            this.Text = "Select Desired Fit";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_SelectFit;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Charts;
        private System.Windows.Forms.ComboBox comboBox_DisplayCount;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Options;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Checkboxes;
        private System.Windows.Forms.ComboBox comboBox_PredictAt;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private DatePicker_Custom datePicker_PredictAt;
    }
}