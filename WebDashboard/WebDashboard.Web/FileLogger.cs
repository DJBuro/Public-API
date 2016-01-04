using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebDashboard.Web
{
    public class FileLogger
    {
        public static void LogEvent(string text)
        {
            //string filename = Path.Combine("c:\\logs", DateTime.Now.ToString("yyyyMMdd") + ".txt");

            //using (StreamWriter streamWriter = new StreamWriter(filename, true))
            //{
            //    streamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + text);
            //}
        }
    }
}