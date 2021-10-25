using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WireGraphik
{
    interface IDrawable
    {
        public int ReloadBuffer();
        public int ReloadVertices();
        public void Delete();
        public void Drow();
    }

    interface ITransformable
    {
        public void MakeTransform(Matrix transformMatrix, double t = 1);

        public void Move(double x, double y, double z, double t = 1) {
           MakeTransform(Matrix.Move3DMatrix(x, y, z), 1);
        }
        public void Rotate(double x, double y, double z, double t = 1)
        {
            MakeTransform(Matrix.Totate3DMatrixX(x) * Matrix.Totate3DMatrixY(y) * Matrix.Totate3DMatrixZ(z), 1);
        }
        public void Scale(double x, double y, double z, double t = 1)
        {
            MakeTransform(Matrix.ScaleMatrix(x, y, z), 1);
        }
        public void Mirror(bool x, bool y, bool z) {
            MakeTransform(Matrix.MirorMatrix(x, y, z));
        }
        public Point Middle();
        public IGraphicObject ToProjection();
    }

    interface IGraphicObject : IDrawable, ITransformable {
    }

    class DrawObjectList : List<IGraphicObject>, IGraphicObject
    {
        public void Delete()
        {
            foreach (IDrawable drowedObject in this)
            {
                drowedObject.Delete();
            }
        }

        public void Drow()
        {
            foreach (IDrawable drowedObject in this)
            {
                drowedObject.Drow();
            }
        }

        public void MakeTransform(Matrix transformMatrix, double t = 1)
        {
            foreach (ITransformable transformedObject in this)
            {
                transformedObject.MakeTransform(transformMatrix, t);
            }
        }

        public Point Middle()
        {
            Point midle = new(0, 0, 0);
            foreach (IGraphicObject graphikObject in this)
            {
                midle += graphikObject.Middle();
            }
            //TODO: сейчас объект - точка, это не правильно
            return 1f / this.Count * midle;
        }

        public int ReloadBuffer()
        {
            foreach (IDrawable drowedObject in this)
            {
                drowedObject.ReloadBuffer();
            }
            return 0;
        }

        public int ReloadVertices()
        {
            foreach (IDrawable drowedObject in this)
            {
                drowedObject.ReloadVertices();
            }
            return 0;
        }

        public IGraphicObject ToProjection()
        {
            DrawObjectList proectedList = new DrawObjectList();
            proectedList.AddRange(this);
            for (int i = 0; i < this.Count; i++)
            {
                this[i] = this[i].ToProjection();
            }
            return proectedList;
        }
    }

    class DrawObject : IGraphicObject, ICloneable
    {
        public List<Point> Points;

        protected int _bufferId;
        protected int _verticeId;

        public DrawObject(double[] points):this()
        {
            for (int i = 0; i < points.Length / 3; i++)
            {
                Points.Add(new Point(points[i * 3], points[i * 3 + 1], points[i * 3 + 2]));
            }
        }

        public DrawObject(float[] points) : this()
        {
            for (int i = 0; i < points.Length / 3; i++)
            {
                Points.Add(new Point(points[i * 3], points[i * 3 + 1], points[i * 3 + 2]));
            }
        }

        public DrawObject()
        {
            Points = new();
            _bufferId = GL.GenBuffer();
            _verticeId = GL.GenVertexArray();
        }

        public float[] ToArray() {
            float[] pointArray = new float[Points.Count * 3];
            for (int i = 0; i < Points.Count; i++)
            {
                pointArray[i * 3] = (float)Points[i].X;
                pointArray[i * 3 + 1] = (float)Points[i].Y;
                pointArray[i * 3 + 2] = (float)Points[i].Z;
            }
            return pointArray;
        }
        public int ReloadBuffer()
        {
            DrawObject proectedObject = this.ToProjection() as DrawObject;
            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, proectedObject.Points.Count * 3 * sizeof(float), proectedObject.ToArray(), BufferUsageHint.DynamicDraw);
            return _bufferId;
        }
        public int ReloadVertices()
        {
            GL.BindVertexArray(_verticeId);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            return _verticeId;
        }
        public void Delete()
        {
            GL.DeleteBuffer(_bufferId);
            GL.DeleteVertexArray(_verticeId);
        }
        public void Drow()
        {
            ReloadBuffer();
            ReloadVertices();
            GL.BindVertexArray(this._verticeId);
            GL.DrawArrays(PrimitiveType.Lines, 0, Points.Count);
        }

        public void MakeTransform(Matrix transformMatrix, double t = 1)
        {
            for (int i = 0; i < Points.Count; i++) {

                Points[i] = transformMatrix * Points[i] * t;
            }
        }

        public void Move(double x, double y, double z, double t = 1)
        {
            this.MakeTransform(Matrix.Move3DMatrix(x, y, z));
        }

        public void Rotate(double x, double y, double z, double t = 1)
        {
            Point midle = Middle();
            this.Move(-midle.X, -midle.Y, -midle.Z);
            this.MakeTransform(Matrix.Totate3DMatrixX(x) * Matrix.Totate3DMatrixY(y) * Matrix.Totate3DMatrixZ(z), t);
            this.Move(midle.X, midle.Y, midle.Z);
        }

        public void Scale(double x, double y, double z, double t = 1)
        {
            Point midle = Middle();
            this.Move(-midle.X, -midle.Y, (double)-midle.Z);
            this.MakeTransform(Matrix.ScaleMatrix(x, y, z), t);
            this.Move(midle.X, midle.Y, (double)midle.Z);
        }

        public void Mirror(bool x, bool y, bool z)
        {
            Point midle = Middle();
            this.Move(-midle.X, -midle.Y, -midle.Z);
            this.MakeTransform(Matrix.MirorMatrix(x, y, z));
            this.Move(midle.X, midle.Y, midle.Z);
        }

        public IGraphicObject ToProjection()
        {
            return ProtejectionTransform();
        }

        public virtual IGraphicObject ProtejectionTransform()
        {
            IGraphicObject proectedObject = (IGraphicObject)this.Clone();
            proectedObject.MakeTransform(Matrix.ProectionMatrix());
            return proectedObject;
        }

        public Point Middle()
        {
            Point midle = new Point(0, 0, 0);


            foreach (Point point in this.Points)
            {
                midle += point;
            }
            return 1f / Points.Count * midle;
        }

        public object Clone()
        {
            DrawObject copyDrawObject = new() { _bufferId = _bufferId, _verticeId = _verticeId};
            foreach(Point point in Points)
            {
                copyDrawObject.Points.Add(new Point(point.X, point.Y, point.Z));
            }
            return copyDrawObject;
        }
    }
}
