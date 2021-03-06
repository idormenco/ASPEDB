﻿using ASPEDB.DTO;
using ASPEDB.DTO.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPEDB.Utils
{
    public static class VectorOperations
    {
        public static decimal AspeNorm(this decimal[] vector)
        {
            decimal sum = 0;
            foreach (var v in vector)
            {
                sum = sum + v * v;
            }
            return sum * -0.5m;
        }
        public static decimal[] Permute(this decimal[] vector, decimal[][] permutation)
        {
            return permutation.Multiply(vector);
        }

        static decimal[][] MatrixCreate(int rows, int cols)
        {
            // allocates/creates a matrix initialized to all 0.0. assume rows and cols > 0
            // do error checking here
            decimal[][] result = new decimal[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new decimal[cols];

            //for (int i = 0; i < rows; ++i)
            //  for (int j = 0; j < cols; ++j)
            //    result[i][j] = 0.0; // explicit initialization needed in some languages

            return result;
        }

        // --------------------------------------------------------------------------------------------------------------

        public static decimal[] Multiply(this decimal[] vector, decimal[][] matrix)
        {
            // result of multiplying an n x m matrix by a m x 1 column vector (yielding an n x 1 column vector)
            int mRows = matrix.Length; int mCols = matrix[0].Length;
            int vRows = vector.Length;
            if (mCols != vRows)
                throw new Exception("Non-conformable matrix and vector in MatrixVectorProduct");
            decimal[] result = new decimal[mRows]; // an n x m matrix times a m x 1 column vector is a n x 1 column vector
            for (int i = 0; i < mRows; ++i)
                for (int j = 0; j < mCols; ++j)
                    result[i] += matrix[i][j] * vector[j];
            return result;
        }
        // matrix-vector multiplication (y = A * x)
        public static decimal[] Multiply(this decimal[][] matrix, decimal[] vector)
        {
            int m = matrix.Length;
            int n = matrix[0].Length;
            if (vector.Length != n) throw new Exception("Illegal matrix dimensions.");
            decimal[] y = new decimal[m];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    y[i] += (matrix[i][j] * vector[j]);
            return y;
        }
        // --------------------------------------------------------------------------------------------------------------

        static decimal[][] MatrixDecompose(decimal[][] matrix, out int[] perm, out int toggle)
        {
            // Doolittle LUP decomposition with partial pivoting.
            // rerturns: result is L (with 1s on diagonal) and U; perm holds row permutations; toggle is +1 or -1 (even or odd)
            int rows = matrix.Length;
            int cols = matrix[0].Length; // assume all rows have the same number of columns so just use row [0].
            if (rows != cols)
                throw new Exception("Attempt to MatrixDecompose a non-square mattrix");

            int n = rows; // convenience

            decimal[][] result = MatrixDuplicate(matrix); // make a copy of the input matrix

            perm = new int[n]; // set up row permutation result
            for (int i = 0; i < n; ++i) { perm[i] = i; }

            toggle = 1; // toggle tracks row swaps. +1 -> even, -1 -> odd. used by MatrixDeterminant

            for (int j = 0; j < n - 1; ++j) // each column
            {
                decimal colMax = Math.Abs(result[j][j]); // find largest value in col j
                int pRow = j;
                for (int i = j + 1; i < n; ++i)
                {
                    if (result[i][j] > colMax)
                    {
                        colMax = result[i][j];
                        pRow = i;
                    }
                }

                if (pRow != j) // if largest value not on pivot, swap rows
                {
                    decimal[] rowPtr = result[pRow];
                    result[pRow] = result[j];
                    result[j] = rowPtr;

                    int tmp = perm[pRow]; // and swap perm info
                    perm[pRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }

                if (Math.Abs((double)result[j][j]) < 1.0E-20) // if diagonal after swap is zero . . .
                    return null; // consider a throw

                for (int i = j + 1; i < n; ++i)
                {
                    result[i][j] /= result[j][j];
                    for (int k = j + 1; k < n; ++k)
                    {
                        result[i][k] -= result[i][j] * result[j][k];
                    }
                }
            } // main j column loop

            return result;
        } // MatrixDecompose

        // --------------------------------------------------------------------------------------------------------------

        public static decimal[][] Inverse(this decimal[][] matrix)
        {
            int n = matrix.Length;
            decimal[][] result = MatrixDuplicate(matrix);

            int[] perm;
            int toggle;
            decimal[][] lum = MatrixDecompose(matrix, out perm, out toggle);
            if (lum == null)
                throw new Exception("Unable to compute inverse");

            decimal[] b = new decimal[n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (i == perm[j])
                        b[j] = 1.0m;
                    else
                        b[j] = 0.0m;
                }

                decimal[] x = HelperSolve(lum, b); // 

                for (int j = 0; j < n; ++j)
                    result[j][i] = x[j];
            }
            return result;
        }

        // --------------------------------------------------------------------------------------------------------------

        public static decimal Determinant(this decimal[][] matrix)
        {
            int[] perm;
            int toggle;
            decimal[][] lum = MatrixDecompose(matrix, out perm, out toggle);
            if (lum == null)
                throw new Exception("Unable to compute MatrixDeterminant");
            decimal result = toggle;
            for (int i = 0; i < lum.Length; ++i)
                result *= lum[i][i];
            return result;
        }

        // --------------------------------------------------------------------------------------------------------------

        static decimal[] HelperSolve(decimal[][] luMatrix, decimal[] b) // helper
        {
            // before calling this helper, permute b using the perm array from MatrixDecompose that generated luMatrix
            int n = luMatrix.Length;
            decimal[] x = new decimal[n];
            b.CopyTo(x, 0);

            for (int i = 1; i < n; ++i)
            {
                decimal sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum;
            }

            x[n - 1] /= luMatrix[n - 1][n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                decimal sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum / luMatrix[i][i];
            }

            return x;
        }

        // --------------------------------------------------------------------------------------------------------------

        static decimal[] SystemSolve(decimal[][] A, decimal[] b)
        {
            // Solve Ax = b
            int n = A.Length;

            // 1. decompose A
            int[] perm;
            int toggle;
            decimal[][] luMatrix = MatrixDecompose(A, out perm, out toggle);
            if (luMatrix == null)
                return null;

            // 2. permute b according to perm[] into bp
            decimal[] bp = new decimal[b.Length];
            for (int i = 0; i < n; ++i)
                bp[i] = b[perm[i]];

            // 3. call helper
            decimal[] x = HelperSolve(luMatrix, bp);
            return x;
        } // SystemSolve

        // --------------------------------------------------------------------------------------------------------------

        static decimal[][] MatrixDuplicate(decimal[][] matrix)
        {
            // allocates/creates a duplicate of a matrix. assumes matrix is not null.
            decimal[][] result = MatrixCreate(matrix.Length, matrix[0].Length);
            for (int i = 0; i < matrix.Length; ++i) // copy the values
                for (int j = 0; j < matrix[i].Length; ++j)
                    result[i][j] = matrix[i][j];
            return result;
        }

        // --------------------------------------------------------------------------------------------------------------

        static decimal[][] ExtractLower(decimal[][] matrix)
        {
            // lower part of a Doolittle decomposition (1.0s on diagonal, 0.0s in upper)
            int rows = matrix.Length; int cols = matrix[0].Length;
            decimal[][] result = MatrixCreate(rows, cols);
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    if (i == j)
                        result[i][j] = 1.0m;
                    else if (i > j)
                        result[i][j] = matrix[i][j];
                }
            }
            return result;
        }

        static decimal[][] ExtractUpper(decimal[][] matrix)
        {
            // upper part of a Doolittle decomposition (0.0s in the strictly lower part)
            int rows = matrix.Length; int cols = matrix[0].Length;
            decimal[][] result = MatrixCreate(rows, cols);
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    if (i <= j)
                        result[i][j] = matrix[i][j];
                }
            }
            return result;
        }

        public static decimal[] Add(this decimal[] a, decimal[] b)
        {
            decimal[] rez = new decimal[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                rez[i] = a[i] + b[i];
            }
            return rez;
        }
        public static decimal[] Substract(this decimal[] a, decimal[] b)
        {
            decimal[] rez = new decimal[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                rez[i] = a[i] - b[i];
            }
            return rez;
        }

        public static decimal Multiply(this decimal[] a, decimal[] b)
        {
            decimal rez = 0;
            for (int i = 0; i < a.Length; i++)
            {
                rez += a[i] * b[i];
            }
            return rez;
        }

        // --------------------------------------------------------------------------------------------------------------
        static decimal[][] PermArrayToMatrix(int[] perm)
        {
            // convert Doolittle perm array to corresponding perm matrix
            int n = perm.Length;
            decimal[][] result = MatrixCreate(n, n);
            for (int i = 0; i < n; ++i)
                result[i][perm[i]] = 1.0m;
            return result;
        }

        static decimal[][] UnPermute(decimal[][] luProduct, int[] perm)
        {
            // unpermute product of Doolittle lower * upper matrix according to perm[]
            // no real use except to demo LU decomposition, or for consistency testing
            decimal[][] result = MatrixDuplicate(luProduct);

            int[] unperm = new int[perm.Length];
            for (int i = 0; i < perm.Length; ++i)
                unperm[perm[i]] = i;

            for (int r = 0; r < luProduct.Length; ++r)
                result[r] = luProduct[unperm[r]];

            return result;
        } // UnPermute

        public static decimal[][] Transpose(this decimal[][] matrix)
        {
            decimal[][] transpose = new decimal[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                transpose[i] = new decimal[matrix.Length];
            }
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    transpose[j][i] = matrix[i][j];
                }
            }
            return transpose;
        }

        public static decimal[] ReduceDimension(this decimal[] point, int d)
        {
            decimal[] rezult = new decimal[d];
            for (int i = 0; i < d; i++)
            {
                rezult[i] = point[i];
            }
            return rezult;
        }

        public static decimal[] RoundValues(this decimal[] point, decimal epsilon)
        {
            var rez = new decimal[point.Length];
            for (int i = 0; i < point.Length; i++)
            {
                rez[i] = Math.Abs(point[i] - Math.Round(point[i])) > epsilon ? point[i] : Math.Round(point[i]);
            }
            return rez;
        }

        public static Point RoundValues(this Point point, decimal epsilon)
        {

            Point rez = new Point(point.p.Length);
            for (int i = 0; i < point.p.Length; i++)
            {
                rez.p[i] = Math.Abs(point.p[i] - Math.Round(point.p[i])) > epsilon ? point.p[i] : Math.Round(point.p[i]);
            }
            return rez;
        }

        public static decimal[] GeneratePointFromValue(this decimal value, int d)
        {
            decimal[] point = new decimal[d];
            point[d - 1] = value;
            return point;
        }

        public static decimal[] GenerateQueryFromValue(this decimal value, int d)
        {
            decimal[] point = new decimal[d];
            point[d - 1] = value;
            return point;
        }
    }
}
