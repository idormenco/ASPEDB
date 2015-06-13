using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.DTO.Query
{
    public class EncryptedQuery
    {
        public decimal[] qa { get; set; }
        public decimal[] qb { get; set; }

        public EncryptedQuery(int dp)
        {
            qa = new decimal[dp];
            qb = new decimal[dp];
        }
    }
}
