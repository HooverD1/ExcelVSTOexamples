using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics;

namespace HelloWorld
{
    public class Correlation
    {
        private EstimateInput input1 { get; set; }
        private EstimateInput input2 { get; set; }
        public double Coefficient { get; set; }
        public double CoefMin { get; set; }
        public double CoefMax { get; set; }

        public Correlation(EstimateInput input1, EstimateInput input2)
        {
            this.input1 = input1;
            this.input2 = input2;
            this.Coefficient = CalculateCoefficient();
            //this.CoefMin = CalculateMin();
            //this.CoefMax = CalculateMax();
        }
        public Correlation(EstimateInput input1, EstimateInput input2, double coef) //allows for building these after mass-calculating a correl coef matrix
        {
            this.input1 = input1;
            this.input2 = input2;
            this.Coefficient = coef;
            //this.CoefMin = CalculateMin();
            //this.CoefMax = CalculateMax();
        }
        
        public double CalculateMin()
        {
            double[] data1 = new double[input1.Data_Simulated.Length];
            input1.Data_Simulated.CopyTo(data1, 0);
            double[] data2 = new double[input1.Data_Simulated.Length];
            input2.Data_Simulated.CopyTo(data2, 0);

            Array.Sort(data1);
            Array.Sort(data2);
            Array.Reverse(data2);

            double min_sum = 0;
            for(int i = 0; i < input1.Data_Simulated.Length; i++)
            {
                min_sum += data1[i] * data2[i];
            }
            return min_sum / input1.Data_Simulated.Length;
        }
        public double CalculateMax()
        {
            double[] data1 = new double[input1.Data_Simulated.Length];
            input1.Data_Simulated.CopyTo(data1, 0);
            double[] data2 = new double[input1.Data_Simulated.Length];
            input2.Data_Simulated.CopyTo(data2, 0);

            Array.Sort(data1);
            Array.Sort(data2);

            double max_sum = 0;
            for (int i = 0; i < input1.Data_Simulated.Length; i++)
            {
                max_sum += data1[i] * data2[i];
            }
            return max_sum / input1.Data_Simulated.Length;
        }
        public double CalculateCoefficient()     //you can calculate just this correlation here
        {
            //this needs replaced with mathnet.numerics
            return ThisAddIn.MyApp.WorksheetFunction.Correl(input1.Data_Simulated, input2.Data_Simulated);  
        }
        public void Calculate()
        {
            this.Coefficient = CalculateCoefficient();
            this.CoefMin = CalculateMin();
            this.CoefMax = CalculateMax();
        }
    }
}
