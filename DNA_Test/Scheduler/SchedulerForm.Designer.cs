﻿namespace DNA_Test.Scheduler
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
            this.button_Insert = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.dateTimePicker_Start = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_End = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButton_SpecifyDate = new System.Windows.Forms.RadioButton();
            this.radioButton_SpecifyPeriods = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_IntervalLength = new System.Windows.Forms.ComboBox();
            this.comboBox_IntervalType = new System.Windows.Forms.ComboBox();
            this.textBox_Periods = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Insert
            // 
            this.button_Insert.Location = new System.Drawing.Point(12, 266);
            this.button_Insert.Name = "button_Insert";
            this.button_Insert.Size = new System.Drawing.Size(175, 40);
            this.button_Insert.TabIndex = 2;
            this.button_Insert.Text = "Insert";
            this.button_Insert.UseVisualStyleBackColor = true;
            this.button_Insert.Click += new System.EventHandler(this.button_Insert_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(193, 266);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(130, 40);
            this.button_Cancel.TabIndex = 3;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // dateTimePicker_Start
            // 
            this.dateTimePicker_Start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_Start.Location = new System.Drawing.Point(6, 116);
            this.dateTimePicker_Start.Name = "dateTimePicker_Start";
            this.dateTimePicker_Start.Size = new System.Drawing.Size(299, 22);
            this.dateTimePicker_Start.TabIndex = 1;
            this.dateTimePicker_Start.ValueChanged += new System.EventHandler(this.dateTimePicker_Start_ValueChanged);
            // 
            // dateTimePicker_End
            // 
            this.dateTimePicker_End.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_End.Location = new System.Drawing.Point(6, 166);
            this.dateTimePicker_End.Name = "dateTimePicker_End";
            this.dateTimePicker_End.Size = new System.Drawing.Size(299, 22);
            this.dateTimePicker_End.TabIndex = 2;
            this.dateTimePicker_End.ValueChanged += new System.EventHandler(this.dateTimePicker_End_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start Date / Time";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "End Date / Time";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanel3);
            this.groupBox1.Controls.Add(this.flowLayoutPanel2);
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateTimePicker_End);
            this.groupBox1.Controls.Add(this.dateTimePicker_Start);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(311, 248);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scheduling Options";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.radioButton_SpecifyDate);
            this.flowLayoutPanel3.Controls.Add(this.radioButton_SpecifyPeriods);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(13, 68);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(292, 25);
            this.flowLayoutPanel3.TabIndex = 15;
            // 
            // radioButton_SpecifyDate
            // 
            this.radioButton_SpecifyDate.AutoSize = true;
            this.radioButton_SpecifyDate.Checked = true;
            this.radioButton_SpecifyDate.Location = new System.Drawing.Point(3, 3);
            this.radioButton_SpecifyDate.Name = "radioButton_SpecifyDate";
            this.radioButton_SpecifyDate.Size = new System.Drawing.Size(138, 21);
            this.radioButton_SpecifyDate.TabIndex = 13;
            this.radioButton_SpecifyDate.TabStop = true;
            this.radioButton_SpecifyDate.Text = "Specify End Date";
            this.radioButton_SpecifyDate.UseVisualStyleBackColor = true;
            this.radioButton_SpecifyDate.CheckedChanged += new System.EventHandler(this.radioButton_SpecifyDate_CheckedChanged);
            // 
            // radioButton_SpecifyPeriods
            // 
            this.radioButton_SpecifyPeriods.AutoSize = true;
            this.radioButton_SpecifyPeriods.Location = new System.Drawing.Point(147, 3);
            this.radioButton_SpecifyPeriods.Name = "radioButton_SpecifyPeriods";
            this.radioButton_SpecifyPeriods.Size = new System.Drawing.Size(133, 21);
            this.radioButton_SpecifyPeriods.TabIndex = 14;
            this.radioButton_SpecifyPeriods.Text = "Specify Duration";
            this.radioButton_SpecifyPeriods.UseVisualStyleBackColor = true;
            this.radioButton_SpecifyPeriods.CheckedChanged += new System.EventHandler(this.radioButton_SpecifyPeriods_CheckedChanged);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.Controls.Add(this.textBox_Periods);
            this.flowLayoutPanel2.Controls.Add(this.label4);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(6, 199);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(299, 41);
            this.flowLayoutPanel2.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(119, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 20);
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
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 21);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(299, 41);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Every";
            // 
            // comboBox_IntervalLength
            // 
            this.comboBox_IntervalLength.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBox_IntervalLength.FormattingEnabled = true;
            this.comboBox_IntervalLength.Location = new System.Drawing.Point(60, 4);
            this.comboBox_IntervalLength.Name = "comboBox_IntervalLength";
            this.comboBox_IntervalLength.Size = new System.Drawing.Size(65, 24);
            this.comboBox_IntervalLength.TabIndex = 8;
            this.comboBox_IntervalLength.SelectedValueChanged += new System.EventHandler(this.comboBox_IntervalLength_SelectedValueChanged);
            // 
            // comboBox_IntervalType
            // 
            this.comboBox_IntervalType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBox_IntervalType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_IntervalType.FormattingEnabled = true;
            this.comboBox_IntervalType.Location = new System.Drawing.Point(131, 3);
            this.comboBox_IntervalType.Name = "comboBox_IntervalType";
            this.comboBox_IntervalType.Size = new System.Drawing.Size(121, 26);
            this.comboBox_IntervalType.TabIndex = 5;
            this.comboBox_IntervalType.SelectedValueChanged += new System.EventHandler(this.comboBox_IntervalType_SelectedValueChanged);
            // 
            // textBox_Periods
            // 
            this.textBox_Periods.Location = new System.Drawing.Point(196, 3);
            this.textBox_Periods.Name = "textBox_Periods";
            this.textBox_Periods.Size = new System.Drawing.Size(100, 22);
            this.textBox_Periods.TabIndex = 9;
            this.textBox_Periods.TextChanged += new System.EventHandler(this.textBox_Periods_TextChanged);
            // 
            // SchedulerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 318);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Insert);
            this.Controls.Add(this.groupBox1);
            this.Name = "SchedulerForm";
            this.Text = "Scheduler";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_Insert;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Start;
        private System.Windows.Forms.DateTimePicker dateTimePicker_End;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_IntervalType;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox comboBox_IntervalLength;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.RadioButton radioButton_SpecifyDate;
        private System.Windows.Forms.RadioButton radioButton_SpecifyPeriods;
        private System.Windows.Forms.TextBox textBox_Periods;
    }
}