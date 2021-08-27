using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNA_Test
{
    public interface IRegression
    {
        //Interface for whatever class ends up being used to hold the regression used for test
        //Some sort of access to parameters
        double[] xVals { get; set; }
        double[] yVals { get; set; }
        double GetValue(double dt);
        double Score();
        Accord.DoubleRange GetConfidenceInterval(double x, double alpha);
        Accord.DoubleRange GetPredictionInterval(double x, double alpha);
        string ToString();
    }
}
