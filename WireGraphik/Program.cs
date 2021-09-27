using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;

namespace WireGraphik
{
    class Window : GameWindow
    {

        private readonly float[] _stive =
    {
            0.25f, 0f, 0f,
            0.1f, 0.15f, 0f,

            0.1f, 0.15f, 0f,
            0.1f, 0.15f, 0.1f,

            0.1f, 0.15f, 0f,
            0.1f, 0.4f, 0f,
            0.1f, 0.4f, 0f,
            0.1f, 0.4f, 0.1f,

            0.1f, 0.4f, 0f,
            0.25f, 0.55f, 0f,
            0.25f, 0.55f, 0f,
            0.25f, 0.55f, 0.1f,

            0.25f, 0.55f, 0f,
            0.5f, 0.55f, 0f,
            0.5f, 0.55f, 0f,
            0.5f, 0.55f, 0.1f,

            0.5f, 0.55f, 0f,
            0.65f, 0.4f, 0f,
            0.65f, 0.4f, 0f,
            0.65f, 0.4f, 0.1f,

            0.65f, 0.4f, 0f,
            0.65f, 0.15f, 0f,
            0.65f, 0.15f, 0f,
            0.65f, 0.15f, 0.1f,

            0.65f, 0.15f, 0f,
            0.5f, 0f, 0f,
            0.5f, 0f, 0f,
            0.5f, 0f, 0.1f,

            0.5f, 0f, 0f,
            0.25f, 0f, 0f,
            0.25f, 0f, 0f,
            0.25f, 0f, 0.1f,

            0.2f, 0.2f, 0f,
            0.3f, 0.15f, 0f,
            0.3f, 0.15f, 0f,
            0.3f, 0.15f, 0.1f,

            0.3f, 0.15f, 0f,
            0.45f, 0.15f, 0f,
            0.45f, 0.15f, 0f,
            0.45f, 0.15f, 0.1f,

            0.45f, 0.15f, 0f,
            0.55f, 0.2f, 0f,
            0.55f, 0.2f, 0f,
            0.55f, 0.2f, 0.1f,

            0.55f, 0.2f, 0f,
            0.45f, 0.1f, 0f,
            0.45f, 0.1f, 0f,
            0.45f, 0.1f, 0.1f,

            0.45f, 0.1f, 0f,
            0.3f, 0.1f, 0f,
            0.3f, 0.1f, 0f,
            0.3f, 0.1f, 0.1f,

            0.3f, 0.1f, 0f,
            0.2f, 0.2f, 0f,
            0.2f, 0.2f, 0f,
            0.2f, 0.2f, 0.1f,

            0.2f, 0.35f, 0f,
            0.2f, 0.45f, 0f,
            0.2f, 0.45f, 0f,
            0.2f, 0.45f, 0.1f,

            0.2f, 0.45f, 0f,
            0.3f, 0.45f, 0f,
            0.3f, 0.45f, 0f,
            0.3f, 0.45f, 0.1f,

            0.3f, 0.45f, 0f,
            0.3f, 0.35f, 0f,
            0.3f, 0.35f, 0f,
            0.3f, 0.35f, 0.1f,

            0.3f, 0.35f, 0f,
            0.2f, 0.35f, 0f,
            0.2f, 0.35f, 0f,
            0.2f, 0.35f, 0.1f,

            0.45f, 0.35f, 0f,
            0.45f, 0.45f, 0f,
            0.45f, 0.45f, 0f,
            0.45f, 0.45f, 0.1f,

            0.45f, 0.45f, 0f,
            0.55f, 0.45f, 0f,
            0.55f, 0.45f, 0f,
            0.55f, 0.45f, 0.1f,

            0.55f, 0.45f, 0f,
            0.55f, 0.35f, 0f,
            0.55f, 0.35f, 0f,
            0.55f, 0.35f, 0.1f,

            0.55f, 0.35f, 0f,
            0.45f, 0.35f, 0f,
            0.45f, 0.35f, 0f,
            0.45f, 0.35f, 0.1f,






            0.25f, 0f, 0.1f,
            0.1f, 0.15f, 0.1f,

            0.1f, 0.15f, 0.1f,
            0.1f, 0.4f, 0.1f,

            0.1f, 0.4f, 0.1f,
            0.25f, 0.55f, 0.1f,

            0.25f, 0.55f, 0.1f,
            0.5f, 0.55f, 0.1f,

            0.5f, 0.55f, 0.1f,
            0.65f, 0.4f, 0.1f,

            0.65f, 0.4f, 0.1f,
            0.65f, 0.15f, 0.1f,

            0.65f, 0.15f, 0.1f,
            0.5f, 0f, 0.1f,

            0.5f, 0f, 0.1f,
            0.25f, 0f, 0.1f,

            0.2f, 0.2f, 0.1f,
            0.3f, 0.15f, 0.1f,

            0.3f, 0.15f, 0.1f,
            0.45f, 0.15f, 0.1f,

            0.45f, 0.15f, 0.1f,
            0.55f, 0.2f, 0.1f,

            0.55f, 0.2f, 0.1f,
            0.45f, 0.1f, 0.1f,

            0.45f, 0.1f, 0.1f,
            0.3f, 0.1f, 0.1f,

            0.3f, 0.1f, 0.1f,
            0.2f, 0.2f, 0.1f,

            0.2f, 0.35f, 0.1f,
            0.2f, 0.45f, 0.1f,

            0.2f, 0.45f, 0.1f,
            0.3f, 0.45f, 0.1f,

            0.3f, 0.45f, 0.1f,
            0.3f, 0.35f, 0.1f,

            0.3f, 0.35f, 00.1f,
            0.2f, 0.35f, 0.1f,

            0.45f, 0.35f, 0.1f,
            0.45f, 0.45f, 0.1f,

            0.45f, 0.45f, 0.1f,
            0.55f, 0.45f, 0.1f,

            0.55f, 0.45f, 0.1f,
            0.55f, 0.35f,0.1f,

            0.55f, 0.35f, 0.1f,
            0.45f, 0.35f, 0.1f,

            };
    
        private DrowObjectList _dinamicObjects;

        private DrowObjectList _scene;
        public Window(int width, int haight): base(new GameWindowSettings(), new NativeWindowSettings() {Size = new OpenTK.Mathematics.Vector2i(width, haight)})
        {
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            var input = KeyboardState;
            var mouse = MouseState.Delta;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (input.IsKeyDown(Keys.T))
            {
                Point midle = _dinamicObjects.Midle();
                _dinamicObjects.Move(-(float)midle.X, -(float)midle.Y, -(float)midle.Z);
                _dinamicObjects.Rotate(mouse.Y / 100, mouse.X / 100, 0.0000f, (float)e.Time);
                _dinamicObjects.Move((float)midle.X, (float)midle.Y, (float)midle.Z);
                _dinamicObjects.Move(mouse.X / 500, -mouse.Y / 500, 0.0000f, (float)e.Time);
            }

            if (input.IsKeyDown(Keys.R))
            {
            if (MouseState.IsAnyButtonDown)
                {
                    _dinamicObjects.Rotate(0, 0, mouse.Y / 100, (float)e.Time);
                }
                else
                {
                    _dinamicObjects.Rotate(mouse.Y / 100, mouse.X / 100, 0.0000f, (float)e.Time);
                }
            }

            if (input.IsKeyDown(Keys.S))
            {
                if (MouseState.IsAnyButtonDown)
                {
                    _dinamicObjects.Scale(1, 1, 1 - mouse.Y / 100, (float)e.Time);
                }
                else
                {
                    _dinamicObjects.Scale(1 + mouse.X / 100, 1 - mouse.Y / 100,1, (float)e.Time);
                }
            }

            if (input.IsKeyDown(Keys.M))
            {
                if (MouseState.IsAnyButtonDown)
                {
                    _dinamicObjects.Move(0, 0, mouse.Y / 100, (float)e.Time);
                }
                else
                {
                    _dinamicObjects.Move(mouse.X / 100, -mouse.Y / 100, 0.0000f, (float)e.Time);
                }
            }

            if (input.IsKeyPressed(Keys.Space))
            {
                _init_scene();
            }

            if (input.IsKeyPressed(Keys.Up))
            {
                _dinamicObjects.Mirror(false, true, false);
            }
            if (input.IsKeyPressed(Keys.Down))
            {
                _dinamicObjects.Mirror(false, false, true);
            }
            if (input.IsKeyPressed(Keys.Left) || input.IsKeyPressed(Keys.Right))
            {
                _dinamicObjects.Mirror(true, false, false);
            }

            if (!input.IsAnyKeyDown)
            {
                if (MouseState.IsAnyButtonDown)
                {
                    _scene.Rotate(mouse.Y / 100, mouse.X / 100, 0.0000f, (float)e.Time);

                }
            }

        }
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(1f, 1f, 1f, 1f);

            _init_scene();

            _scene.ReloadBuffer();
            _scene.ReloadVerts();
        }

        private void _init_scene()
        {
            _scene = new DrowObjectList();
            _dinamicObjects = new DrowObjectList();
            _scene.Add(_dinamicObjects);
            _dinamicObjects.Add(new DrowObject(_stive));
            _scene.Add(new DrowObject(new float[] { 0f, 0f, 0f, 10f, 0f, 0f, 0f, 0f, 0f, 0, 10f, 0f, 0f, 0f, 0f, 0f, 0f, 10f }));

        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _scene.Drow();


            SwapBuffers();
        }
        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
            _scene.Delete();

            base.OnUnload();
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
