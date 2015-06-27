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
    public class EncryptedDBValue
    {
        public EncryptedDBValue(EncryptedPoint c, EncryptedPoint d)
        {

            this.C = new EncryptedPoint(c);
            this.D = new EncryptedPoint(d);
        }

        public EncryptedDBValue(EncryptedDBValue edbv)
            : this(edbv.C, edbv.D)
        {
        }

        [DataMember]
        public EncryptedPoint C { get; set; }
        [DataMember]
        public EncryptedPoint D { get; set; }
    }
}
