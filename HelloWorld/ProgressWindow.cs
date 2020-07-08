using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace HelloWorld
{
    public partial class ProgressWindow : Form
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int j = 0; j < 100; j++)
            {
                Thread.Sleep(100);
                backgroundWorker.ReportProgress(j);
            }
        }

        private void Calculate(int i)
        {
            double pow = Math.Pow(i, i);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            //progressBar1.Value = e.ProgressPercentage;

            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new MethodInvoker(delegate { progressBar1.Value = e.ProgressPercentage; }));
            }
            else
                progressBar1.Value = e.ProgressPercentage;

        }
    
        private void btnRun_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            backgroundWorker.RunWorkerAsync();
        }



    }
}
