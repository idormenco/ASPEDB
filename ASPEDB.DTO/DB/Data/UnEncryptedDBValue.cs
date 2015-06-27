using System;
using System.Runtime.Serialization;

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
