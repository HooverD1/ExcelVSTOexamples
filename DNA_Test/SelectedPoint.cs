using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace DNA_Test
{
    public class SelectedPoint
    {
        public Color resetColor { get; set; }
        public Color resetBorderColor { get; set; }
        public DataPoint datapoint { get; set; }
        public Series parent { get; set; }

        public SelectedPoint(DataPoint datapoint, Series parent)
        {
            this.datapoint = datapoint;
            this.parent = parent;
            resetColor = datapoint.Color;
            resetBorderColor = datapoint.BorderColor;
        }

        public void FormatSelection()
        {
            ResetSeries();
            datapoint.Color = Color.Orange;
        }

        public void LoadDataLabel(string type)
        {
            if(type == "date")
            {
                DateTime date = DateTime.FromOADate(datapoint.XValue);
                datapoint.Label = $"({date.ToString("MM/dd/yyyy")}, {Math.Round(datapoint.YValues.First(), 2)})";
                datapoint.LabelBackColor = System.Drawing.Color.White;
            }
            else
            {
                throw new Exception("Unexpected data type");
            }
        }

        public void ResetSeries()
        {
            foreach (DataPoint dp in parent.Points)
            {
                dp.Color = Color.FromArgb(0,162,232);
                dp.Label = "";
                
            }
        }
    }
}
