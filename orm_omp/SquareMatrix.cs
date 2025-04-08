using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace orm_omp
{
    internal class SquareMatrix : Matrix
    {
        public SquareMatrix(int rowsAmount, int columnsAmount) : base(rowsAmount, columnsAmount){
            this.size = rowsAmount;
        }
        public int size { get; }
        public double det() {
            if (this.size == 1) 
                return this.mat[0, 0];
            if (this.size == 2)
                return this.mat[0, 0] * this.mat[1, 1] - this.mat[0, 1] * this.mat[1, 0];
            if (this.size == 3)
                return this.mat[0, 0] * this.mat[1, 1] * this.mat[2, 2] + this.mat[0, 1] * this.mat[1, 2] * this.mat[2, 0] + this.mat[0, 2] * this.mat[1, 0] * this.mat[2, 1] - this.mat[0, 2] * this.mat[1, 1] * this.mat[2, 0] - this.mat[0, 1] * this.mat[1, 0] * this.mat[2, 2] - this.mat[0, 0] * this.mat[1, 2] * this.mat[2, 1];
            throw new Exception("det is appliable only for 1x1, 2x2 or 3x3 sized matriсes");
        }
        public static SquareMatrix mat2SqMat(Matrix a)
        {
            SquareMatrix result = new SquareMatrix(a.rowsAmount, a.columnsAmount);
            for (int i = 0; i < a.rowsAmount; i++)
            {
                for(int j = 0; j < a.columnsAmount; j++)
                {
                    result.mat[i, j] = a.mat[i, j];
                }
            }
            return result;
        }
        public SquareMatrix adj()
        {
            var result = new SquareMatrix(this.size, this.size);
            var redMatrix = new SquareMatrix(this.size - 1, this.size - 1); // reduced matrix
            int redMatrix_k = 0;
            int redMatrix_l = 0;
            for (int i = 0; i < this.size; i++)
            {
                for(int j = 0; j < this.size; j++)
                {
                    redMatrix_k = 0;
                    for (int k = 0; k < this.size; k++) // matrix reduction
                    {
                        if (k == i) continue;
                        redMatrix_l = 0;
                        for (int l = 0; l < this.size; l++)
                        {
                            if (l == j) continue;
                            redMatrix.mat[redMatrix_k, redMatrix_l] = this.mat[k,l];
                            redMatrix_l++;
                        }
                        redMatrix_k++;
                    }
                    result.mat[i, j] = Math.Pow(-1, (i + 1) + (j + 1)) * redMatrix.det();
                }
            }
            return result;
        }
        public new SquareMatrix transpose()
        {
            var result = new SquareMatrix(this.columnsAmount, this.rowsAmount);
            for (int i = 0; i < result.rowsAmount; i++)
            {
                for (int j = 0; j < result.columnsAmount; j++)
                {
                    result.mat[i, j] = this.mat[j, i];
                }
            }
            return result;
        }
        public SquareMatrix inverse()
        {
            var result = new SquareMatrix(this.size, this.size);
            SquareMatrix adjmat = this.adj();
            for (int i = 0;i < this.size; i++)
            {
                for(int j = 0;j < this.size; j++)
                {
                    result.mat[i, j] = adjmat.mat[i, j] / this.det();
                }
            }
            return result;
        }
        public SquareMatrix pseudoInverse()
        {
            SquareMatrix result;
            result = SquareMatrix.mat2SqMat(this.transpose() * this);
            result = result.inverse();
            result = SquareMatrix.mat2SqMat(result * this.transpose());
            return result;
        }
       
    }
}
