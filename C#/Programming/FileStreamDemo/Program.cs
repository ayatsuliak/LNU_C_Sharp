using System;
using System.IO;
using System.Text;

namespace FileStreamm
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = @"D:\C#\Programming\FileStreamDemo";

            string[] dirs = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories); //виидоть всі папки і підпапки

            /*foreach (string dir in dirs)
            {
                Console.WriteLine(dir);
            }*/

            var files = Directory.GetFiles(rootPath, "*.*", SearchOption.TopDirectoryOnly);

            foreach ( var file in files ) 
            {
                //Console.WriteLine(file);
                //Console.WriteLine(Path.GetFileName(file));
                //Console.WriteLine(Path.GetFileNameWithoutExtension(file));

                var info = new FileInfo(file);
                //Console.WriteLine($"{Path.GetFileName(file)}: {info.Length} bytes");
            }

            bool directoryExists = Directory.Exists(rootPath);

            string newRootPath = @"D:\C#\Programming\FileStreamDemo\SubFolderA";

            if (directoryExists )
            {
                Console.WriteLine("Directory exists");
            }
            else
            {
                Console.WriteLine("Directory not exists");
                Directory.CreateDirectory(newRootPath);
            }

            string[] destinationFolder = Directory.GetFiles(rootPath);

            foreach (string dir in destinationFolder)
            {
                //File.Copy(dir, $"{destinationFolder}{Path.GetFileName(dir)}", true);
                File.Move(dir, $"{destinationFolder}{Path.GetFileName(dir)}");
            }
        }

    }
}