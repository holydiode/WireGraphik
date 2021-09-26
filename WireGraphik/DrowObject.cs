using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

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
        public void MakeTransform(Matrix transformMatrix);
        public void Move(float x, float y, float z, float t = 1);
        public void Rotate(float x, float y, float z, float t = 1);
        public void Expand(float x, float y, float z, float t = 1);
        public void Mirror(bool x, bool y, bool z, float t = 1);

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

        public void Expand(float x, float y, float z, float t = 1)
        {
            throw new System.NotImplementedException();
        }

        public void MakeTransform(Matrix transformMatrix)
        {
            foreach (ITransformed transformedObject in this)
            {
                transformedObject.MakeTransform(transformMatrix);
            }
        }

        public void Mirror(bool x, bool y, bool z, float t = 1)
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
                transformedObject.Move(x, y, z);
                System.Console.WriteLine(1);
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
                transformedObject.Rotate(x, y, z);
            }
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
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer_id);
            GL.BufferData(BufferTarget.ArrayBuffer, Points.Count * 3 * sizeof(float), this.ToArray() , BufferUsageHint.StaticDraw);
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
            GL.DrawArrays(PrimitiveType.LineLoop, 0, Points.Count);
        }

        public void MakeTransform(Matrix transformMatrix)
        {
            for(int i = 0; i < Points.Count; i++) { 

                Points[i] *=  transformMatrix;
            }
        }

        public void Move(float x, float y, float z, float t = 1)
        {
            this.MakeTransform(Matrix.Move3DMatrix(x, y, z));
        }

        public void Rotate(float x, float y, float z, float t = 1)
        {
            throw new System.NotImplementedException();
        }

        public void Expand(float x, float y, float z, float t = 1)
        {
            throw new System.NotImplementedException();
        }

        public void Mirror(bool x, bool y, bool z, float t = 1)
        {
            throw new System.NotImplementedException();
        }
    }
}
