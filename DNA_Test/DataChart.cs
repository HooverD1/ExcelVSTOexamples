using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace DNA_Test
{
    public class DataChart : Chart
    {
        protected Title Description { get; set; }

        public virtual void PrintChartImageToFile(string path)
        {
            //Place the Description label
            this.Description.Visible = true;
            //Test if the path is valid
            //Print image
            this.SaveImage($"{path}/test_save_file.png", ChartImageFormat.Png);
            //Remove the Description label
            this.Description.Visible = false;
        }
    }
}
