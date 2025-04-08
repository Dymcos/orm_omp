using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace orm_omp
{
    public class Matrix
    {
        public Matrix(int rowsAmount, int columnsAmount) {
            this.mat = new double[rowsAmount, columnsAmount];
            this.rowsAmount = rowsAmount;
            this.columnsAmount = columnsAmount;
            for (int i = 0; i < rowsAmount; i++)
                {
                    for (int j = 0; j < columnsAmount; j++) {
                        mat[i,j] = 0.0;
                    }
                }
        }
        public int columnsAmount { get; }
        public int rowsAmount { get; }
        public double[,] mat { set; get; }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.rowsAmount != b.columnsAmount)
            {
                throw new ArgumentException("Multiplication of the matrixes is impossible");
            }
            var result = new Matrix(a.rowsAmount, b.columnsAmount);
            for (int i = 0; i < result.rowsAmount; i++)
            {
                for (int j = 0; j < result.columnsAmount; j++)
                {
                    for (int k = 0; k < a.columnsAmount; k++)
                    {
                        result.mat[i, j] += a.mat[i, k] * b.mat[k, j];
                    }
                }
            }
            return result;
        }
        public static Matrix operator -(Matrix a){
            var result = new Matrix(a.rowsAmount,a.columnsAmount);
            for( int i=0; i < a.rowsAmount; i++)
            {
                for( int j = 0;j < a.columnsAmount; j++)
                {
                    result.mat[i,j] = -a.mat[i,j];
                }
            }
            return result;
        }
        public Matrix transpose()
        {
            var result = new Matrix(this.columnsAmount, this.rowsAmount);
            for(int i = 0;i < result.rowsAmount; i++)
            {
                for(int j = 0;j < result.columnsAmount; j++)
                {
                    result.mat[i, j] = this.mat[j, i];
                }
            }
            return result;
        }
        public static double max(double[,] mat)
        {
            double max = 0;
            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    if (!double.IsNaN(mat[x, y]) & (!double.IsInfinity(mat[x, y])) & mat[x, y] > max) max = mat[x, y];
                }
            }
            return max;
        }
    }
}
