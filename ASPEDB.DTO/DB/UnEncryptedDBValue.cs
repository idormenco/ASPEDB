using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.DTO.DB
{
    [Serializable]
    [DataContract]
    public class UnEncryptedDBValue
    {
        public UnEncryptedDBValue(decimal[] c, decimal[] d)
        {
            this.C = new Point(c);
            this.D = new Point(d);
        }

        public UnEncryptedDBValue(Point c, Point d)
            : this(c.p, d.p)
        {
        }

        public UnEncryptedDBValue(UnEncryptedDBValue uedbv)
            : this(uedbv.C, uedbv.D)
        {
        }

        [DataMember]
        public Point C { get; set; }
        [DataMember]
        public Point D { get; set; }
    }
}
