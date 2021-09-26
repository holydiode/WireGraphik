using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System.Collections.Generic;

namespace WireGraphik
{
    class Window : GameWindow
    {

    private readonly float[] _vertices =
{
            -0.5f, -0.5f, 0.0f,
             0.5f, -0.5f, 0.0f,
             0.0f,  0.5f, 0.0f 
        };
    

    private readonly float[] _vertices1 =
    {
            -0.0f, -0.0f, 0.0f,
             0.5f, -0.5f, 0.0f,
             0.0f,  0.5f, 0.0f
        };


    public IGrapjikIbject scene;
    public Window(int width, int haight): base(new GameWindowSettings(), new NativeWindowSettings() {Size = new OpenTK.Mathematics.Vector2i(width, haight)})
    {
    }
    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }


        if (input.IsKeyDown(Keys.A))
        {
            scene.Move(0.001f, 0.001f, 0.001f);
        }
    }
    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(1f, 1f, 1f, 1f);

        scene = new DrowObjectList();
        ((DrowObjectList)scene).Add(new DrowObject(_vertices));
        ((DrowObjectList)scene).Add(new DrowObject(_vertices1));
        ((DrowObjectList)scene).Add(new DrowObject(_vertices));


        scene.ReloadBuffer();
        scene.ReloadVerts();


    }
    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        scene.Drow();


        SwapBuffers();
    }
    protected override void OnUnload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);
        scene.Delete();

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
