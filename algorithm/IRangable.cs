using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public interface IRangable : IDistancable
    {
        public double Range { get; set; }
    }
}
