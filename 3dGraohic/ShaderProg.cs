using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;

namespace _3dGraohic
{
    class ShaderProg
    {
        public int ID{ private set; get; }
        public ShaderProg(string vertsfile, string fragfile)
        {
            InitShaders(vertsfile, fragfile);
        }
        private void InitShaders(string vertsfile, string fragfile)
        {
            string vertexShaderSource = "";

            using (StreamReader sr = new StreamReader("Shaders/" + vertsfile))
            {
                vertexShaderSource = sr.ReadToEnd();
            }

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, 1, new string[1] { vertexShaderSource }, (int[])null);
            GL.CompileShader(vertexShader);

            Console.WriteLine(GetCompileShaderStatus(vertexShader));


            string fragmentShaderSource = "";

            using (StreamReader sr = new StreamReader("Shaders/" + fragfile))
            {
                fragmentShaderSource = sr.ReadToEnd();
            }
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, 1, new string[1] { fragmentShaderSource }, (int[])null);
            GL.CompileShader(fragmentShader);
            var a = GL.GetShaderInfoLog(fragmentShader);


            Console.WriteLine(GetCompileShaderStatus(fragmentShader));


            ID = GL.CreateProgram();
            GL.AttachShader(ID, vertexShader);
            GL.AttachShader(ID, fragmentShader);
            GL.LinkProgram(ID);
            Console.WriteLine(GetCompileProgrammStatus(ID));


            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

        }

        private bool GetCompileShaderStatus(int shader)
        {
            int[] parameters = new int[] { 0 };
            GL.GetShader(shader, ShaderParameter.CompileStatus, parameters);
            return parameters[0] == 1;
        }

        private bool GetCompileProgrammStatus(int shader)
        {
            int[] parameters = new int[] { 0 };
            GL.GetProgram(shader, GetProgramParameterName.LinkStatus, parameters);
            return parameters[0] == 1;
        }
    }
}
