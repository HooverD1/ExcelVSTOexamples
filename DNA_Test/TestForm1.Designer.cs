namespace DNA_Test
{
    partial class TestForm1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chart_test = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart_test)).BeginInit();
            this.SuspendLayout();
            // 
            // chart_test
            // 
            chartArea1.Name = "ChartArea1";
            this.chart_test.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart_test.Legends.Add(legend1);
            this.chart_test.Location = new System.Drawing.Point(12, 12);
            this.chart_test.Name = "chart_test";
            this.chart_test.Size = new System.Drawing.Size(752, 426);
            this.chart_test.TabIndex = 0;
            this.chart_test.Text = "chart1";
            // 
            // TestForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chart_test);
            this.Name = "TestForm1";
            this.Text = "TestForm1";
            ((System.ComponentModel.ISupportInitialize)(this.chart_test)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_test;
    }
}