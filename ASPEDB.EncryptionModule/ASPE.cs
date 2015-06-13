using ASPEDB.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPEDB.Utils;
using ASPEDB.DTO.Query;
namespace ASPEDB.EncryptionModule
{
    public class ASPE
    {
        public SecretKey sk { get; set; }

        public ASPE()
        {
            sk = null;
        }

        public ASPE(SecretKey sk)
        {
            this.sk = new SecretKey(sk);
        }

        public void GenKey()
        {
        }

        public EncryptedPoint Enc(Point point)
        {
            Random r = new Random();
            if (this.sk == null) throw new Exception("No secret key sk");
            EncryptedPoint encPoint = new EncryptedPoint(sk.dPrim);
            decimal[] pTilda = new decimal[sk.dPrim];
            for (int i = 0; i < sk.d; i++)
            {
                pTilda[i] = point.p[i];
            }
            pTilda[sk.d] = point.p.AspeNorm();
            decimal sum = 0;
            int k = sk.s.LastIndexOf('0');
            for (int i = 0; i < sk.s.Length; i++)
            {
                if (sk.s[i] == '0')
                {
                    pTilda[sk.d + 1 + i] = r.Next(255);
                    if (k != i)
                    {
                        sum += sk.Wds[sk.d + 1 + (i + 1)] * pTilda[sk.d + 1 + i];
                    }
                }
                else
                {
                    pTilda[sk.d + 1 + i] = sk.Wds[sk.d + 1 + (i + 1)];
                }
            }

            pTilda[k + sk.d + 1] = (decimal)(-sum / sk.Wds[sk.d + 1 + k + 1]);
            pTilda = pTilda.Permute(sk.Permutation);
            for (int i = 0; i < pTilda.Length; i++)
            {
                if (sk.sw[i] == '1')
                {
                    encPoint.pa[i] = pTilda[i] + sk.R[i];
                    encPoint.pb[i] = pTilda[i] - sk.R[i];
                }
                else
                {
                    encPoint.pa[i] = pTilda[i] - sk.R[i];
                    encPoint.pb[i] = pTilda[i] + sk.R[i];
                }
            }
            encPoint.pa = sk.M1.Transpose().Multiply(encPoint.pa);
            encPoint.pb = sk.M2.Transpose().Multiply(encPoint.pb);
            return encPoint;
        }

        public EncryptedQuery Que(Query query)
        {
            EncryptedQuery encQuery = new EncryptedQuery(sk.dPrim);
            Random rand = new Random();
            int r = rand.Next(255);
            decimal[] qTilda = new decimal[sk.dPrim];
            for (int i = 0; i < sk.d; i++)
            {
                qTilda[i] = r * query.q[i];
            }
            qTilda[sk.d] = r;
            decimal sum = 0;
            int k = sk.s.LastIndexOf('1');
            for (int i = 0; i < sk.s.Length; i++)
            {
                if (sk.s[i] == '1')
                {
                    qTilda[sk.d + 1 + i] = rand.Next(255);
                    if (k != i)
                    {
                        sum += sk.Wds[sk.d + 1 + (i + 1)] * qTilda[sk.d + 1 + i];
                    }
                }
                else
                {
                    qTilda[sk.d + 1 + i] = sk.Wds[sk.d + 1 + (i + 1)];
                }
            }
            qTilda[k + sk.d + 1] = (decimal)(-sum / sk.Wds[sk.d + 1 + k + 1]);
            qTilda = qTilda.Permute(sk.Permutation);
            for (int i = 0; i < qTilda.Length; i++)
            {
                if (sk.sw[i] == '0')
                {
                    encQuery.qa[i] = qTilda[i] + sk.R[i];
                    encQuery.qb[i] = qTilda[i] - sk.R[i];
                }
                else
                {
                    encQuery.qa[i] = qTilda[i] - sk.R[i];
                    encQuery.qb[i] = qTilda[i] + sk.R[i];
                }
            }
            encQuery.qa = sk.M1.Inverse().Multiply(encQuery.qa);
            encQuery.qb = sk.M2.Inverse().Multiply(encQuery.qb);
            return encQuery;
        }

        public decimal Dis(EncryptedPoint p1, EncryptedPoint p2, EncryptedQuery q)
        {
            return (p1.pa.Substract(p2.pa)).Multiply(q.qa) + (p1.pb.Substract(p2.pb)).Multiply(q.qb);
        }

        public Point Dec(EncryptedPoint encryptedPoint)
        {
            Point point = new Point(sk.d);
            var pHata = sk.M1.Transpose().Inverse().Multiply(encryptedPoint.pa);
            var pHatb = sk.M2.Transpose().Inverse().Multiply(encryptedPoint.pb);
            pHata = pHata.Permute(sk.Permutation.Transpose());
            pHatb = pHatb.Permute(sk.Permutation);
            pHata = pHata.ReduceDimension(sk.d);
            for (int i = 0; i < pHata.Length; i++)
            {
                if (sk.sw[i] == '0')
                {
                    point.p[i] = pHata[i] - sk.R[i];
                }
            }
            return point.RoundValues(sk.epsilon);
        }
    }
}
