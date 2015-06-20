﻿using ASPEDB.DTO;
using ASPEDB.EncryptionModule;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPEDB.Utils;
using ASPEDB.DTO.Query;
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

            List<decimal> R = new List<decimal>() { 1, 1, 1, 1, 1, 1 };
            SecretKey sk = new SecretKey(2, 6, "010", "101010", wds, R.ToArray(), Permutation,M1,M2,(decimal)Math.Pow(10,-10));
            ASPE aspe = new ASPE(sk);
            decimal[] p = new decimal[2] { 0, 1 };
            decimal[] p2 = new decimal[2] { 0, 5 };
            decimal[] q = new decimal[2] { 0, 3 };
            Point point = new Point(p);
            Point point2 = new Point(p2);
            Query query = new Query(q);
            Console.Write("point = ");
            var ep1 = aspe.Enc(point);
            var eq = aspe.Que(query);
            var ep2 = aspe.Enc(new Point(p2));
            Console.WriteLine( aspe.Dis(ep1, ep2, eq));
            Console.ReadLine();
        }
    }
}