using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPTanks.DedicatedServer
{
    class TimeoutReader
    {
        private static Thread inputThread;
        private static AutoResetEvent getInput, gotInput;
        private static string input;

        static TimeoutReader()
        {
            getInput = new AutoResetEvent(false);
            gotInput = new AutoResetEvent(false);
            inputThread = new Thread(reader);
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        private static void reader()
        {
            while (true)
            {
                getInput.WaitOne();
                var cColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Cyan;
                input = Console.ReadLine();
                Console.ForegroundColor = cColor;
                gotInput.Set();
            }
        }

        public static string ReadLine(int timeOutMillisecs)
        {
            getInput.Set();
            bool success = gotInput.WaitOne(timeOutMillisecs);
            if (success)
                return input;
            else return null;
        }
    }
}
