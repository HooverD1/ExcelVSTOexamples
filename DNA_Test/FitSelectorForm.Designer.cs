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
            this.listBox_FitOptions = new System.Windows.Forms.ListBox();
            this.listBox_FitDisplayAxis = new System.Windows.Forms.ListBox();
            this.button_SelectFit = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.flowLayoutPanel_Charts = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
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
            // flowLayoutPanel_Charts
            // 
            this.flowLayoutPanel_Charts.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel_Charts.Name = "flowLayoutPanel_Charts";
            this.flowLayoutPanel_Charts.Size = new System.Drawing.Size(1251, 388);
            this.flowLayoutPanel_Charts.TabIndex = 5;
            // 
            // FitSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1392, 555);
            this.Controls.Add(this.flowLayoutPanel_Charts);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_SelectFit);
            this.Controls.Add(this.listBox_FitDisplayAxis);
            this.Controls.Add(this.listBox_FitOptions);
            this.Name = "FitSelectorForm";
            this.Text = "FitSelectorForm";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox listBox_FitOptions;
        private System.Windows.Forms.ListBox listBox_FitDisplayAxis;
        private System.Windows.Forms.Button button_SelectFit;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Charts;
    }
}