using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ObjLoader;
using ObjLoader.Loader.Loaders;

namespace _3dGraohic
{
    class DrawObject{
        public  int VAO { get; private set; }
        private int VBO {get; set; }
        private int EBO {get; set;}
        public int ShaderProgramm { get; private set; }
        
        private float[] _vertices;

        private int[] _binds;

        public int DotCount => _vertices.Length / 8;
        public int RealDotCount => _binds.Length;

        public DrawObject(string patch, int shader)
        {
            ObjLoaderFactory objLoaderFactory = new ObjLoaderFactory();
            IObjLoader objLoader = objLoaderFactory.Create();
            FileStream fileStream = new FileStream(patch, FileMode.Open);
            LoadResult loadResult = objLoader.Load(fileStream);
            LoadResult result = loadResult;

            List<float> vertices = new();
            for(int i = 0; i < result.Vertices.Count; i++)
            {
                vertices.Add(result.Vertices[i].X);
                vertices.Add(result.Vertices[i].Y);
                vertices.Add(result.Vertices[i].Z);

                int texture_index = i;
                int normal_index = i;
                
                for (int k = 0; k < result.Groups[0].Faces.Count; k++)
                {
                    if (result.Groups[0].Faces[k][0].VertexIndex == i)
                    {
                        texture_index = result.Groups[0].Faces[k][0].TextureIndex;
                        normal_index = result.Groups[0].Faces[k][0].NormalIndex - 1;
                        break;
                    }else if(result.Groups[0].Faces[k][1].VertexIndex == i)
                    {
                        texture_index = result.Groups[0].Faces[k][1].TextureIndex;
                        normal_index = result.Groups[0].Faces[k][1].NormalIndex - 1;
                        break;
                    }
                    else if (result.Groups[0].Faces[k][2].VertexIndex == i)
                    {
                        texture_index = result.Groups[0].Faces[k][2].TextureIndex;
                        normal_index = result.Groups[0].Faces[k][2].NormalIndex - 1;
                        break;
                    }
                }


                vertices.Add(result.Textures[texture_index].X);
                vertices.Add(result.Textures[texture_index].Y);

                vertices.Add(result.Normals[normal_index].X);
                vertices.Add(result.Normals[normal_index].Y);
                vertices.Add(result.Normals[normal_index].Z);

            }
            List<int> faces = new();


            for (int i = 0; i < result.Groups[0].Faces.Count ; i++)
            {
                faces.Add(result.Groups[0].Faces[i][0].VertexIndex - 1);
                faces.Add(result.Groups[0].Faces[i][1].VertexIndex - 1);
                faces.Add(result.Groups[0].Faces[i][2].VertexIndex - 1);
            }
            

            _binds = faces.ToArray();
            _vertices = vertices.ToArray();


            ShaderProgramm = shader;
            InitBuffers();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public DrawObject() { }
        public  void InitBuffers()
        {
            VBO = GL.GenBuffer();
            EBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _binds.Length * sizeof(int), _binds, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
            GL.EnableVertexAttribArray(2);

        }

        public DrawObject(float[] verts, int shader)
        {
            _vertices = verts;
            _binds = new int[_vertices.Length];
            for(int i = 0; i < _vertices.Length; i++)
            {
                _binds[i] = i;
            }
            var a = CreateNormalArray();
            List<float> newVerts = new List<float>();
            ShaderProgramm = shader;


            for (int i = 0; i < _vertices.Length/5; i++){
                for(int k = 0; k < 5 ; k++)
                {
                    newVerts.Add(_vertices[i * 5 + k]);
                }
                for (int k = 0; k < 3; k++)
                {
                    newVerts.Add(a[i * 3 + k]);
                }
            }

            _vertices = newVerts.ToArray();


            InitBuffers();


            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            
        }

        public void SetShaderParametr(string name, Matrix4 value)
        {
            GL.UseProgram(ShaderProgramm);
            int transformLoc = GL.GetUniformLocation(ShaderProgramm, name);
            GL.UniformMatrix4(transformLoc, false, ref value);
        }

        public void SetShaderParametr(string name, Vector3 value)
        {
            GL.UseProgram(ShaderProgramm);
            int transformLoc = GL.GetUniformLocation(ShaderProgramm, name);
            GL.Uniform3(transformLoc, ref value);
        }

        public void SetShaderParametr(string name, float value)
        {
            GL.UseProgram(ShaderProgramm);
            int transformLoc = GL.GetUniformLocation(ShaderProgramm, name);
            GL.Uniform1(transformLoc, value);
        }
        public void SetShaderParametr(string name, int value)
        {
            GL.UseProgram(ShaderProgramm);
            int transformLoc = GL.GetUniformLocation(ShaderProgramm, name);
            GL.Uniform1(transformLoc, value);
        }
        
        private int GenerateTextire(string textureName)
        {
            GL.BindVertexArray(VAO);
            Bitmap bitmap = new Bitmap("Textures/" + textureName);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            System.Drawing.Imaging.BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            int t = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, t);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            bitmap.Dispose();
            GL.BindVertexArray(0);
            return t;
        }
        private float[] CreateNormalArray()
        {
            float[] normals = new float[_vertices.Length/5 * 3];
            for(int i = 0; i < _vertices.Length / 5; i += 3)
            {
                Vector3 normal = NoramlVector(new Vector3(
                    _vertices[i * 5],
                    _vertices[i * 5 + 1],
                    _vertices[i * 5 + 2]),
                    new Vector3(
                    _vertices[(i + 1) * 5],
                    _vertices[(i + 1) * 5 + 1],
                    _vertices[(i + 1) * 5 + 2]
                        ),
                    new Vector3(
                    _vertices[(i + 2) * 5],
                    _vertices[(i + 2) * 5 + 1],
                    _vertices[(i + 2) * 5 + 2]
                        )
                    );
                for (int k = 0; k < 3; k ++)
                {
                    normals[i * 3 + k * 3] = normal.X;
                    normals[i * 3 + 1 + k * 3] = normal.Y;
                    normals[i * 3 + 2 + k * 3] = normal.Z;
                    int a = 1;
                }
                
            }
            return normals;
        }

        private Vector3 NoramlVector(Vector3 a, Vector3 b, Vector3 c)
        {

            float x,y,z;

            x         = a.Y * b.Z +
                        b.Y * c.Z +
                        c.Y * a.Z -
                        c.Y * b.Z -
                        b.Y * a.Z -
                        a.Y * c.Z;

            y =       -(a.X * b.Z +
                        b.X * c.Z +
                        c.X * a.Z -
                        c.X * b.Z -
                        b.X * a.Z -
                        a.X * c.Z);

            z         = a.X * b.Y +
                        b.X * c.Y +
                        c.X * a.Y -
                        c.X * b.Y -
                        b.X * a.Y -
                        a.X * c.Y;
            Vector3 v = new Vector3(x, y, z);
            v.Normalize();
            return v;
        }

        int DiffuseMap;
        int Specular;
        float Shines;


        Matrix4 StateMatrix;

        public DrawObject SetMaterial(string textureName, string speculatextureName, float shines = 32)
        {
            GL.UseProgram(ShaderProgramm);
            SetShaderParametr("material.diffuse", 1);
            SetShaderParametr("material.specular", 2);

            DiffuseMap = GenerateTextire(textureName);
            Specular = GenerateTextire(speculatextureName);


            Shines = shines;
            SetShaderParametr("material.shininess", 32.0f);
            GL.BindVertexArray(0);
            return this;
        }

        public DrawObject SetState(float x, float y, float z, float scale = 1, float rx = 0, float ry = 0, float rz = 0)
        {
            StateMatrix = Matrix4.CreateScale(scale) * new Matrix4(
                    new Vector4(1f, 0, 0, 0),
                    new Vector4(0, 1f, 0, 0),
                    new Vector4(0, 0, 1f, 0),
                    new Vector4(x, y, z, 1f)
                ) * Matrix4.CreateRotationX(rx) * Matrix4.CreateRotationY(ry) * Matrix4.CreateRotationZ(rz);


            return this;
        }

        public DrawObject Material()
        {
            GL.UseProgram(ShaderProgramm);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, DiffuseMap);

            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, Specular);

            SetShaderParametr("material.shininess", 32.0f);
            return this;
        }

        public DrawObject State()
        {
            SetShaderParametr("transform", StateMatrix);
            return this;
        }

        public DrawObject SetLight()
        {
            SetShaderParametr("light.ambient", new Vector3(0.2f, 0.2f, 0.2f));
            SetShaderParametr("light.diffuse", new Vector3(0.5f, 0.5f, 0.5f));
            SetShaderParametr("light.specular", new Vector3(1.0f, 1.0f, 1.0f));


            SetShaderParametr("light.constant", 1);
            SetShaderParametr("light.linear", 0.045f);
            SetShaderParametr("light.quadratic", 0.0075f);
            return this;
        }

        public DrawObject Clone() {
            return new DrawObject()
            {
                EBO = EBO,
                VAO = VAO,
                VBO = VBO,
                DiffuseMap = DiffuseMap,
                StateMatrix = StateMatrix,
                ShaderProgramm = ShaderProgramm,
                _vertices = _vertices,
                _binds = _binds
            };
        }
    }

    class Window : GameWindow
    {
        public Window(int width, int haight) : base(new GameWindowSettings(), new NativeWindowSettings() { Size = new OpenTK.Mathematics.Vector2i(width, haight) })
        {
            CursorGrabbed = true;
            Cursor  = MouseCursor.Empty;
        }

        private List<DrawObject> _scene = new();

        private Camera _camera;
        protected override void OnLoad()
        {
            base.OnLoad();
            _camera = new();
            GL.ClearColor(0f, 0f, 0f, 1f);
            GL.Enable(EnableCap.DepthTest);
            InitScene();
        }
        private void InitScene()
        {


            float[] vertices2 = new float[]{
                 50f,  -50f, 0f, 10f, 10f,
                -50f,  -50f, 0f, 0.0f, 10f,
                 50f,   50f, 0f, 10f, 0.0f,
                -50f,   50f, 0f, 0.0f, 0.0f,
                 50f,   50f, 0f, 10f, 0.0f,
                -50f,  -50f, 0f, 0.0f, 10f
                };


            float[] vertices3 = new float[]{
                 50f,  0f, -50f,  10f, 10f,
                -50f,  0f, -50f,  0.0f, 10f,
                 50f,  0f,  50f,  10f, 0.0f,
                -50f,  0f,  50f,  0.0f, 0.0f,
                 50f,  0f,  50f,  10f, 0.0f,
                -50f,  0f, -50f,  0.0f, 10f
                };

            ShaderProg solid = new ShaderProg("vert.vert", "frag.frag");
            ShaderProg light = new ShaderProg("vert.vert", "light.frag");

            DrawObject cube = new DrawObject("Models/cube.obj", solid.ID).SetMaterial("cube.jpg", "cube.jpg").SetLight().SetState(0, 6.5f, 0, 0.1f);
            DrawObject cake = new DrawObject("Models/cake.obj", solid.ID).SetMaterial("cake.jpg", "cake.jpg").SetLight().SetState(0, 4f, 0, 0.8f);
            //_scene.Add(cake);
            _scene.Add(cube);
            _scene.Add(cube.Clone().SetState(1f, 6.5f, 4, 0.1f));

            _scene.Add(cube.Clone().SetState(0.5f, 10.3f, 2, 0.1f, ry:0.4f));
            _scene.Add(cube.Clone().SetState(-8f, 6.5f, 9, 0.1f, ry: (float)Math.PI / 2));
            _scene.Add(cube.Clone().SetState(-8.2f, 10.3f, 9.1f, 0.1f, ry: (float)Math.PI / 2));
            _scene.Add(cube.Clone().SetState(-8.3f, 14.3f, 8.9f, 0.1f, ry: (float)Math.PI / 2));


            _scene.Add(new DrawObject(vertices3, solid.ID).SetMaterial("1.png", "1.png").SetLight().SetState(0, 0, 0, 0.6f));
            _scene.Add(new DrawObject(vertices2, solid.ID).SetMaterial("2.png", "2.png").SetLight().SetState(0, 0, 12, 0.6f));
            _scene.Add(new DrawObject(vertices2, solid.ID).SetMaterial("2.png", "2.png").SetLight().SetState(0, 0, -12, 0.6f, ry:(float)Math.PI/2));

        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (DrawObject drawObject in _scene)
            {
                drawObject.Material().State();
                GL.UseProgram(drawObject.ShaderProgramm);
                GL.BindVertexArray(drawObject.VAO);
                GL.DrawElements(PrimitiveType.Triangles, drawObject.RealDotCount, DrawElementsType.UnsignedInt, 0);
            }

            SwapBuffers();
        }



        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            _camera.KeyManager(args, KeyboardState, MouseState.Delta);

            SetShaderParametr("projection", Matrix4.CreatePerspectiveFieldOfView( (float)Math.PI/4, 800f/600f, 0.1f, 100f));
            SetShaderParametr("view", Matrix4.LookAt(_camera.CameraPos, _camera.CameraPos + _camera.CameraTarget, new Vector3(0, 1f, 0)));

            SetShaderParametr("light.position", new  Vector3(10f, 3f, 0f));

            SetShaderParametr("viewPos", _camera.CameraPos);
        }
        private void SetShaderParametr(string name, Matrix4 value)
        {
            foreach (DrawObject drawObject in _scene)
            {
                drawObject.SetShaderParametr(name, value);
            }
        }

        private void SetShaderParametr(string name, float value)
        {
            foreach (DrawObject drawObject in _scene)
            {
                drawObject.SetShaderParametr(name, value);
            }
        }

        private void SetShaderParametr(string name, Vector3 value)
        {
            foreach (DrawObject drawObject in _scene)
            {
                drawObject.SetShaderParametr(name, value);
            }
        }
    }



    class Program
    {
        static void Main()
        {
            using (Window w = new(1000, 1000))
            {
                w.Run();
            }
        }
    }
}
