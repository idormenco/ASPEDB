using System;
using System.ComponentModel.Design;
using ASPEDB.DTO;
using ASPEDB.DTO.DB;

namespace ASPEDB.Utils
{
    public static class Comparer
    {
        public static bool DBQueryCoversDBPoint(this EncryptedDBQuery edbq, EncryptedDBPoint edbp, decimal epsilon)
        {
            if (edbq.Type.QueryCovers(edbp.Type, Operator.Equal, epsilon))
            {
                if (edbq.Name.QueryCovers(edbp.Name, Operator.Equal, epsilon))
                {
                    switch (edbq.Operator)
                    {
                        case Operator.Equal:
                        case Operator.Greater:
                        case Operator.GreaterEqual:
                        case Operator.Less:
                        case Operator.LessEqual:
                        case Operator.NotEqual:
                            return edbq.Value.QueryCovers(edbp.Value, edbq.Operator, epsilon);
                            break;
                        case Operator.Between:
                            if (edbq.OptionalValue == null) throw new Exception("Optional Value can't be null!");
                            return edbq.Value.QueryCovers(edbp.Value, Operator.GreaterEqual, epsilon)
                                && edbq.OptionalValue.QueryCovers(edbp.Value, Operator.LessEqual, epsilon);
                            break;
                        case Operator.BetweenDown:
                            if (edbq.OptionalValue == null) throw new Exception("Optional Value can't be null!");
                            return edbq.Value.QueryCovers(edbp.Value, Operator.GreaterEqual, epsilon)
                                && edbq.OptionalValue.QueryCovers(edbp.Value, Operator.Less, epsilon);
                            break;
                        case Operator.ExactBetween:
                            if (edbq.OptionalValue == null) throw new Exception("Optional Value can't be null!");
                            return edbq.Value.QueryCovers(edbp.Value, Operator.Greater, epsilon)
                                && edbq.OptionalValue.QueryCovers(edbp.Value, Operator.Less, epsilon);
                            break;
                        case Operator.BetweenUp:
                            if (edbq.OptionalValue == null) throw new Exception("Optional Value can't be null!");
                            return edbq.Value.QueryCovers(edbp.Value, Operator.Greater, epsilon)
                                && edbq.OptionalValue.QueryCovers(edbp.Value, Operator.LessEqual, epsilon);
                            break;
                    }
                }
            }
            return false;
        }

        public static bool QueryCovers(this EncryptedQuery eq, EncryptedDBValue ep, Operator op, decimal epsilon)
        {
            decimal dis = Dis(ep.C, ep.D, eq);
            decimal apDis = Math.Abs(dis) <= epsilon ? 0 : dis;
            switch (op)
            {
                case Operator.Equal:
                    return apDis == 0;
                    break;
                case Operator.Greater:
                    return apDis > 0;
                    break;
                case Operator.GreaterEqual:
                    return apDis >= 0;
                    break;
                case Operator.Less:
                    return apDis < 0;
                    break;
                case Operator.LessEqual:
                    return apDis <= 0;
                    break;
                case Operator.NotEqual:
                    return apDis != 0;
                    break;
            }
            return false;
        }

        public static decimal Dis(EncryptedPoint p1, EncryptedPoint p2, EncryptedQuery q)
        {
            return (p1.pa.Substract(p2.pa)).Multiply(q.qa) + (p1.pb.Substract(p2.pb)).Multiply(q.qb);
        }

    }
}
