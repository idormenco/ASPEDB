using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.DTO
{
    public class SecretKey
    {
        public SecretKey()
        {
        }
        public SecretKey(int d, int dPrim, string s, string sw, Dictionary<int, decimal> wds, decimal[] R, decimal[][] permutation, decimal[][] _M1, decimal[][] _M2, decimal _epsilon)
        {
            this.d = d;
            this.dPrim = dPrim;
            this.s = s;
            this.sw = sw;
            this.Wds = new Dictionary<int, decimal>(wds);
            this.R = new List<decimal>(R).ToArray();
            this.Permutation = permutation;
            this.M1 = _M1;
            this.M2 = _M2;
            this.epsilon = _epsilon;
        }
        public SecretKey(SecretKey sk)
        {
            this.d = sk.d;
            this.dPrim = sk.dPrim;
            this.s = sk.s;
            this.sw = sk.sw;
            this.Wds = new Dictionary<int, decimal>(sk.Wds);
            this.R = new List<decimal>(sk.R).ToArray();
            this.Permutation = sk.Permutation;
            this.M1 = sk.M1;
            this.M2 = sk.M2;
            this.epsilon = sk.epsilon;
        }

        public int d { get; set; }
        public int dPrim { get; set; }

        public decimal[][] M1 { get; set; }
        public decimal[][] M2 { get; set; }

        public string s { get; set; }
        public string sw { get; set; }
        public Dictionary<int, decimal> Wds { get; set; }
        public decimal[] R { get; set; }
        public decimal[][] Permutation { get; set; }

        public decimal epsilon { get; set; }
    }
}
