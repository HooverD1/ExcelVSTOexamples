using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Distributions.Univariate;

namespace HelloWorld
{
    public class Estimate : ICorrelParent, ICorrelChild
    {
        public string Ident {get;set;}
        public EstimateSheet SheetParent { get; set; }
        public CorrelationSheet CorrelParent { get; set; }
        public ICorrelParent Parent { get; set; }
        public List<ICorrelChild> Children { get; set; }
        public CorrelationMatrix CorrelMatrix { get; set; }

        public Estimate(EstimateSheet SheetParent, CorrelationSheet CorrelParent, int input_count=0)
        {
            this.SheetParent = SheetParent;
            this.CorrelParent = CorrelParent;
            //Inputs = GetFakeEstimateInputs(input_count);    //these will get set up off the sheet by the user
        }
        

    }
}
