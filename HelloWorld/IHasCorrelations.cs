using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public interface IHasCorrelations
    {
        CorrelationMatrix CorrelMatrix { get; set; }
        List<EstimateInput> Inputs { get; set; }        //need to make EstimateInput replacement interface
    }
}
