using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DNA_Test
{
    public abstract class ScatterChart : DataChart
    {
        public Series DataSeries { get; set; } //The data (potentially bucketed)

        public double[,] DataPoints { get; set; }

        protected override void SetupXAxisGridlines() { throw new NotImplementedException(); }
    }
}
