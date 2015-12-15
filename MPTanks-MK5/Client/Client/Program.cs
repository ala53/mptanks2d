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
                System.Windows.Forms.MessageBox.Show("MP Tanks is already open.", "Already running.");
                Logger.Fatal("Already open!");
                return; //Close
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                ClientSettings.Instance.GetAllSettings();
            }
            catch (Exception e)
            {

            }
            Logger.Info("Initialized.");
            Logger.Info($".NET Runtime version {Environment.Version} on {Environment.OSVersion} " + 
                (Environment.Is64BitOperatingSystem ? "(64-bit)" : "(32-bit)"));
            Logger.Info($"Process is running in " + (Environment.Is64BitProcess ? "64-bit" : "32-bit") + " mode.");
            Logger.Info($"Number of CPUs: {Environment.ProcessorCount}");
            Logger.Info($"Executed as: {Environment.CommandLine}");
            Logger.Info($"Executing in directory {Environment.CurrentDirectory}");

            try
            {
                var window = new OpenTK.GameWindow();
                window.Visible = false;
                window.ProcessEvents();
                Logger.Info("---GPU INFO---");
                Logger.Info("Extensions: " + OpenTK.Graphics.OpenGL.GL.GetString(OpenTK.Graphics.OpenGL.StringName.Extensions));
                Logger.Info("GLSL Version: " + OpenTK.Graphics.OpenGL.GL.GetString(OpenTK.Graphics.OpenGL.StringName.ShadingLanguageVersion));
                Logger.Info("GPU: " + OpenTK.Graphics.OpenGL.GL.GetString(OpenTK.Graphics.OpenGL.StringName.Vendor) + " "
                    + OpenTK.Graphics.OpenGL.GL.GetString(OpenTK.Graphics.OpenGL.StringName.Renderer));
                Logger.Info("GL Version: " + OpenTK.Graphics.OpenGL.GL.GetString(OpenTK.Graphics.OpenGL.StringName.Version));
                window.Close();
                window.Dispose();
            }
            catch
            {
                Logger.Error("GPU does not support OpenGL or OpenTK could not create GL game window");
            }

            try
            {
                using (var gm = new ClientCore())
                    gm.Run();
            }
            catch (PlatformNotSupportedException)
            {
                System.Windows.Forms.MessageBox.Show("Your computer does not meet the minimum requirements for MP Tanks. Check that up-to-date graphics drivers are installed.",
                    "Below requirements", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject);
            Logger.Fatal("Unhandled fatal exception at AppDomain level.", (Exception)e.ExceptionObject);
        }
    }
#endif
}
