using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public class Data : ICorrelParent
    {
        public string Ident { get; set; }
        public List<ICorrelChild> Children { get; set; }
        public CorrelationMatrix CorrelMatrix { get; set; }
        public string CreateCorrelString() { throw new NotImplementedException(); }
    }
}
