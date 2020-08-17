using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics;
using MStats = MathNet.Numerics.Statistics;
using System.Windows.Forms;

namespace HelloWorld
{
    public class Correlation                //This may be good as a utility class?
    {
        public int ID { get; set; }
        public EstimateInput input1 { get; set; }
        public EstimateInput input2 { get; set; }
        public double Coefficient { get; set; }
        public double CoefMin { get; set; }
        public double CoefMax { get; set; }
        public CorrelationMatrix Parent { get; set; }
        public double TransitivityMax { get; set; }
        public double TransitivityMin { get; set; }

        public Correlation(CorrelationMatrix Parent, EstimateInput input1, EstimateInput input2)
        {
            this.Parent = Parent;
            this.input1 = input1;
            this.input2 = input2;
            this.Coefficient = CalculateCoefficient();
            this.CoefMin = CalculateMin();
            this.CoefMax = CalculateMax();
        }
        public Correlation(CorrelationMatrix Parent, EstimateInput input1, EstimateInput input2, double coef) //allows for building these after mass-calculating a correl coef matrix
        {
            this.Parent = Parent;
            this.input1 = input1;
            this.input2 = input2;
            this.Coefficient = coef;
            this.CoefMin = CalculateMin();
            this.CoefMax = CalculateMax();
        }
        public void CalculateTransitivity()
        {
            this.TransitivityMax = this.FindTransitivityMax();
            this.TransitivityMin = this.FindTransitivityMin();
        }
        public double FindTransitivityMin()
        {
            if (this.input1 == this.input2)
                return 1;
            IEnumerable<Correlation> intermediates = GetIntermediates();
            double true_min = -1;
            foreach(Correlation intermediate in intermediates.Where(x=>x.input1 == this.input1))      //first path
            {
                double this_min = (from Correlation i in intermediates      //second paths
                                   where i.input2 == this.input2
                                   select (i.Coefficient * intermediate.Coefficient)-Math.Sqrt(1-Math.Pow(intermediate.Coefficient,2)*(1-Math.Pow(i.Coefficient,2)))).Max();
                if (this_min > true_min)
                    true_min = this_min;
            }
            return true_min;
        }

        public double FindTransitivityMax()
        {
            if (this.input1 == this.input2)
                return 1;
            IEnumerable<Correlation> intermediates = GetIntermediates();
            double true_max = 1;
            foreach (Correlation intermediate in intermediates.Where(x=>x.input1 == this.input1))   //get first inputs
            {
                //this is coming up empty...
                double this_max = (from Correlation i in intermediates    //get second inputs
                                   where i.input2 == this.input2
                                   select (i.Coefficient * intermediate.Coefficient) + Math.Sqrt(1 - Math.Pow(intermediate.Coefficient, 2) * (1 - Math.Pow(i.Coefficient, 2)))).Min();
                if (this_max < true_max)
                    true_max = this_max;
            }
            return true_max;
        }
        private IEnumerable<Correlation> GetIntermediates()
        {
            List<string> inputNames = new List<string>();
            inputNames = (from Correlation correl in Parent.Correlations
                          select correl.input1.Name).Distinct().ToList();
            //correlation object represents the start (input1) and end (input2). 
            //Create a list of intermediates
            //LINQ query that list to find the intermediates
            return from Correlation correl in Parent.Correlations      //All the intermediates for this correlation
                            where (correl.input1.Ident == this.input1.Ident || correl.input2.Ident == this.input2.Ident) &&
                               !(correl.input1.Ident == this.input1.Ident && correl.input2.Ident == this.input2.Ident) &&
                                (correl.input1.Ident != correl.input2.Ident)
                            select correl;
        }
        private double CalculateTransValue()
        {
            return 0;
        }
        public double CalculateMin()
        {
            double[] data1 = new double[input1.Data.Length];
            input1.Data.CopyTo(data1, 0);
            double[] data2 = new double[input1.Data.Length];
            input2.Data.CopyTo(data2, 0);

            Array.Sort(data1);
            Array.Sort(data2);
            Array.Reverse(data2);

            double min_sum = 0;
            for(int i = 0; i < input1.Data.Length; i++)
            {
                min_sum += data1[i] * data2[i];
            }
            return min_sum / input1.Data.Length;
        }
        public double CalculateMax()
        {
            double[] data1 = new double[input1.Data.Length];
            input1.Data.CopyTo(data1, 0);
            double[] data2 = new double[input1.Data.Length];
            input2.Data.CopyTo(data2, 0);

            Array.Sort(data1);
            Array.Sort(data2);

            double max_sum = 0;
            for (int i = 0; i < input1.Data.Length; i++)
            {
                max_sum += data1[i] * data2[i];
            }
            return max_sum / input1.Data.Length;
        }
        public double CalculateCoefficient()    //for two series at a time
        {
            return MStats.Correlation.Pearson(input1.Data, input2.Data);
        }
    }
}
