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
    }
}
