﻿#region Using Statements
using MPTanks.Clients.GameClient.Menus;
using MPTanks.Clients.GameClient.Menus.InGame;
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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            using (var gm = new ClientCore())
                gm.Run();
            var game = new LiveGame(new Networking.Common.Connection.ConnectionInfo() { FriendlyServerName = "LOL" }, new[] { "" });
            game.WaitForExit();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
        }
    }
#endif
}
