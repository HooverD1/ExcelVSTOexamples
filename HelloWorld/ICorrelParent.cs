using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public interface ICorrelParent
    {
        string Ident { get; set; }
        CorrelationMatrix CorrelMatrix { get; set; }
        List<ICorrelChild> Children { get; set; }
    }
}
