using ASPEDB.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.Utils
{
    public static class Printer
    {
        public static void Print(this Point point)
        {
            if (point == null || point.p.Length == 0) throw new Exception("Null point!");
            foreach (var p in point.p)
            {
                Console.Write(p.ToString() + " ");
            }
            Console.WriteLine();
        }

        public static void Print(this decimal[] point)
        {
            if (point == null || point.Length == 0) throw new Exception("Null point!");
            foreach (var p in point)
            {
                Console.Write(p.ToString() + " ");
            }
            Console.WriteLine();
        }
        public static void Print(this decimal[][] matrix)
        {
            if (matrix == null || matrix.Length == 0) throw new Exception("Null point!");
            foreach (var line in matrix)
            {
                foreach (var m in line)
                {
                    Console.Write(m.ToString("#.###") + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
