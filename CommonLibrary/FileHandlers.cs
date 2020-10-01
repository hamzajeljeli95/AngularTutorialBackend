using System;
using System.IO;

namespace CommonLibrary
{
    public class FileHandlers
    {
        public static void WriteToFile(String path, String s)
        {
            File.WriteAllText(path, s);
        }

        public static String ReadFileContents(String path)
        {
            return System.IO.File.ReadAllText(path);
        }

        public static FileStream OpenFile(String path, FileMode mode)
        {
            return File.Open(path, mode);
        }
    }
}
