using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ObjOptimize
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<string> queue = new Queue<string>(args);

            while(queue.Count > 0)
            {
                string filePath = queue.Dequeue();

                if (System.IO.Path.GetExtension(filePath) != ".obj") continue;

                string originalDirectoryName = System.IO.Path.GetDirectoryName(filePath);
                string originalFileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                string objData = System.IO.File.ReadAllText(filePath);

                // Remove comments
                Regex regex = new Regex(@"^#.*$[\r\n]*", RegexOptions.Multiline);
                string result = regex.Replace(objData, "");

                // Optimize vertexes
                regex = new Regex(@"(v\s\-?\d+\.\d{3})\d*(\s\-?\d+\.\d{3})\d*(\s\-?\d+\.\d{3})\d*");
                result = regex.Replace(result, "$1$2$3");

                // Optimize textures
                regex = new Regex(@"(vt\s\-?\d+\.\d{3})\d*(\s\-?\d+\.\d{3})\d*((\s\-?\d+\.\d{3})\d*)?");
                result = regex.Replace(result, "$1$2$3");

                // Optimize normals
                regex = new Regex(@"(vn\s\-?\d+\.\d{3})\d*(\s\-?\d+\.\d{3})\d*(\s\-?\d+\.\d{3})\d*");
                result = regex.Replace(result, "$1$2$3");

                // Optimize vertex parameters
                regex = new Regex(@"(vp\s\-?\d+\.\d{3})\d*((\s\-?\d+\.\d{3})\d*){0,2}");
                result = regex.Replace(result, "$1$2");

                System.IO.File.WriteAllText(System.IO.Path.Combine(originalDirectoryName, originalFileName + "_optimized.obj"), result);
            }
        }
    }
}
