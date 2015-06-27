using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.DTO.Query
{
    [Serializable]
    [DataContract]
    public class Query
    {
        [DataMember]
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
        public Query(Query q)
            : this(q.q)
        {
        }
    }
}