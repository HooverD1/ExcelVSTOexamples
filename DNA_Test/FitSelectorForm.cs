using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNA_Test
{
    public partial class FitSelectorForm : Form
    {
        public FitSelectorForm(List<OptimizationResult> fittedResults)
        {
            InitializeComponent();
            //Load the fit options off the parameter
            
            OptimizationResult[] enumeratedResults = fittedResults.OrderByDescending(x => x.Score).ToArray();
            IEnumerable<OptimizationResult> restrictedResults;
            if (enumeratedResults.Length > 5)
            {
                double fifthScore = enumeratedResults[4].Score;
                if(fifthScore < 0.8)
                {
                    //Show top five if their fit falls off quickly
                    restrictedResults = from OptimizationResult or in enumeratedResults where or.Score > fifthScore select or;
                }
                else
                {
                    //Show everything above 0.8
                    restrictedResults = from OptimizationResult or in enumeratedResults where or.Score >= 0.8 select or;
                }
                
            }
            else
            {
                //Show everything if there are less than 5
                restrictedResults = enumeratedResults;
            }

            foreach (OptimizationResult result in restrictedResults)
            {
                this.listBox_FitOptions.Items.Add(result.ToString());
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_SelectFit_Click(object sender, EventArgs e)
        {
            //What happens here?
            
        }
    }
}
