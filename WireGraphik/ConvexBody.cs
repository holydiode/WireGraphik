using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace WireGraphik
{
    class ConvexBody : DrawObject, IGraphicObject
    {
        public Polygon[] Polygons { get; private set;}
        public ConvexBody(Polygon[] polygons) : base() 
        {
            Polygons = polygons;
            Repoint();
        }
        public void Repoint()
        {
            Points.Clear();
            foreach (Polygon poligon in Polygons)
            {
                this.Points.AddRange(poligon.GetPointsArray());
            }
        }
        public Matrix GetMatrix()
        {
            Matrix  matrix = Polygons[0].PlateCoefficients();
            for(int i = 1; i < Polygons.Length; i++)
            {
                matrix |= Polygons[i].PlateCoefficients();
            }

            return matrix;
        }
        public void FixPolygonTraverse()
        {
            Matrix midle = this.Middle();
            Matrix res_vector = GetMatrix() * midle;
            for(int i = 0; i < Polygons.Length; i++)
            {
                if(res_vector[0,i] > 0)
                {
                    Polygons[i].Reverse();
                }
            }
        }
        public new void MakeTransform(Matrix transformMatrix, double t = 1) {
            foreach (Polygon poligon in Polygons)
            {
                poligon.MakeTransform(transformMatrix, t);
            }
            Repoint();
        }
        public ConvexBody GetConvexBodyWithHide()
        {
            List<Polygon> front_oligon = new();
            Matrix matrix = GetMatrix() * new Matrix(new List<List<double>>(){ new List<double>() {Math.Sqrt(2)/4, Math.Sqrt(2) / 4, 1,0} });
            for (int i = 0; i < matrix.Width; i++)
            {
                if (matrix[0,i] > 0)
                {
                    front_oligon.Add(Polygons[i]);
                }
            }
            ConvexBody body = new(front_oligon.ToArray()) {_bufferId = _bufferId, _verticeId = _verticeId};
            return body;
        }
        public override IGraphicObject ProtejectionTransform()
        {
            IGraphicObject proectedObject = GetConvexBodyWithHide().Clone() as DrawObject;
            proectedObject.MakeTransform(Matrix.ProectionMatrix());
            return proectedObject;
        }
    }
}
