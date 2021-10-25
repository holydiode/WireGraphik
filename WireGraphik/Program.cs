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
            base.OnUpdateFrame(e);
            var input = KeyboardState;
            var mouse = MouseState.Delta;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (input.IsKeyDown(Keys.T))
            {
                Point midle = _dinamicObjects.Middle();
                _dinamicObjects.Move(-midle.X, -midle.Y, -midle.Z);
                _dinamicObjects.Rotate(mouse.Y / 100, mouse.X / 100, 0.0000f, e.Time);
                _dinamicObjects.Move(midle.X, midle.Y, midle.Z);
                _dinamicObjects.Move(mouse.X / 500, -mouse.Y / 500, 0.0000f, e.Time);
            }

            if (input.IsKeyDown(Keys.R))
            {
            if (MouseState.IsAnyButtonDown)
                {
                    _dinamicObjects.Rotate(0, 0, mouse.Y / 100, e.Time);
                }
                else
                {
                    _dinamicObjects.Rotate(mouse.Y / 100, mouse.X / 100, 0.0000f, e.Time);
                }
            }

            if (input.IsKeyDown(Keys.S))
            {
                if (MouseState.IsAnyButtonDown)
                {
                    _dinamicObjects.Scale(1, 1, 1 - mouse.Y / 100, e.Time);
                }
                else
                {
                    _dinamicObjects.Scale(1 + mouse.X / 100, 1 - mouse.Y / 100,1, e.Time);
                }
            }

            if (input.IsKeyDown(Keys.M))
            {
                if (MouseState.IsAnyButtonDown)
                {
                    _dinamicObjects.Move(0, 0, mouse.Y / 100, e.Time);
                }
                else
                {
                    _dinamicObjects.Move(mouse.X / 100, -mouse.Y / 100, 0.0000f, e.Time);
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
                    _scene.Rotate(mouse.Y / 100, mouse.X / 100, 0.0000f, e.Time);

                }
            }
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(1f, 1f, 1f, 1f);

            _init_scene();
            _scene.ReloadBuffer();
        }
        private void _init_scene()
        {
            _scene = new DrawObjectList();
            _dinamicObjects = new DrawObjectList();

            ((DrawObjectList)_scene).Add(_dinamicObjects);

            ConvexBody boop = new ConvexBody(
                    new Polygon[] {
                        new Polygon(new double[]{
                            0, 0, 0,
                            0.2, 0, 0,
                            0.1, 0, 0.2
                        }),
                        new Polygon(new double[]{
                            0, 0, 0,
                            0.1, 0.2, 0,
                            0.2, 0, 0
                        }),
                        new Polygon(new double[]{
                            0, 0, 0,
                            0.1, 0.2, 0,
                            0.1, 0, 0.2
                        }),
                        new Polygon(new double[]{
                            0.1, 0.2, 0,
                            0.1, 0, 0.2,
                            0.2, 0, 0
                        })
                    }
            );

            ConvexBody loop = new ConvexBody(
                    new Polygon[] {
                        new Polygon(new double[]{
                            0.2, 0, 0,
                            0.4, 0, 0,
                            0.3, 0, 0.2
                        }),
                        new Polygon(new double[]{
                            0.2, 0, 0,
                            0.3, 0.2, 0,
                            0.4, 0, 0
                        }),
                        new Polygon(new double[]{
                            0.2, 0, 0,
                            0.3, 0.2, 0,
                            0.3, 0, 0.2
                        }),
                        new Polygon(new double[]{
                            0.3, 0.2, 0,
                            0.3, 0, 0.2,
                            0.4, 0, 0
                        })
                    }
            );

            loop.FixPolygonTraverse();
            boop.FixPolygonTraverse();

            ((DrawObjectList)_dinamicObjects).Add(loop);
            ((DrawObjectList)_dinamicObjects).Add(boop);
            //((DrawObjectList)_dinamicObjects).Add(new DrawObject(boop.ToArray()));

        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            ((ConvexBody)((DrawObjectList)_dinamicObjects)[0]).HideOverdrawLines((ConvexBody)((DrawObjectList)_dinamicObjects)[1]);



            GL.Clear(ClearBufferMask.ColorBufferBit);
            _scene.Drow();
            SwapBuffers();
        }
        protected override void OnUnload()
        {
            _scene.Delete();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
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
