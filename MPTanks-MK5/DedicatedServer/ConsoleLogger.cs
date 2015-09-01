using MPTanks.Engine.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.DedicatedServer
{
    class ConsoleLogger : ILogger
    {
        public void Debug(string message)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[DEBUG] [{DateTime.Now.ToShortTimeString()}] {message}");
        }

        public void Error(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] [{DateTime.Now.ToShortTimeString()}] {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }

        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] [{DateTime.Now.ToShortTimeString()}] {message}");
        }

        public void Error(string message, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] [{DateTime.Now.ToShortTimeString()}] {message}");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

        public void Fatal(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"[FATAL] [{DateTime.Now.ToShortTimeString()}] {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }

        public void Fatal(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"[FATAL] [{DateTime.Now.ToShortTimeString()}] {message}");
        }

        public void Fatal(string message, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"[FATAL] [{DateTime.Now.ToShortTimeString()}] {message}");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
        }

        public void Info(object data)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[INFO] [{DateTime.Now.ToShortTimeString()}] \n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[INFO] [{DateTime.Now.ToShortTimeString()}] {message}");
        }

        public void Trace(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[TRACE] [{DateTime.Now.ToShortTimeString()}] {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }

        public void Trace(object data)
        {/*
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[TRACE] [{DateTime.Now.ToShortTimeString()}] \n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));*/
        }

        public void Trace(string message)
        {/*
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[TRACE] [{DateTime.Now.ToShortTimeString()}] {message}");*/
        }

        public void Trace(string message, Exception ex)
        {/*
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[TRACE] [{DateTime.Now.ToShortTimeString()}] {message}");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);*/
        }

        public void Warning(object data)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[WARN] [{DateTime.Now.ToShortTimeString()}] \n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[WARN] [{DateTime.Now.ToShortTimeString()}] {message}");
        }
    }
}
