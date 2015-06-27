using ASPEDB.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPEDB.Utils;
using ASPEDB.DTO.Query;
using ASPEDB.DTO.DB;
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
            Random rand = new Random();
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
            for (int i = sk.s.Length - sk.d - 1; i < sk.dPrim; i++)
            {
                if (sk.s[i] == '0')
                {
                    pTilda[i] = rand.Next(255);
                    if (k != i)
                    {
                        sum += sk.Wds[i + 1] * pTilda[i];
                    }
                }
                else
                {
                    pTilda[i] = sk.Wds[i + 1];
                }
            }

            pTilda[k] = (decimal)(-sum / sk.Wds[k + 1]);
            pTilda = pTilda.Permute(sk.Permutation);
            for (int i = 0; i < pTilda.Length; i++)
            {
                int ri = rand.Next(255);
                if (sk.s[i] == '1')
                {
                    encPoint.pa[i] = pTilda[i] + ri;//sk.R[i];
                    encPoint.pb[i] = pTilda[i] - ri; //sk.R[i];
                }
                else
                {
                    encPoint.pa[i] = pTilda[i] - ri; //sk.R[i];
                    encPoint.pb[i] = pTilda[i] + ri; //sk.R[i];
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
            for (int i = sk.s.Length - sk.d - 1; i < sk.dPrim; i++)
            {
                if (sk.s[i] == '1')
                {
                    qTilda[i] = rand.Next(255);
                    if (k != i)
                    {
                        sum += sk.Wds[i + 1] * qTilda[i];
                    }
                }
                else
                {
                    qTilda[i] = sk.Wds[i + 1];
                }
            }
            qTilda[k] = (decimal)(-sum / sk.Wds[k + 1]);
            qTilda = qTilda.Permute(sk.Permutation);
            for (int i = 0; i < qTilda.Length; i++)
            {
                encQuery.qa[i] = qTilda[i];
                encQuery.qb[i] = qTilda[i];
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
            pHatb = pHatb.Permute(sk.Permutation.Transpose());
            pHata = pHata.ReduceDimension(sk.d);
            pHatb = pHatb.ReduceDimension(sk.d);
            for (int i = 0; i < pHata.Length; i++)
            {
                point.p[i] = (pHata[i] + pHatb[i]) / 2;
            }
            return point.RoundValues(sk.epsilon);
        }

        public EncryptedDBValue EncryptDBValue(UnEncryptedDBValue dbValue)
        {
            EncryptedDBValue ecdbv = new EncryptedDBValue(this.Enc(dbValue.C), this.Enc(dbValue.D));
            return ecdbv;
        }

        public UnEncryptedDBValue DecryptDBValue(EncryptedDBValue dbValue)
        {
            UnEncryptedDBValue unedbv = new UnEncryptedDBValue(this.Dec(dbValue.C), this.Dec(dbValue.D));
            return unedbv;
        }
        public EncryptedDBPoint EncryptDBPoint(UnEncryptedDBPoint uedbp)
        {
            EncryptedDBPoint edbp = new EncryptedDBPoint(this.EncryptDBValue(uedbp.Type),
                this.EncryptDBValue(uedbp.Name),
                this.EncryptDBValue(uedbp.Value));
            return edbp;
        }
        public UnEncryptedDBPoint DecryptDBPoint(EncryptedDBPoint edbp)
        {
            UnEncryptedDBPoint uedbp = new UnEncryptedDBPoint(this.DecryptDBValue(edbp.Type), this.DecryptDBValue(edbp.Name), this.DecryptDBValue(edbp.Value));
            return uedbp;
        }

        public EncryptedDBPoint EncryptDBPoint(DBPoint dbPoint)
        {
            EncryptedDBPoint edbp = new EncryptedDBPoint(this.EncryptDBValue(DBPointsUtils.GenerateUnEncryptedDBValue(dbPoint.Type, sk.d)),
                this.EncryptDBValue(DBPointsUtils.GenerateUnEncryptedDBValue(dbPoint.Name, sk.d)),
                this.EncryptDBValue(DBPointsUtils.GenerateUnEncryptedDBValue(dbPoint.Value, sk.d)));
            return edbp;
        }
        public DBPoint DecryptDBPointToValue(EncryptedDBPoint edbp)
        {
            UnEncryptedDBPoint uedbp = new UnEncryptedDBPoint(this.DecryptDBValue(edbp.Type), this.DecryptDBValue(edbp.Name), this.DecryptDBValue(edbp.Value));
            return uedbp.RecoverDBPointValue(sk.epsilon);
        }
    }
}
