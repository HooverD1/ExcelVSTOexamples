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
        public string Ident { get; set; }
        public string Name { get; set; }
        public double[] Data { get; set; }
        public ISampleableDistribution<double> Distribution { get; set; }
        const long datapoints = 5000;

        public EstimateInput(string Name, string ident, ISampleableDistribution<double> Distribution)
        {
            this.Name = Name;
            this.Ident = ident;
            this.Distribution = Distribution;
            this.Data = GenerateData();
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
