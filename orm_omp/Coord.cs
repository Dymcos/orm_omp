using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orm_omp
{
    internal class Coord
    {
        public Coord() { this.x = 0; this.y = 0; this.z = 0; }
        public Coord(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double x { set; get; }
        public double y { set; get; }
        public double z { set; get; }

        public static Coord operator +(Coord a, Coord b) => new Coord(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Coord operator -(Coord a, Coord b) => new Coord(a.x-b.x,a.y-b.y,a.z-b.z);
        public static Coord operator -(Coord a) => new Coord(-a.x , -a.y , -a.z);
        public double Norm() { return Math.Sqrt(this.x * this.x + this.y * this.y + this.y * this.y); }
    }
}
