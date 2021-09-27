using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;

namespace WireGraphik
{
    interface IDorwed
    {
        public int ReloadBuffer();
        public int ReloadVerts();
        public void Delete();
        public void Drow();
    }

    interface ITransformed
    {
        public void MakeTransform(Matrix transformMatrix, float t = 1);
        public void Move(float x, float y, float z, float t = 1);
        public void Rotate(float x, float y, float z, float t = 1);
        public void Scale(float x, float y, float z, float t = 1);
        public void Mirror(bool x, bool y, bool z);
        public Point Midle();
        public IGrapjikIbject ToProection();
    }

    interface IGrapjikIbject: IDorwed, ITransformed { 
    }

    class DrowObjectList : List<IGrapjikIbject>, IGrapjikIbject
    {
        public void Delete()
        {
            foreach (IDorwed drowedObject in this)
            {
                drowedObject.Delete();
            }
        }

        public void Drow()
        {
            foreach (IDorwed drowedObject in this)
            {
                drowedObject.ReloadBuffer();
                drowedObject.ReloadVerts();
                drowedObject.Drow();
            }
        }

        public void Scale(float x, float y, float z, float t = 1)
        {
            foreach (ITransformed transformedObject in this)
            {
                transformedObject.Scale(x, y, z, t);
            }
        }

        public void MakeTransform(Matrix transformMatrix, float t = 1)
        {
            foreach (ITransformed transformedObject in this)
            {
                transformedObject.MakeTransform(transformMatrix, t);
            }
        }

        public Point Midle()
        {
            Point midle = new Point(0, 0, 0);
            foreach (IGrapjikIbject graphikObject in this)
            {
                midle += graphikObject.Midle();
            }
            //TODO: сейчас объект - точка, это не правильно
            return 1f / this.Count * midle;
        }

        public void Mirror(bool x, bool y, bool z)
        {
            foreach(ITransformed transformedObject in this)
            {
                transformedObject.Mirror(x, y, z);
            }
        }

        public void Move(float x, float y, float z, float t = 1)
        {
            foreach(ITransformed transformedObject in this)
            {
                transformedObject.Move(x, y, z, t);
            }
        }

        public int ReloadBuffer()
        {
            foreach (IDorwed drowedObject in this)
            {
                drowedObject.ReloadBuffer();
            }
            return 0;
        }

        public int ReloadVerts()
        {
            foreach (IDorwed drowedObject in this)
            {
                drowedObject.ReloadVerts();
            }
            return 0;
        }

        public void Rotate(float x, float y, float z, float t = 1)
        {
            foreach (ITransformed transformedObject in this)
            {
                transformedObject.Rotate(x, y, z, t);
            }
        }

        public IGrapjikIbject ToProection()
        {
            DrowObjectList proectedList = new DrowObjectList();
            proectedList.AddRange(this);
            for (int i =0; i < this.Count; i++)
            {
                this[i] = this[i].ToProection();
            }
            return proectedList;
        }
    }
    class DrowObject : IGrapjikIbject {
        public List<Point> Points;

        public int buffer_id;
        public int object_id;
        public DrowObject(float[] points)
        {
            Points = new();

            for (int i = 0; i < points.Length / 3; i++)
            {
                Points.Add( new Point(points[i * 3], points[i * 3 + 1], points[i * 3 + 2]));
            }

            buffer_id = GL.GenBuffer();
            object_id = GL.GenVertexArray();


        }
        public float[] ToArray() {
            float[] pointArray = new float[Points.Count * 3];
            for(int i = 0; i < Points.Count; i++)
            {
                pointArray[i * 3] = (float)Points[i].X; 
                pointArray[i * 3 + 1] = (float)Points[i].Y; 
                pointArray[i * 3 + 2] = (float)Points[i].Z; 
            }
            return pointArray;
        }
        public int ReloadBuffer()
        {
            DrowObject proectedObject = this.ToProection() as DrowObject;
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer_id);
            GL.BufferData(BufferTarget.ArrayBuffer, Points.Count * 3 * sizeof(float), proectedObject.ToArray() , BufferUsageHint.StaticDraw);
            return buffer_id;
        }
        public int ReloadVerts()
        {
            GL.BindVertexArray(object_id);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            return object_id;
        }
        public void Delete()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(buffer_id);
            GL.DeleteVertexArray(object_id);
        }
        public void Drow()
        {
            GL.BindVertexArray(this.object_id);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer_id);
            GL.DrawArrays(PrimitiveType.Lines, 0, Points.Count);
        }

        public void MakeTransform(Matrix transformMatrix, float t = 1)
        {

            for(int i = 0; i < Points.Count; i++) { 

                Points[i] =  transformMatrix * Points[i] * 1;
            }
        }

        public void Move(float x, float y, float z, float t = 1)
        {
            this.MakeTransform(Matrix.Move3DMatrix(x, y, z));
        }

        public void Rotate(float x, float y, float z, float t = 1)
        {
            Point midle = Midle();
            this.Move((float)-midle.X, (float)-midle.Y, (float)-midle.Z);
            this.MakeTransform(Matrix.Totate3DMatrixX(x) * Matrix.Totate3DMatrixY(y) * Matrix.Totate3DMatrixZ(z), t);
            this.Move((float)midle.X, (float)midle.Y, (float)midle.Z);
        }

        public void Scale(float x, float y, float z, float t = 1)
        {
            Point midle = Midle();
            this.Move((float)-midle.X, (float)-midle.Y, (float)-midle.Z);
            this.MakeTransform(Matrix.ScaleMatrix(x, y, z), t);
            this.Move((float)midle.X, (float)midle.Y, (float)midle.Z);
        }

        public void Mirror(bool x, bool y, bool z)
        {
            Point midle = Midle();
            this.Move((float)-midle.X, (float)-midle.Y, (float)-midle.Z);
            this.MakeTransform(Matrix.MirorMatrix(x, y, z));
            this.Move((float)midle.X, (float)midle.Y, (float)midle.Z);
        }

        public IGrapjikIbject ToProection()
        {
            DrowObject proectedObject = new DrowObject(ToArray()) { buffer_id = buffer_id, object_id = object_id};
            proectedObject.MakeTransform(Matrix.ProectionMatrix());
            return proectedObject;
        }

        public Point Midle()
        {
            Point midle = new Point(0, 0, 0, 1);


            foreach (Point point in this.Points)
            {
                midle += point;
            }
            return 1f / Points.Count * midle;
        }
    }
}
