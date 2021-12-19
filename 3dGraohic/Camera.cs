using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace _3dGraohic
{
    class Camera
    {
        private float _cameraPitch = 0;
        private float _cameraYaw = 0;
        public Vector3 CameraPos { get; private set; } 
        public Vector3 CameraTarget { get; private set; }

        public Camera()
        {
            CameraPos = new Vector3(0, 0, 3f);
            CameraTarget = new Vector3(0, 0, -1f);
        }


        public void KeyManager(FrameEventArgs args, KeyboardState input, Vector2 mouse )
        {
            _cameraYaw += mouse.X * (float)args.Time;
            _cameraPitch += mouse.Y * -(float)args.Time;

            CameraTarget = new Vector3(
                (float)Math.Cos(_cameraYaw) * (float)Math.Cos(_cameraPitch),
                (float)Math.Sin(_cameraPitch),
                (float)Math.Sin(_cameraYaw) * (float)Math.Cos(_cameraPitch)
                );
            CameraTarget.Normalize();

            if (input.IsKeyDown(Keys.LeftShift))
            {
                MoveCamera(new Vector3(0, (float)args.Time, 0));
            }

            if (input.IsKeyDown(Keys.LeftControl))
            {
                MoveCamera(new Vector3(0, -(float)args.Time, 0));
            }

            if (input.IsKeyDown(Keys.A))
            {
                MoveCamera(Vector3.Cross(new Vector3(0, 1, 0), CameraTarget) * (float)args.Time);

            }

            if (input.IsKeyDown(Keys.D))
            {
                MoveCamera(Vector3.Cross(new Vector3(0, 1, 0), CameraTarget) * -(float)args.Time);
            }

            if (input.IsKeyDown(Keys.W))
            {
                MoveCamera(CameraTarget * (float)args.Time);
            }

            if (input.IsKeyDown(Keys.S))
            {
                MoveCamera(CameraTarget * -(float)args.Time);
            }
        }

        private void MoveCamera(Vector3 movment)
        {
            CameraPos += movment * 10;
        }


    }
}
