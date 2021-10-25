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

        public double[] GetLinesArray()
        {
            double[] coords = new double[6 * Points.Count];
            for (int i = 0; i < Points.Count - 1; i++)
            {
                coords[i * 6] = Points[i].X;
                coords[i * 6 + 1] = Points[i].Y;
                coords[i * 6 + 2] = Points[i].Z;

                coords[i * 6 + 3] = Points[i + 1].X;
                coords[i * 6 + 4] = Points[i + 1].Y;
                coords[i * 6 + 5] = Points[i + 1].Z;
            }

            coords[(Points.Count - 1) * 6] = Points[^1].X;
            coords[(Points.Count - 1) * 6 + 1] = Points[^1].Y;
            coords[(Points.Count - 1) * 6 + 2] = Points[^1].Z;

            coords[(Points.Count - 1) * 6 + 3] = Points[0].X;
            coords[(Points.Count - 1) * 6 + 4] = Points[0].Y;
            coords[(Points.Count - 1) * 6 + 5] = Points[0].Z;

            return coords;
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
            coords[(Points.Count - 1) * 2 + 1] = Points[0];
            return coords;
        }

        public Matrix PlateCoefficients()
        {
            Matrix Coof = new();
            Coof[0, 0] = Points[0].Y * Points[1].Z +
                        Points[1].Y * Points[2].Z +
                        Points[2].Y * Points[0].Z -
                        Points[2].Y * Points[1].Z -
                        Points[1].Y * Points[0].Z -
                        Points[0].Y * Points[2].Z;

            Coof[1, 0] = -(Points[0].X * Points[1].Z +
                        Points[1].X * Points[2].Z +
                        Points[2].X * Points[0].Z -
                        Points[2].X * Points[1].Z -
                        Points[1].X * Points[0].Z -
                        Points[0].X * Points[2].Z);

            Coof[2, 0] = Points[0].X * Points[1].Y +
                        Points[1].X * Points[2].Y +
                        Points[2].X * Points[0].Y -
                        Points[2].X * Points[1].Y -
                        Points[1].X * Points[0].Y -
                        Points[0].X * Points[2].Y;


            Coof[3, 0] = -(Points[0].X * Points[1].Y * Points[2].Z +
                        Points[1].X * Points[2].Y * Points[0].Z +
                        Points[2].X * Points[0].Y * Points[1].Z -
                        Points[2].X * Points[1].Y * Points[0].Z -
                        Points[1].X * Points[0].Y * Points[2].Z -
                        Points[0].X * Points[2].Y * Points[1].Z);

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
            Point midle = Points[0];

            for (int i = 1; i < Points.Count; i++)
            {
                midle += Points[i];
            }

            return 1f / Points.Count * midle;
        }

        public List<Point> GetOverdrawLinePoint(Point a, Point b)
        {
            List<Point> segments = new();
            Point[] PolygonLines = GetPointsArray();
            for (int i = 0; i < PolygonLines.Length - 1; i +=2)
            {
                Point crossPoint = Point.CrossSegment(a, b, PolygonLines[i], PolygonLines[i + 1]);
                if( !(crossPoint is null))
                {
                    segments.Add(crossPoint);
                }
            }
            return segments;
        }

        public IGraphicObject ToProjection()
        {
            throw new System.NotImplementedException();
        }
    }

}
