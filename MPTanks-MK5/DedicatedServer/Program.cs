using Microsoft.Xna.Framework;
using MPTanks.Client.GameSandbox;
using MPTanks.Engine;
using MPTanks.Engine.Logging;
using MPTanks.Engine.Settings;
using MPTanks.Networking.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPTanks.DedicatedServer
{
    class Program
    {
        static Server _server;
        static ILogger logger = new ConsoleLogger();
        static void Main(string[] args)
        {
            logger.Info("Server started, loading mods...");

            Client.GameSandbox.Mods.CoreModLoader.LoadTrustedMods(GameSettings.Instance);
            logger.Info("Mods loaded.");
            foreach (var mod in Modding.ModLoader.LoadedMods)
                logger.Info($"\t{mod.Value.Name} version {mod.Value.Version.Major}." +
                    $"{mod.Value.Version.Minor}-{mod.Value.Version.Tag ?? ""}");

            _server = new Server(new Configuration { StateSyncRate = TimeSpan.FromSeconds(0.25), MaxPlayers = 999 },
                new GameCore(new NullLogger(), "DeathMatchGamemode",
                Modding.ModLoader.LoadedMods["core-assets.mod"].GetAsset("testmap.json"),
                new EngineSettings("enginesettings.json")), true, new ConsoleLogger());

            _server.GameInstance.GameChanged += (a, e) =>
             {
                 e.Game.EventEngine.OnGameStarted += (b, f) => logger.Info("Game started");
             };

            for (var i = 0; i < 3; i++)
                _server.AddPlayer(new ServerPlayer(_server, new Networking.Common.NetworkPlayer()
                {
                    Username = "ZZZZZ" + _server.Players.Count,
                    UniqueId = Guid.NewGuid(),
                    ClanName = ""
                }));

            Stopwatch sw = Stopwatch.StartNew();
            GameTime gt = new GameTime();
            while (true)
            {
                _server.Update(gt);
                var elapsed = sw.Elapsed;
                gt.TotalGameTime += elapsed;
                if (16 - sw.ElapsedMilliseconds > 0)
                {
                    Thread.Sleep(16 - (int)sw.ElapsedMilliseconds);
                    gt.ElapsedGameTime = TimeSpan.FromMilliseconds(16);
                }
                else gt.ElapsedGameTime = TimeSpan.FromMilliseconds(sw.Elapsed.TotalMilliseconds);
                sw.Restart();
            }
        }
    }
}
