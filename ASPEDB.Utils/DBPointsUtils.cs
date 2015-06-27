using ASPEDB.DTO;
using ASPEDB.DTO.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.Utils
{
    public static class DBPointsUtils
    {
        public static UnEncryptedDBValue HidePoint(this Point p)
        {
            Random r = new Random();
            int s = 0;
            do
            {
                s = r.Next(255);
            } while (s == 0);
            decimal[] c = new decimal[p.p.Length];
            decimal[] d = new decimal[p.p.Length];
            for (int i = 0; i < p.p.Length; i++)
            {
                c[i] = p.p[i] - (decimal)s;
                d[i] = p.p[i] + (decimal)s;
            }
            return new UnEncryptedDBValue(c, d);
        }

        public static Point RecoverPoint(this UnEncryptedDBValue unEncryptedDBValue, decimal epsilon)
        {
            Point p = new Point(unEncryptedDBValue.C.p.Length);
            for (int i = 0; i < unEncryptedDBValue.C.p.Length; i++)
            {
                p.p[i] = (unEncryptedDBValue.C.p[i] + unEncryptedDBValue.D.p[i]) / (decimal)2;
            }
            return p.RoundValues(epsilon);
        }

        public static DBPoint RecoverDBPointValue(this UnEncryptedDBPoint uedbp, decimal epsilon)
        {
            decimal type = uedbp.Type.RecoverPoint(epsilon).RecoverValue();
            decimal name = uedbp.Name.RecoverPoint(epsilon).RecoverValue();
            decimal value = uedbp.Value.RecoverPoint(epsilon).RecoverValue();
            return new DBPoint(type, name, value);
        }
        public static decimal RecoverValue(this Point unencryptedPoint)
        {
            return unencryptedPoint.p[unencryptedPoint.p.Length - 1];
        }

        public static UnEncryptedDBValue GenerateUnEncryptedDBValue(decimal value, int d)
        {
            Point p = new Point(value.GeneratePointFromValue(d));
            return new UnEncryptedDBValue(p.HidePoint());
        }

        public static UnEncryptedDBPoint GenerateUnEncryptedDBPoint(decimal type, decimal name, decimal value, int d)
        {
            Point pType = new Point(type.GeneratePointFromValue(d));
            Point pName = new Point(name.GeneratePointFromValue(d));
            Point pValue = new Point(value.GeneratePointFromValue(d));
            return new UnEncryptedDBPoint(pType.HidePoint(), pName.HidePoint(), pValue.HidePoint());
        }
        public static UnEncryptedDBPoint GenerateUnEncryptedDBPoint(DBPoint dbp, int d)
        {
            Point pType = new Point(dbp.Type.GeneratePointFromValue(d));
            Point pName = new Point(dbp.Name.GeneratePointFromValue(d));
            Point pValue = new Point(dbp.Value.GeneratePointFromValue(d));
            return new UnEncryptedDBPoint(pType.HidePoint(), pName.HidePoint(), pValue.HidePoint());
        }

    }
}

