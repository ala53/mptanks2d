#region Using Statements
using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
#endregion

namespace MPTanks.Client
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //Prevent multiple instances from running
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                Logger.Fatal("Already open!");
                return; //Close
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Logger.Info("Initialized.");

            using (var gm = new ClientCore())
                gm.Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject);
            Logger.Fatal("Unhandled fatal exception at AppDomain level.", (Exception)e.ExceptionObject);
        }
    }
#endif
}
