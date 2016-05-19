using System;
using System.IO;

namespace Inga.Log
{
    public class SimpleLogFileLogger : ILogger
    {
        private readonly string _logname;
        
        public SimpleLogFileLogger(string logname)
        {
            _logname = logname;
        }

        public void Log(string text)
        {    
            Log(text, null);        
        }

        public void Log(string text, Exception ex)
        {
            if (!File.Exists(_logname))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_logname));
            }

            using (var w = File.AppendText(_logname))
            {
                Write(w, text, ex);
            }
        }

        private static void Write(TextWriter w, string text, Exception ex)
        {
            Console.WriteLine(text);
            w.WriteLine($"[{DateTime.Now.ToShortDateString()}] [{DateTime.Now.ToLongTimeString()}] {text}");

            if (ex != null)
            {
                Console.WriteLine(ex.Message);
                w.WriteLine($"[{DateTime.Now.ToShortDateString()}] [{DateTime.Now.ToLongTimeString()}] !!! {ex.Message}");
            }
        }
    }
}
