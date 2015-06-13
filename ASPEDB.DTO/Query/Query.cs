using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.DTO.Query
{
    public class Query
    {
        public decimal[] q { get; set; }

        public Query(int d)
        {
            q = new decimal[d];
        }
        public Query(decimal[] _q)
        {
            q = new decimal[_q.Length];
            for (int i = 0; i < _q.Length; i++)
            {
                q[i] = _q[i];
            }
        }
    }
}