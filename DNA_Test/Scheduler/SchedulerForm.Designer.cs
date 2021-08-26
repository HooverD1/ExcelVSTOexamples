namespace DNA_Test.Scheduler
{
    partial class SchedulerForm
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
            this.button_InsertMidpoints = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.elementHost2 = new System.Windows.Forms.Integration.ElementHost();
            this.datePicker_Custom2 = new DNA_Test.DatePicker_Custom();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.datePicker_Custom1 = new DNA_Test.DatePicker_Custom();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.textBox_Periods = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_IntervalLength = new System.Windows.Forms.ComboBox();
            this.comboBox_IntervalType = new System.Windows.Forms.ComboBox();
            this.button_InsertEndpoints = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_InsertMidpoints
            // 
            this.button_InsertMidpoints.Location = new System.Drawing.Point(14, 316);
            this.button_InsertMidpoints.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_InsertMidpoints.Name = "button_InsertMidpoints";
            this.button_InsertMidpoints.Size = new System.Drawing.Size(150, 48);
            this.button_InsertMidpoints.TabIndex = 2;
            this.button_InsertMidpoints.Text = "Insert Midpoints";
            this.button_InsertMidpoints.UseVisualStyleBackColor = true;
            this.button_InsertMidpoints.Click += new System.EventHandler(this.button_InsertMidpoints_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start Date / Time";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "End Date / Time";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.elementHost2);
            this.groupBox1.Controls.Add(this.elementHost1);
            this.groupBox1.Controls.Add(this.flowLayoutPanel2);
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(14, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(307, 294);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scheduling Options";
            // 
            // elementHost2
            // 
            this.elementHost2.Location = new System.Drawing.Point(6, 187);
            this.elementHost2.Name = "elementHost2";
            this.elementHost2.Size = new System.Drawing.Size(289, 27);
            this.elementHost2.TabIndex = 15;
            this.elementHost2.Text = "elementHost2";
            this.elementHost2.Child = this.datePicker_Custom2;
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(6, 115);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(289, 27);
            this.elementHost1.TabIndex = 14;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.datePicker_Custom1;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.Controls.Add(this.textBox_Periods);
            this.flowLayoutPanel2.Controls.Add(this.label4);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(7, 236);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(288, 49);
            this.flowLayoutPanel2.TabIndex = 12;
            // 
            // textBox_Periods
            // 
            this.textBox_Periods.Location = new System.Drawing.Point(224, 4);
            this.textBox_Periods.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_Periods.Name = "textBox_Periods";
            this.textBox_Periods.Size = new System.Drawing.Size(61, 27);
            this.textBox_Periods.TabIndex = 9;
            this.textBox_Periods.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Periods.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBox_Periods_MouseClick);
            this.textBox_Periods.TextChanged += new System.EventHandler(this.textBox_Periods_TextChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(148, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 19);
            this.label4.TabIndex = 7;
            this.label4.Text = "Periods:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.comboBox_IntervalLength);
            this.flowLayoutPanel1.Controls.Add(this.comboBox_IntervalType);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(7, 25);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(288, 49);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Every";
            // 
            // comboBox_IntervalLength
            // 
            this.comboBox_IntervalLength.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBox_IntervalLength.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_IntervalLength.FormattingEnabled = true;
            this.comboBox_IntervalLength.Location = new System.Drawing.Point(60, 4);
            this.comboBox_IntervalLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBox_IntervalLength.Name = "comboBox_IntervalLength";
            this.comboBox_IntervalLength.Size = new System.Drawing.Size(49, 27);
            this.comboBox_IntervalLength.TabIndex = 8;
            this.comboBox_IntervalLength.SelectedValueChanged += new System.EventHandler(this.comboBox_IntervalLength_SelectedValueChanged);
            // 
            // comboBox_IntervalType
            // 
            this.comboBox_IntervalType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBox_IntervalType.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_IntervalType.FormattingEnabled = true;
            this.comboBox_IntervalType.Location = new System.Drawing.Point(115, 4);
            this.comboBox_IntervalType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBox_IntervalType.Name = "comboBox_IntervalType";
            this.comboBox_IntervalType.Size = new System.Drawing.Size(91, 27);
            this.comboBox_IntervalType.TabIndex = 5;
            this.comboBox_IntervalType.SelectedValueChanged += new System.EventHandler(this.comboBox_IntervalType_SelectedValueChanged);
            // 
            // button_InsertEndpoints
            // 
            this.button_InsertEndpoints.Location = new System.Drawing.Point(171, 317);
            this.button_InsertEndpoints.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_InsertEndpoints.Name = "button_InsertEndpoints";
            this.button_InsertEndpoints.Size = new System.Drawing.Size(150, 48);
            this.button_InsertEndpoints.TabIndex = 3;
            this.button_InsertEndpoints.Text = "Insert Endpoints";
            this.button_InsertEndpoints.UseVisualStyleBackColor = true;
            this.button_InsertEndpoints.Click += new System.EventHandler(this.button_InsertEndpoints_Click);
            // 
            // SchedulerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 378);
            this.Controls.Add(this.button_InsertEndpoints);
            this.Controls.Add(this.button_InsertMidpoints);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SchedulerForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ROSE: Insert a Time Series";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.SchedulerForm_HelpButtonClicked);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_InsertMidpoints;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_IntervalType;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox comboBox_IntervalLength;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_Periods;
        private System.Windows.Forms.Integration.ElementHost elementHost2;
        private DatePicker_Custom datePicker_Custom2;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private DatePicker_Custom datePicker_Custom1;
        private System.Windows.Forms.Button button_InsertEndpoints;
    }
}