using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNA_Test
{
    public interface IRegression
    {
        double GetValue(double dt);
        double Score();
        Accord.DoubleRange GetConfidenceInterval(double x, double alpha);
        Accord.DoubleRange GetPredictionInterval(double x, double alpha);
        string ToString();
    }
}
