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
        double GetValue(double dt);
        double GetR2();
        double GetMeanSquareError();
    }
}
