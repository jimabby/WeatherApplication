using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.Utils
{
    class Logger
    {
        private static string filePath = "weatherApplication.log";

        public static void Log(string message)
        {
            string logMessage = $"{DateTime.Now}: ERROR - {message}";
            Console.WriteLine(logMessage);
            File.AppendAllText(filePath, logMessage + Environment.NewLine);
        }
    }
}
