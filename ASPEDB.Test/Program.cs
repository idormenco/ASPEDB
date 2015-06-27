using System;
using System.Collections.Generic;
using ASPEDB.DTO;
using ASPEDB.DTO.DB;
using ASPEDB.EncryptionModule;
using ASPEDB.Utils;

namespace ASPEDB.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            decimal[][] M1 = new decimal[6][]
            {
                new decimal[6]{9,4,3,3,8,6},
                new decimal[6]{6,6,2,0,2,7}, 
                new decimal[6]{4,4,4,6,2,0},
                new decimal[6]{5,4,4,4,3,4},
                new decimal[6]{4,9,2,6,5,4}, 
                new decimal[6]{2,3,5,1,2,0}
            };
            decimal[][] M2 = new decimal[6][]
            {
                new decimal[6]{3,1,3,2,8,4},
                new decimal[6]{4,4,5,5,0,9}, 
                new decimal[6]{2,1,6,4,8,9},
                new decimal[6]{6,7,2,8,0,7},
                new decimal[6]{0,3,5,9,3,4}, 
                new decimal[6]{4,8,5,6,1,6}
            };
            decimal[][] Permutation = new decimal[6][]
            {
                new decimal[6]{0,1,0,0,0,0},
                new decimal[6]{0,0,0,1,0,0}, 
                new decimal[6]{1,0,0,0,0,0},
                new decimal[6]{0,0,0,0,1,0},
                new decimal[6]{0,0,0,0,0,1}, 
                new decimal[6]{0,0,1,0,0,0}
            };
            Dictionary<int, decimal> wds = new Dictionary<int, decimal>();
            wds.Add(4, 8);
            wds.Add(5, 2);
            wds.Add(6, 5);

            SecretKey sk = new SecretKey(2, 6, "101010", wds, Permutation, M1, M2, (decimal)Math.Pow(10, -10));
            ASPE aspe = new ASPE(sk);

            DBPoint dbp = new DBPoint(1, 2, -3);
            var edbp = aspe.EncryptDBPoint(dbp);
            DBQuery dbq = new DBQuery(1, 2, Operator.Between, -3, 4);
            EncryptedDBQuery edbq = aspe.EncryptDBQuery(dbq);
            Console.WriteLine(edbq.DBQueryCoversDBPoint(edbp, sk.epsilon));
            Console.ReadLine();
        }
    }
}
