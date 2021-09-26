using System;
using System.Collections.Generic;



namespace WireGraphik
{
    class Matrix : List<List<double>>
    {
        public Matrix() : base() { }
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

            for (int j = 0; j < value.Width; j++)
            {
                for (int i = 0; i < value.Height; i++)
                {
                    m += value[i, j] + " ";
                }
                m += "\n";
            }
            return m;
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if(a.Width != b.Height)
                throw new Exception("Матрицы невозможно перемножить");

            Matrix c = new();
            for(int i = 0; i < a.Height; i++)
            {
                for(int j = 0; j< b.Width; j++)
                {
                    double val = 0;
                    for (int k = 0; k < a.Width; k++)
                    {
                        val += a[i, k] * b[k, j];
                    }
                    c[j, i] = val;
                }
            }

            return c;
        }
        
        public static Matrix operator *(Matrix a, float b)
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

        public static Matrix operator *( float b, Matrix a)
        {
            return a * b;
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
        public static Matrix Move3DMatrix(float x, float y, float z)
        {
            Matrix matrix = new Matrix();

            for(int i =0; i < 4; i++)
            {
                matrix[i, i] = 1;
            }
            matrix[3, 0] = x;
            matrix[3, 0] = y;
            matrix[3, 0] = z;

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
            return new Point(((Matrix)a * b));
        }

        public static Point operator *( Matrix b, Point a)
        {
            return b * a;
        }

        public static Point operator *(Point a, float b)
        {
            return new Point((Matrix)a * b);
        }

        public static Point operator *(float b, Point a)
        {
            return a * b;
        }



    }

}
