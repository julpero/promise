using System;
using System.IO;

namespace promise
{
    public static class Logger
    {
        private static void WriteLogToFile(string message, TextWriter w)
        {
            w.WriteLine(message);
        }

        public static void Log(string message, string fileName = "")
        {
            using (StreamWriter w = File.AppendText($"log_{fileName}.txt"))
            {
                WriteLogToFile(message, w);
            }
        }

    }
}
