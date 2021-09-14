using System;
using System.Collections.Generic;



namespace WireGraphik
{
    class Matrix : List<List<double>>
    {

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
                    while(str.Count <= firstIndex)
                    {
                        str.Add(0);
                    }
                }

                while (Height <= secondIndex)
                {
                    this.Add(new List<double>());
                    int wight = Width < firstIndex ? firstIndex : Width;
                    for (int i = 0; i <= wight; i++)
                    {
                        this[Height - 1].Add(0);
                    }
                }
                this[secondIndex][firstIndex] = value;
            }
        }

    }

    class Point : Matrix
    {
        public Point(double X, double Y, double Z, double h = 1)
        {
            this[0, 0] = X / h;
            this[0, 1] = Y / h;
            this[0, 2] = Z / h;
            this[0, 3] = h;
        }

        public double h
        {
            get
            {
                return this[0, 3];
            }
            set
            {
                for (int i = 0; i < 3; i++)
                    this[0, i] *= value / this.h;
                this[0, 3] = h;
            }

        }

        public double X
        {
            get
            {
                return this[0, 0] * h;
            }
            set
            {
                this[0, 0] = value / h;
            }
        }
        public double Y
        {
            get
            {
                return this[0, 1] * h;
            }
            set
            {
                this[0, 1] = value / h;
            }
        }
        public double Z
        {
            get
            {
                return this[0, 2] * h;
            }
            set
            {
                this[0, 2] = value / h;
            }
        }

    }

}
