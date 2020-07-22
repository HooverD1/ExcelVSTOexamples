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
                min_sum += input1.Data_Simulated[i] * input2.Data_Simulated[i];
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
                max_sum += input1.Data_Simulated[i] * input2.Data_Simulated[i];
            }
            return max_sum / input1.Data_Simulated.Length;
        }
        public double CalculateCoefficient()        //should be able to do this all at once with the full dataset in CorrelationMatrix instead and then pull it in here
        {
            //double[][] combined = { input1.Data_Simulated, input2.Data_Simulated };
            return ThisAddIn.MyApp.WorksheetFunction.Correl(input1.Data_Simulated, input2.Data_Simulated);  //this needs replaced with mathnet.numerics
        }
        public void Calculate()
        {
            this.Coefficient = CalculateCoefficient();
            this.CoefMin = CalculateMin();
            this.CoefMax = CalculateMax();
        }
    }
}
