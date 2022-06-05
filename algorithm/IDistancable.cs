using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorithm
{
    public interface IDistancable
    {
        public double GetDistance(IDistancable other);
        public bool IsInRange(IRangable other);
    }
}
