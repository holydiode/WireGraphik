using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Linq;


namespace WireGraphik
{

    class Window : GameWindow
    {
        public Window(int width, int haight): base(new GameWindowSettings(), new NativeWindowSettings() {Size = new OpenTK.Mathematics.Vector2i(width, haight)})
        {
            GLFW.Init();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();

            base.OnUpdateFrame(e);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            vbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, ver.Length * sizeof(float), ver.ToArray(), BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 0, 0);


        }

        int vao;
        int vbo;
        float[] ver = {
            -0.8f, -0.8f, 1f,
            0.0f, 0.8f, 1.0f,
            0.8f, -0.8f, 1.0f
        };

        protected override void OnRenderFrame(FrameEventArgs e)
        {

            base.OnRenderFrame(e);

            GL.Begin(BeginMode.Triangles);
            GL.Vertex2(1,1);
            GL.Vertex2(0,0);
            GL.Vertex2(-1,-1);
            GL.End();
            Context.SwapBuffers();
        }




    }



    class Program
    {
        static void Main()
        {

            Window w = new(500, 500);
            w.Run();
        }
    }
}
