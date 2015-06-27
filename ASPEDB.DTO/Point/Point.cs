using System;
using System.Runtime.Serialization;

namespace ASPEDB.DTO
{
    [Serializable]
    [DataContract]
    public class Point
    {
        [DataMember]
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

        public Point(Point p)
            : this(p.p)
        {
        }
    }
}
