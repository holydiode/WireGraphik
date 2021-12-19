using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireGraphik
{
    class RealObject
    {
        public Vector3[] Verties {private set; get; }
        public Vector3[] Color   {private set; get; }
        public Vector2[] textureBounds { private set; get; }

        private List<Tuple<int, int, int>> _faces = new List<Tuple<int, int, int>>();


        public int[] GetIndices()
        {
            List<int> temp = new List<int>();

            foreach (var face in _faces)
            {
                temp.Add(face.Item1);
                temp.Add(face.Item2);
                temp.Add(face.Item3);
            }

            return temp.ToArray();
        }

        public static RealObject LoadFromFile(string path)
        {
            RealObject obj = new();
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                {
                    obj = LoadFromString(reader.ReadToEnd());
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: {0}", path);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading file: {0}", path);
            }
            return obj;
        }

        public static RealObject LoadFromString(string obj)
        {
            List<String> lines = new List<string>(obj.Split('\n'));

            List<Vector3> verts = new List<Vector3>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> textures = new List<Vector2>();

            List<Tuple<int, int, int>> faces = new List<Tuple<int, int, int>>();

            foreach (String line in lines)
            {
                if (line.StartsWith("v ")) // Vertex definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a vertex
                    {
                        String[] vertparts = temp.Split(' ');

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0], out vec.X);
                        success &= float.TryParse(vertparts[1], out vec.Y);
                        success &= float.TryParse(vertparts[2], out vec.Z);

                        // Dummy color/texture coordinates for now
                        colors.Add(new Vector3((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));
                        textures.Add(new Vector2((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing vertex: {0}", line);
                        }
                    }

                    verts.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Tuple<int, int, int> face = new Tuple<int, int, int>(0, 0, 0);

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a face
                    {
                        String[] faceparts = temp.Split(' ');

                        int i1, i2, i3;

                        // Attempt to parse each part of the face
                        bool success = int.TryParse(faceparts[0], out i1);
                        success &= int.TryParse(faceparts[1], out i2);
                        success &= int.TryParse(faceparts[2], out i3);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing face: {0}", line);
                        }
                        else
                        {
                            // Decrement to get zero-based vertex numbers
                            face = new Tuple<int, int, int>(i1 - 1, i2 - 1, i3 - 1);
                            faces.Add(face);
                        }
                    }
                }
            }

            // Create the ObjVolume
            RealObject robj = new RealObject() {
                Verties = verts.ToArray(),
                _faces = new List<Tuple<int, int, int>>(faces),
                Color = colors.ToArray(),
                textureBounds = textures.ToArray()
            };


            return robj;
        }
    }


}
