#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace MPTanks.Clients.GameClient
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
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //Write down some logging information so we know what happened.
            Logger.Log("MonoGame: " + typeof(Microsoft.Xna.Framework.Game).ToString() + ", " +
                typeof(Microsoft.Xna.Framework.Game).Assembly.ToString());
            string dependencies = "Dependencies: ";
            foreach (var dep in typeof(Microsoft.Xna.Framework.Game).Assembly.GetReferencedAssemblies())
                dependencies += "[" + dep.ToString() + "], ";
            Logger.Log(dependencies);

            MPTanks.Engine.ConstructorHelper.CallGlobalCtors();
            StartGame();
        }

        static void StartGame()
        {
            using (var game = new GameClient())
                game.Run();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Logger.Fatal(e.ExceptionObject.ToString());
            }
            catch
            {
            }
        }
    }
#endif
}
