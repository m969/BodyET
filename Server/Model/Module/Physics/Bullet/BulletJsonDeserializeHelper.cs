using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace ETModel
{
    public class BulletJsonDeserializeHelper
    {
        public static List<Vector3> Load(string filePath)
        {
            // byte[] bytes = LoadFromFile(filePath);
            
            List<Vector3> verts = new List<Vector3>();

            string[] file = File.ReadAllLines(filePath);
            int NumVertexes = 0;
            foreach (string line in file)
            {
                if (line[0] == '#') continue;
                if (line[0] == 'v' && line[1] != 'n' && line[1] != 't')
                {
                    string[] positions = line.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                    // Vertex pos
                    verts.Add(new Vector3(float.Parse(positions[1]), float.Parse(positions[2]),
                        float.Parse(positions[3])));
                    NumVertexes++;
                }
            }
            return verts;
        }
        
        
        
        public static byte[] LoadFromFile (string path) 
        {
#if NETFX_CORE
			throw new System.NotSupportedException("Cannot load from file on this platform");
#else
          using (var stream = new FileStream(path, FileMode.Open)) 
          {
                var bytes = new byte[(int)stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
                return bytes;
          }
#endif
        }
    }
}