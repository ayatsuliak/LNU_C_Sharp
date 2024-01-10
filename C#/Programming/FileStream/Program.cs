using System;
using System.IO;
using System.Text;

namespace FileStreamm
{
    class Program
    {
        public static void addTextByFileStream(string path)
        {
            if(File.Exists(path)) File.Delete(path);

            using (var fs = File.Create(path))
            {
                AddText(fs, "This text");
            }
        }

        private static void AddText(FileStream fs, string value)
        {
            var info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(fs, info.Length);
        }
        public static void readTextByFileStream(string path)
        {
            using (var fs = File.OpenRead(path))
            {
                var data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                Console.WriteLine(Encoding.UTF8.GetString(data));
            }
        }
        public static void AddTextByStreamWriter(string path)
        {
            if (File.Exists(path)) File.Delete(path);

            var fs = new FileStream(path, FileMode.CreateNew);

            using (var writer = new StreamWriter(fs))
            {
                writer.Write("This is text");
            }
        }
        public static void ReadTextByStreamWriter(string path)
        {
            if (File.Exists(path)) File.Delete(path);

            var fs = new FileStream(path, FileMode.CreateNew);

            using (var sr = new StreamReader(fs))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
        public static void Main(string[] args)
        {
            addTextByFileStream("@MyTest.txt");
            readTextByFileStream("@MyTest.txt");
        }

    }
}