#region Using Statements
using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace MPTanks.Client.GameSandbox
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
            Logger.Trace("MonoGame Game Class: " + typeof(Microsoft.Xna.Framework.Game).ToString() + ", " +
                typeof(Microsoft.Xna.Framework.Game).Assembly.ToString());
            string dependencies = "Dependencies: ";
            foreach (var dep in typeof(Microsoft.Xna.Framework.Game).Assembly.GetReferencedAssemblies())
                dependencies += "[" + dep.ToString() + "], ";
            Logger.Info(dependencies);

            if (args.Length < 1)
            {
                Logger.Fatal("Missing info from main thread. Did you accidentally run the executable directly?");
                return;
            }

            StartGame(args);
        }

        static void StartGame(string[] args)
        {
            using (var game = new GameClient(Newtonsoft.Json.JsonConvert.DeserializeObject<CrossProcessStartData>(args[0])))
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
            System.Windows.Forms.MessageBox.Show("Fatal internal error!\n" + e.ExceptionObject.ToString(), "Fatal error");
        }
    }
#endif
}
