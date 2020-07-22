using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Distributions;

namespace HelloWorld
{
    public class EstimateInput
    {
        public int InputID { get; set; }
        public string InputName { get; set; }
        public double[] Data_Simulated { get; set; }
        public ISampleableDistribution<double> Distribution { get; set; }
        const long datapoints = 50000;

        public EstimateInput(string InputName, ISampleableDistribution<double> Distribution)
        {
            this.InputName = InputName;
            this.Distribution = Distribution;
            this.Data_Simulated = GenerateData();
        }
        public double[] GenerateData()
        {
            double[] data = new double[datapoints];
            for(int i = 0; i < datapoints; i++)
            {
                data[i] = Distribution.Generate();
            }
            return data;
        }
    }
}
