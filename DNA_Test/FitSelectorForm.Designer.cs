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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Chart_FitDisplay = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.listBox_FitOptions = new System.Windows.Forms.ListBox();
            this.listBox_FitDisplayAxis = new System.Windows.Forms.ListBox();
            this.button_SelectFit = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_FitDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // Chart_FitDisplay
            // 
            chartArea3.Name = "ChartArea1";
            this.Chart_FitDisplay.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.Chart_FitDisplay.Legends.Add(legend3);
            this.Chart_FitDisplay.Location = new System.Drawing.Point(12, 12);
            this.Chart_FitDisplay.Name = "Chart_FitDisplay";
            series3.ChartArea = "ChartArea1";
            series3.IsVisibleInLegend = false;
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.Chart_FitDisplay.Series.Add(series3);
            this.Chart_FitDisplay.Size = new System.Drawing.Size(1251, 388);
            this.Chart_FitDisplay.TabIndex = 0;
            this.Chart_FitDisplay.Text = "chart1";
            // 
            // listBox_FitOptions
            // 
            this.listBox_FitOptions.FormattingEnabled = true;
            this.listBox_FitOptions.ItemHeight = 16;
            this.listBox_FitOptions.Location = new System.Drawing.Point(311, 407);
            this.listBox_FitOptions.Name = "listBox_FitOptions";
            this.listBox_FitOptions.Size = new System.Drawing.Size(952, 132);
            this.listBox_FitOptions.TabIndex = 1;
            this.listBox_FitOptions.SelectedIndexChanged += new System.EventHandler(this.listBox_FitOptions_SelectedIndexChanged);
            // 
            // listBox_FitDisplayAxis
            // 
            this.listBox_FitDisplayAxis.FormattingEnabled = true;
            this.listBox_FitDisplayAxis.ItemHeight = 16;
            this.listBox_FitDisplayAxis.Location = new System.Drawing.Point(12, 406);
            this.listBox_FitDisplayAxis.Name = "listBox_FitDisplayAxis";
            this.listBox_FitDisplayAxis.Size = new System.Drawing.Size(293, 132);
            this.listBox_FitDisplayAxis.TabIndex = 2;
            // 
            // button_SelectFit
            // 
            this.button_SelectFit.Location = new System.Drawing.Point(1269, 406);
            this.button_SelectFit.Name = "button_SelectFit";
            this.button_SelectFit.Size = new System.Drawing.Size(111, 58);
            this.button_SelectFit.TabIndex = 3;
            this.button_SelectFit.Text = "Select Fit";
            this.button_SelectFit.UseVisualStyleBackColor = true;
            this.button_SelectFit.Click += new System.EventHandler(this.button_SelectFit_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(1269, 480);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(111, 58);
            this.button_Cancel.TabIndex = 4;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // FitSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1392, 555);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_SelectFit);
            this.Controls.Add(this.listBox_FitDisplayAxis);
            this.Controls.Add(this.listBox_FitOptions);
            this.Controls.Add(this.Chart_FitDisplay);
            this.Name = "FitSelectorForm";
            this.Text = "FitSelectorForm";
            ((System.ComponentModel.ISupportInitialize)(this.Chart_FitDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart Chart_FitDisplay;
        private System.Windows.Forms.ListBox listBox_FitOptions;
        private System.Windows.Forms.ListBox listBox_FitDisplayAxis;
        private System.Windows.Forms.Button button_SelectFit;
        private System.Windows.Forms.Button button_Cancel;
    }
}