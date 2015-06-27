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

        public EncryptedPoint(decimal[] _pa,decimal[] _pb)
        {
            this.pa = new decimal[_pa.Length];
            this.pb = new decimal[_pb.Length];
            for (int i = 0; i < _pa.Length; i++)
            {
                this.pa[i] = _pa[i];
                this.pb[i] = _pb[i];
            }
        }

        public EncryptedPoint(EncryptedPoint ep)
            : this(ep.pa, ep.pb)
        {
        }

        [DataMember]
        public decimal[] pa { get; set; }
        [DataMember]
        public decimal[] pb { get; set; }
    }
}
