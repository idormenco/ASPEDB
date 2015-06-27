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
    public class EncryptedQuery
    {
        [DataMember]
        public decimal[] qa { get; set; }

        [DataMember]
        public decimal[] qb { get; set; }

        public EncryptedQuery(int dp)
        {
            qa = new decimal[dp];
            qb = new decimal[dp];
        }
        public EncryptedQuery(decimal[] qa, decimal[] qb)
        {
            this.qa = new decimal[qa.Length];
            this.qb = new decimal[qb.Length];
            for (int i = 0; i < qa.Length; i++)
            {
                this.qa[i] = qa[i];
                this.qb[i] = qb[i];
            }
        }
        public EncryptedQuery(EncryptedQuery eq)
            : this(eq.qa, eq.qb)
        {
        }
    }
}
