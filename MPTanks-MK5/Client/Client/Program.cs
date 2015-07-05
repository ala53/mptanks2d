#region Using Statements
using MPTanks.Clients.GameClient.Menus;
using MPTanks.Clients.GameClient.Menus.InGame;
using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
#endregion

namespace MPTanks.Clients.GameClient.Menus
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
            if (!GlobalSettings.Debug)
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Logger.Info("Initialized.");

            using (var gm = new ClientCore())
                gm.Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Logger.Fatal("Unhandled fatal exception at AppDomain level.", (Exception)e.ExceptionObject);
        }
    }
#endif
}
