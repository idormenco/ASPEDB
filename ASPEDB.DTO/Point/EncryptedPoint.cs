using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.DTO
{
    [Serializable]
    [DataContract]
    public class EncryptedPoint
    {
        public EncryptedPoint(int dPrim)
        {
            this.pa = new decimal[dPrim];
            this.pb = new decimal[dPrim];
        }
        [DataMember]
        public decimal[] pa { get; set; }
        [DataMember]
        public decimal[] pb { get; set; }
    }
}
