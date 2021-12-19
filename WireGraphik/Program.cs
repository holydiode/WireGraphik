using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;
using System.Threading;

namespace WireGraphik
{
    class Window : GameWindow
    {
        private IGraphicObject _dinamicObjects;
        private IGraphicObject _scene;
        public Window(int width, int haight): base(new GameWindowSettings(), new NativeWindowSettings() {Size = new OpenTK.Mathematics.Vector2i(width, haight)})
        {
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(1f, 1f, 1f, 1f);

            _init_scene();
        }
        private void _init_scene()
        {
            
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            float[] vertices = new float[9]{
                -0.5f, -0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
                0.0f,  0.5f, 0.0f
            };

            int VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer,vertices.Length, vertices, BufferUsageHint.StaticDraw);

        }
        protected override void OnUnload()
        {

        }
}

    class Program
    {
        static void Main()
        {

            using (Window w = new(500, 500))
            {
                w.Run();
            }
        }
    }
}
