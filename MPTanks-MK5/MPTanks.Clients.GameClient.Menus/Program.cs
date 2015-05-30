﻿#region Using Statements
using EKUI;
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
        static void Main()
        {
            using (var gm = new Game1())
                gm.Run();
            var game = new LiveGame(new Networking.Common.Connection.ConnectionInfo() { FriendlyServerName = "LOL" }, new[] { "" });
            game.WaitForExit();
        }
    }
#endif
}