using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.DTO
{
    public class EncryptedPoint
    {
        public EncryptedPoint(int dPrim)
        {
            this.pa = new decimal[dPrim];
            this.pb = new decimal[dPrim];
        }
        public decimal[] pa { get; set; }
        public decimal[] pb { get; set; }
    }
}
