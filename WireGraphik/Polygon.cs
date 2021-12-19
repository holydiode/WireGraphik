using System.Collections.Generic;

namespace WireGraphik
{
    class Polygon : ITransformable
    {
        public List<Point> Points { get; private set; }

        public Polygon()
        {
            Points = new();
        }

        public Polygon(double[] points_array):this()
        {
            for (int i = 0; i < points_array.Length / 3; i++)
            {
                Points.Add(new Point(points_array[i * 3], points_array[i * 3 + 1], points_array[i * 3 + 2]));
            }
        }

        public Polygon(int count_vets):this()
        {
            for (int i = 0; i < count_vets; i++)
            {
                Points.Add(new Point(0,0,0));
            }
        }

        public Point[] GetPointsArray()
        {
            Point[] coords = new Point[2 * Points.Count];
            for (int i = 0; i < Points.Count - 1; i++)
            {
                coords[i * 2] = Points[i];
                coords[i * 2 + 1] = Points[i + 1];
            }

            coords[(Points.Count - 1) * 2] = Points[^1];
            return coords;
        }

        public Matrix PlateCoefficients()
        {
            Matrix Coof = new();


            return Coof;
        }

        public void Reverse()
        {
            Points.Reverse();
        }

        public void MakeTransform(Matrix transformMatrix, double t = 1) {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] = transformMatrix * Points[i] * 1;
            }
        }

        public Point Middle()
        {



            return new()  ;
        }

        public IGraphicObject ToProjection()
        {
            throw new System.NotImplementedException();
        }
    }

}
