// area size = 1000 x 1000; rx1 = [250, 600]; rx2 = [590, 400]; rx3 = [600, 200]

using orm_omp;
using System.Windows.Markup;
using System.Drawing;
using Accord.Math;
using Matrix = orm_omp.Matrix;

internal class Program
{
    private static void Main(string[] args)
    {
        string repeatProgram = "1";
        do
        {
            int z;

            Console.WriteLine($"Enter z coordinate for bitmap [0, 999]");
            z = Convert.ToInt16(Console.ReadLine());
            if (z < 0 || z > 999) throw new Exception("Error! Selected z value is out of range.");
            int a, b, c;
            Console.WriteLine($"Enter z coordinates of resceivers (3 numbers in range of [0, 999])");
            a = Convert.ToInt16(Console.ReadLine());
            b = Convert.ToInt16(Console.ReadLine());
            c = Convert.ToInt16(Console.ReadLine());
            if (a < 0 || a > 999 || b < 0 || b > 999 || c < 0 || c > 999) throw new Exception("Error! Some values are out of range.");
            Console.WriteLine("Processing...");

            List<Coord> rx = new List<Coord>() { new Coord(250, 600, a), new Coord(590, 400, b), new Coord(600, 200, c) };
            List<double[,]> DOP = new List<double[,]>() { new double[1000, 1000], new double[1000, 1000] };
            List<string> DOPstr = new List<string>() { "PDOP", "HDOP" };

            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    Coord crd = new Coord(x, y, z);
                    Matrix jacobMatrix = new Matrix(3, 3);
                    for (int l = 0; l < jacobMatrix.columnsAmount; l++)
                    {
                        Coord diff = rx[l] - crd;
                        jacobMatrix.mat[0, l] = -diff.x / diff.Norm();
                        jacobMatrix.mat[1, l] = -diff.y / diff.Norm();
                        jacobMatrix.mat[2, l] = -diff.z / diff.Norm();
                    }
                    Matrix jacobMatrixT = jacobMatrix.transpose();
                    SquareMatrix QI = new SquareMatrix(jacobMatrix.rowsAmount, jacobMatrix.rowsAmount);
                    QI = SquareMatrix.mat2SqMat(jacobMatrix * jacobMatrixT);
                    SquareMatrix Q = QI.pseudoInverse();

                    var inversed = QI.mat.PseudoInverse();

                    //DOP[0][x, y] = Math.Sqrt(Q.mat[0, 0] * Q.mat[0, 0] + Q.mat[1, 1] * Q.mat[1, 1] + Q.mat[2, 2] * Q.mat[2, 2]);
                    //DOP[1][x, y] = Math.Sqrt(Q.mat[0, 0] * Q.mat[0, 0] + Q.mat[1, 1] * Q.mat[1, 1]);

					DOP[0][x, y] = Math.Sqrt(inversed[0, 0] * inversed[0, 0] + inversed[1, 1] * inversed[1, 1] + inversed[2, 2] * inversed[2, 2]);
					DOP[1][x, y] = Math.Sqrt(inversed[0, 0] * inversed[0, 0] + inversed[1, 1] * inversed[1, 1]);
				}
            }

            Bitmap bmp = new Bitmap(1000, 1000);
            for (int i = 0; i < 2; i++)
            {
                double max = Matrix.max(DOP[i]);
                for (int x = 0; x < 1000; x++)
                {
                    for (int y = 0; y < 1000; y++)
                    {
                        int val = 0;
                        if ((double.IsNaN(DOP[i][x, y])) || (double.IsInfinity(DOP[i][x, y]))|| (double.IsInfinity(-DOP[i][x, y]))) {
                            DOP[i][x, y] = max;
                        }
                        double curDOP = DOP[i][x, y];
                        if (curDOP == 0) curDOP = double.MinValue;
                        else curDOP = Double.Log10(curDOP);
                        if (curDOP < 0) curDOP = 0;
                        val = Convert.ToInt32(curDOP / Math.Log10(max) * 255);
                        var col = Color.FromArgb(val, val, val);
                        bmp.SetPixel(y, x, col); // transposed due to x,y axis orientation
                    }
                }
                bmp.Save($".\\{DOPstr[i]}.bmp");
            }
            Console.WriteLine("Done! Results are saved in PDOP.bmp and HDOP.bmp. Black stands for min and white stands for max.");
            Console.WriteLine("Do you want to try again? 1 for Yes and 0 for No");
            do
            {
                repeatProgram = Console.ReadLine();
            } while ((repeatProgram != "0") & (repeatProgram != "1"));

        } while (repeatProgram != "0");
    }
}