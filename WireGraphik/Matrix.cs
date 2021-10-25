using System;
using System.Collections.Generic;



namespace WireGraphik
{
    class Matrix : List<List<double>>
    {
        public Matrix() : base() { }

        public Matrix(List<List<double>> list):base(list)
        {

        }
        public int Height => this.Count;
        public int Width => Height > 0 ? this[0].Count : 0 ;

        public static implicit operator Matrix(double[][] value)
        {
            Matrix m = new();
            for(int i = 0;  i < value.Length; i++)
            {
                for(int j = 0; j < value[0].Length; j++)
                {
                    m[i, j] = value[i][j];
                }
            }
            return m;
        }

        public static implicit operator string(Matrix value)
        {
            string m = "";
            for (int i = 0; i < value.Height; i++)
            {


                for (int j = 0; j < value.Width; j++)
                {
                    m += value[i, j] + " ";
                }
                m += "\n";
            }
            return m;
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if( a.Height != b.Width)
                throw new Exception("Матрицы невозможно перемножить");

            Matrix c = new();
            for(int i = 0; i < a.Width; i++)
            {
                for(int j = 0; j< b.Height; j++)
                {
                    double val = 0;
                    for (int k = 0; k < a.Height; k++)
                    {
                        val += a[k, i] * b[j, k];
                    }
                    c[j, i] = val;
                }
            }

            return c;
        }
        public static Matrix operator *(Matrix a, double b)
        {
            Matrix newMatrix = new Matrix();
            for(int i = 0; i < a.Height; i++)
            {
                for(int j = 0; j< a.Width; j++)
                {
                    newMatrix[i, j] = a[i, j] * b;
                }
            }
            return newMatrix;
        }

        public static Matrix operator *(double b, Matrix a)
        {
            return a * b;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            Matrix matrix = new Matrix();
            for(int i = 0; i < a.Height; i++)
            {
                for(int j = 0; j < a.Width; j++)
                {
                    matrix[i, j] = a[i, j] + b[i, j];
                }
            }
            return matrix;
        }

        public double this[int firstIndex, int secondIndex]
        {
            get
            {
                return this[firstIndex][secondIndex];
            }

            set
            {
                foreach(List<double> str in this)
                {
                    while(str.Count <= secondIndex)
                    {
                        str.Add(0);
                    }
                }


                while (Height <= firstIndex)
                {
                    this.Add(new List<double>());
                    int wight = Width - 1 < secondIndex ? secondIndex : Width - 1;



                    for (int i = 0; i <= wight; i++)
                    {
                        this[Height - 1].Add(0);
                    }
                }
                this[firstIndex][secondIndex] = value;
            }
        }
        public static Matrix Move3DMatrix(double x, double y, double z)
        {
            Matrix matrix = new Matrix();

            for(int i =0; i < 4; i++)
            {
                matrix[i, i] = 1;
            }
            matrix[3, 0] = x;
            matrix[3, 1] = y;
            matrix[3, 2] = z;

            return matrix;
        }

        public static Matrix Totate3DMatrixX(double phi)
        {
            Matrix matrix = new Matrix();
            matrix[0, 0] = 1;
            matrix[1, 1] = Math.Cos(phi);
            matrix[1, 2] = Math.Sin(phi);
            matrix[2, 1] = -Math.Sin(phi);
            matrix[2, 2] = Math.Cos(phi);
            matrix[3, 3] = 1;
            return matrix;
        }

        public static Matrix Totate3DMatrixY(double phi)
        {
            Matrix matrix = new Matrix();
            matrix[0, 0] = Math.Cos(phi);
            matrix[0, 2] = -Math.Sin(phi);
            matrix[1, 1] = 1;
            matrix[2, 0] = Math.Sin(phi);
            matrix[2, 2] = Math.Cos(phi);
            matrix[3, 3] = 1;
            return matrix;
        }

        public static Matrix Totate3DMatrixZ(double phi)
        {
            Matrix matrix = new Matrix();
            matrix[0, 0] = Math.Cos(phi);
            matrix[0, 1] = Math.Sin(phi);
            matrix[1, 0] = -Math.Sin(phi);
            matrix[1, 1] = Math.Cos(phi);
            matrix[2, 2] = 1;
            matrix[3, 3] = 1;
            return matrix;
        }

        public static Matrix ProectionMatrix()
        {
            Matrix matrix = new Matrix();

            matrix[0, 0] = 1;
            matrix[1, 1] = 1;
            matrix[2, 0] = -Math.Sqrt(2) / 4;
            matrix[2, 1] = -Math.Sqrt(2) / 4;
            matrix[3, 3] = 1;
    


            return matrix;
        }

        public static Matrix MirorMatrix(bool x, bool y, bool z)
        {
            Matrix matrix = new Matrix();
            matrix[0, 0] = x ? -1: 1; 
            matrix[1, 1] = y ? -1: 1; 
            matrix[2, 2] = z ? -1: 1; 
            matrix[3, 3] = 1; 

            return matrix;

        }

        public static Matrix ScaleMatrix(double x, double y, double z)
        {
            Matrix matrix = new Matrix();
            matrix[0, 0] = x;
            matrix[1, 1] = y;
            matrix[2, 2] = z;
            matrix[3, 3] = 1;

            return matrix;
        }

        public static Matrix operator |(Matrix a, Matrix b)
        {
            Matrix matrix = new();

            for(int i = 0; i < a.Height; i++)
            {
                for(int j = 0; j < a.Width; j++)
                {
                    matrix[i, j] = a[i, j];
                }
            }

            for (int i = 0; i < a.Height; i++)
            {
                for (int j = a.Width; j < a.Width + b.Width; j++)
                {
                    matrix[i, j] = b[i, j - a.Width];
                }
            }

            return matrix;
        }

    }

    class Point : Matrix
    {
        public Point() : base() { }

        public Point(Matrix matrix):base()
        {
            this.AddRange(matrix);
        }

        public Point(double X, double Y, double Z, double h = 1):base()
        {
            this[0, 0] = X / h;
            this[0, 1] = Y / h;
            this[0, 2] = Z / h;
            this[0, 3] = h;
        }

        public double H
        {
            get
            {
                return this[0, 3];
            }
            set
            {
                for (int i = 0; i < 3; i++)
                    this[0, i] *= value / this.H;
                this[0, 3] = H;
            }

        }
        public double X
        {
            get
            {
                return this[0, 0] * H;
            }
            set
            {
                this[0, 0] = value / H;
            }
        }
        public double Y
        {
            get
            {
                return this[0, 1] * H;
            }
            set
            {
                this[0, 1] = value / H;
            }
        }
        public double Z
        {
            get
            {
                return this[0, 2] * H;
            }
            set
            {
                this[0, 2] = value / H;
            }
        }

        public static Point operator *(Point a, Matrix b)
        {
            return new Point((Matrix)a * b);
        }

        public static Point operator *( Matrix b, Point a)
        {
            return new Point((b * (Matrix)a));
        }

        public static Point operator *(Point a, double b)
        {
            Point point = new((Matrix)a * b);
            point[0, 3] = a.H;
            return point;
        }

        public static Point operator *(double b, Point a)
        {
            return a * b;
        }

        public static Point operator +(Point a, Point b)
        {
            Point point = new Point((Matrix)a + (Matrix)b);
            point[0, 3] = a.H;
            return  point;
        }

        public static Point CrossLines(Point a1, Point a2, Point a3, Point a4)
        {
            double x = ((a1.X * a2.Y - a1.Y * a2.X) * (a3.X - a4.X) - (a1.X - a2.X) * (a3.X * a4.Y - a3.Y * a4.X)) /
                       ((a1.X - a2.X) * (a3.Y - a4.Y) - (a1.Y - a2.Y) * (a3.X - a4.X));

            double y = ((a1.X * a2.Y - a1.Y * a2.X) * (a3.Y - a4.Y) - (a1.Y - a2.Y) * (a3.X * a4.Y - a3.Y * a4.X)) /
                       ((a1.X - a2.X) * (a3.Y - a4.Y) - (a1.Y - a2.Y) * (a3.X - a4.X));

            return new Point(x,y,0);
        }

        public static Point CrossSegment(Point a1, Point a2, Point a3, Point a4)
        {
            Point crossLinePoint = CrossLines(a1,a2,a3,a4);

            if ( Math.Max(Math.Min(a1.X, a2.X),  Math.Min(a3.X, a4.X)) < crossLinePoint.X &&
                 crossLinePoint.X < Math.Min(Math.Max(a1.X, a2.X), Math.Max(a3.X, a4.X)) &&
                 Math.Max(Math.Min(a1.Y, a2.Y), Math.Min(a3.Y, a4.Y)) < crossLinePoint.Y &&
                 crossLinePoint.Y < Math.Min(Math.Max(a1.Y, a2.Y), Math.Max(a3.Y, a4.Y)))
            {
                return crossLinePoint;
            }

            return null;
        }

    }

}
