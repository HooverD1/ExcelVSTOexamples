using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class EstimateInput
    {
        public int InputID { get; set; }
        public double[] Data_Simulated { get; set; }

        public EstimateInput(double[] input_data)
        {
            this.Data_Simulated = input_data;
        }
        public double[] GenerateData()
        {
            throw new NotImplementedException();
        }

    }
}
