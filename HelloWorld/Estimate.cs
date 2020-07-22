using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Distributions.Univariate;

namespace HelloWorld
{
    public class Estimate
    {
        public EstimateSheet SheetParent { get; set; }
        public CorrelationSheet CorrelParent { get; set; }
        public List<EstimateInput> Inputs { get; set; }
        public CorrelationMatrix CorrelMatrix { get; set; }

        public Estimate(EstimateSheet SheetParent, CorrelationSheet CorrelParent, int input_count)
        {
            this.SheetParent = SheetParent;
            this.CorrelParent = CorrelParent;
            Inputs = GetFakeEstimateInputs(input_count);
            CorrelMatrix = new CorrelationMatrix(this);
        }
        public List<EstimateInput> GetFakeEstimateInputs(int input_count)
        {
            List<EstimateInput> returnInputs = new List<EstimateInput>();
            for(int i = 0; i < input_count; i++)
            {
                returnInputs.Add(new EstimateInput($"Input{i}", new NormalDistribution()));            
            }
            return returnInputs;
        }
    }
}
