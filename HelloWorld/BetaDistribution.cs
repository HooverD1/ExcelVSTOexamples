using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace HelloWorld
{
    public class BetaDistribution
    {
        private double alpha_param { get; set; }
        private double beta_param { get; set; }
        private double mean { get; set; }
        private double stdev { get; set; }
        private double min { get; set; }
        private double max { get; set; }

        public double[] Beta2M(double mean, double stdev, double min, double max)
        {
            this.mean = mean;
            this.stdev = stdev;
            this.min = min;
            this.max = max;

            Excel.Range selection = ObjModel.GetSelection();
            /*
            if (selection.Height != 1 || selection.Width != 4)
            {
                MessageBox.Show("Improper selection size (1x4)");
                return null;
            }
            */
            double dA = -1;
            double dB = -1;
            dA = (mean - min) / (max - min) * ((mean - min) / (max - min) * (1 - (mean - min) / (max - min)) / (stdev * stdev / Math.Pow(max - min, 2)) - 1);
            dB = (1 - (mean - min) / (max - min)) * ((mean - min) / (max - min) * (1 - (mean - min) / (max - min)) / (Math.Pow(stdev, 2) / Math.Pow(max - min, 2)) - 1);
            double[] vOUT = new double[4];
            vOUT[0] = dA;
            vOUT[1] = dB;
            vOUT[2] = min;
            vOUT[3] = max;
            return vOUT;
        }
    }
}
