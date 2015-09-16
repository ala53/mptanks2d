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
        private string Prefix => $"[{DateTime.Now.ToShortTimeString()}]";
        public void Debug(string message)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[DEBUG] {Prefix} {message}");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public void Error(Exception ex)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {Prefix} {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public void Error(string message)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {Prefix} {message}");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public void Error(string message, Exception ex)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {Prefix} {message}");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public void Fatal(Exception ex)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"[FATAL] {Prefix} {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public void Fatal(string message)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"[FATAL] {Prefix} {message}");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public void Fatal(string message, Exception ex)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"[FATAL] {Prefix} {message}");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public void Info(object data)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" [INFO] {Prefix} \n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public void Info(string message)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" [INFO] {Prefix} {message}");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public void Trace(Exception ex)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[TRACE] {Prefix} {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public void Trace(object data)
        {/*
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[TRACE] {Prefix} \n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));*/
        }

        public void Trace(string message)
        {/*
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[TRACE] {Prefix} {message}");*/
        }

        public void Trace(string message, Exception ex)
        {/*
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[TRACE] {Prefix} {message}");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);*/
        }

        public void Warning(object data)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" [WARN] {Prefix} \n" +
                JsonConvert.SerializeObject(data, Formatting.Indented));
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public void Warning(string message)
        {
            Console.CursorLeft = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" [WARN] {Prefix} {message}");
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
    }
}
