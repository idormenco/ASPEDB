using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ASPEDB.DTO
{
    public class Point
    {
        public decimal[] p { get; set; }

        public Point(int d)
        {
            p = new decimal[d];
        }
        public Point(decimal[] _p)
        {
            p = new decimal[_p.Length];
            for (int i = 0; i < _p.Length; i++)
            {
                p[i] = _p[i];
            }
        }
    }
}
