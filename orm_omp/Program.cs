// area size = 1000 x 1000; rx1 = [250, 600]; rx2 = [590, 400]; rx3 = [600, 200]

using orm_omp;
using System.Windows.Markup;
using System.Drawing;

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

            List<Coord> rx = new List<Coord>() { new Coord(250, 600, 000), new Coord(590, 400, 500), new Coord(600, 200, 999) };
            List<double[,]> DOP = new List<double[,]>() { new double[1000, 1000], new double[1000, 1000] };
            List<string> DOPstr = new List<string>() { "PDOP", "HDOP" };

            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 1000; y++)
                {
                    Coord crd = new Coord(x, y, z);
                    Matrix JRT = new Matrix(3, 3);
                    for (int l = 0; l < JRT.columnsAmount; l++)
                    {
                        Coord diff = rx[l] - crd;
                        JRT.mat[0, l] = -diff.x / diff.Norm();
                        JRT.mat[1, l] = -diff.y / diff.Norm();
                        JRT.mat[2, l] = -diff.z / diff.Norm();
                    }
                    Matrix JR = JRT.transpose();
                    SquareMatrix QI = new SquareMatrix(JRT.rowsAmount, JRT.rowsAmount);
                    QI = QI.mat2SqMat(JRT * JR);
                    SquareMatrix Q = QI.inverse();
                    DOP[0][x, y] = Math.Sqrt(Q.mat[0, 0] * Q.mat[0, 0] + Q.mat[1, 1] * Q.mat[1, 1] + Q.mat[2, 2] * Q.mat[2, 2]);
                    DOP[1][x, y] = Math.Sqrt(Q.mat[0, 0] * Q.mat[0, 0] + Q.mat[1, 1] * Q.mat[1, 1]);
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
                        if ((double.IsNaN(DOP[i][y, x])) || (double.IsInfinity(DOP[i][y, x])))
                        {
                            DOP[i][y, x] = max;
                        }
                        val = Convert.ToInt32(Math.Log10(DOP[i][y, x]) / Math.Log10(max) * 255);
                        var col = Color.FromArgb(val, val, val);
                        bmp.SetPixel(x, y, col);
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