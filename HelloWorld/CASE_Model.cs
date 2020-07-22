using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class CASE_Model
    {
        public FileSheet fileSheet { get; set; }
        public DataSheet dataSheet { get; set; }
        public TotalSheet totalSheet { get; set; }
        public List<WBSsheet> WBSsheets { get; set; }
        public List<EstimateSheet> EstimateSheets { get; set; }

    }
}
