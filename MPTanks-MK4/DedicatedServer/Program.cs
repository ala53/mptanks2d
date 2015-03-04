using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DedicatedServer
{
    /*
     * NOTE 
     *  This server is not intended for distribution. It is the most barebones
     *  server engine I could create. It spins up a server for testing but does
     *  no matchmaking, etc.
    */
    class Program
    {
        private static bool _exit = false;
        private static string _closeReason = "Server closed";
        private static Server.Server _server;
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Missing port number.");
                return 1;
            }

            int port;

            if (!int.TryParse(args[0], out port))
            {
                Console.WriteLine("Port number is not a number.");
                return 1;
            }


            _server = new Server.Server(int.Parse(args[0]));

            //Increase framerate so we have smoother servers
            Engine.Game.Framerate = 60;
            //Bind event handlers
            _server.OnWeaponFired += _server_OnWeaponFired;
            _server.OnWarning += _server_OnWarning;
            _server.OnPlayerKilled += _server_OnPlayerKilled;
            _server.OnPlayerDamaged += _server_OnPlayerDamaged;
            _server.OnMapObjectDamaged += _server_OnMapObjectDamaged;
            _server.OnGameEnded += _server_OnGameEnded;
            _server.OnFatalError += _server_OnFatalError;
            _server.OnChat += _server_OnChat;
            //Start the server
            _server.Start();

            Task.Factory.StartNew(() =>
            {
                var sw = Stopwatch.StartNew();
                while (!_exit)
                {
                    _server.Update();
                    System.Threading.Thread.Sleep(
                        TimeSpan.FromMilliseconds(
                        Engine.Game.TimePerFrame - sw.ElapsedMilliseconds));

                    sw.Restart();
                }

                _server.Close(_closeReason);
            }, TaskCreationOptions.LongRunning);

            while (!_exit)
                ProcessInput();

            return 0;
        }

        static void _server_OnChat(object sender, Server.Events.Chat e)
        {
            Console.WriteLine("[Chat] " + e.PlayerFrom.Name + " - " + e.Message);
        }

        static void _server_OnFatalError(object sender, Server.Events.FatalError e)
        {
            Console.WriteLine("[Fatal] " + e.FriendlyMessage + "\n\n" + e.StackTrace + "\n\n");
            _closeReason = "Fatal Error: " + e.FriendlyMessage;
            _exit = true;
        }

        static void _server_OnGameEnded(object sender, Server.Events.GameEnded e)
        {
            Console.WriteLine("Round ended. Starting new round.");
            _server.StartNewRound();
        }

        static void _server_OnMapObjectDamaged(object sender, Server.Events.MapObjectDamaged e)
        {
            Console.WriteLine("Map object damaged by " + e.DamagingPlayer.Name);
        }

        static void _server_OnPlayerDamaged(object sender, Server.Events.PlayerDamaged e)
        {
            Console.WriteLine(e.DamagingPlayer.Name + " damaged " +
                e.DamagedPlayer.Name + " (Lost " + e.HPLoss + " HP)");
        }

        static void _server_OnPlayerKilled(object sender, Server.Events.PlayerKilled e)
        {
            Console.WriteLine(e.KillingPlayer.Name + " killed " +
                e.KilledPlayer.Name + " with a " + e.Weapon.Name);
        }

        static void _server_OnWarning(object sender, Server.Events.Warning e)
        {
            Console.WriteLine("[WARN " + e.Severity + "] " + e.Message);
        }

        static void _server_OnWeaponFired(object sender, Server.Events.WeaponFired e)
        {
            Console.WriteLine(e.FiringPlayer.Name + " fired their " + e.Weapon.Name);
        }

        private static void ProcessInput()
        {
            var line = Console.ReadLine();

            if (line == "exit")
                _exit = true;

            if (line.StartsWith("setframerate"))
                Engine.Game.Framerate = double.Parse(line.Split(' ')[1]);
        }
    }
}
