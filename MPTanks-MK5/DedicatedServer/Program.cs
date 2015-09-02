using Microsoft.Xna.Framework;
using MPTanks.Client.GameSandbox;
using MPTanks.Engine;
using MPTanks.Engine.Gamemodes;
using MPTanks.Engine.Logging;
using MPTanks.Engine.Settings;
using MPTanks.Modding;
using MPTanks.Networking.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MPTanks.DedicatedServer
{
    class Program
    {
        static Server _server;
        static ILogger _logger = new ConsoleLogger();
        static string _gamemode;
        static ModAssetInfo _map;
        static void Main(string[] args)
        {
            _logger.Info("Server started, loading core mods...");

            Client.GameSandbox.Mods.CoreModLoader.LoadTrustedMods(GameSettings.Instance);
            _logger.Info("Core mods loaded.");

            LoadMods();

            _logger.Info("Loaded mods list: ");
            foreach (var mod in Modding.ModLoader.LoadedMods)
                _logger.Info($"\t{mod.Value.Name} version {mod.Value.Version.Major}." +
                    $"{mod.Value.Version.Minor}-{mod.Value.Version.Tag ?? ""}");

            _gamemode = ChooseGamemode();
            _map = ChooseMap();
            _server = new Server(new Configuration { StateSyncRate = TimeSpan.FromSeconds(2) },
                new GameCore(new NullLogger(), _gamemode, _map,
                new EngineSettings("enginesettings.json")), true, new ConsoleLogger());

            _server.GameInstance.GameChanged += (a, e) =>
             {
                 _logger.Info("Game changed (new game created).");
                 e.Game.EventEngine.OnGameStarted += (b, f) => _logger.Info("Game started");
                 e.Game.EventEngine.OnGameEnded += (b, f) =>
                 {
                     _logger.Info("Game ended.");
                     //Start a new game of the same type
                     _server.SetGame(new GameCore(new NullLogger(), _gamemode, _map,
                         new EngineSettings("enginesettings.json")));
                 };
             };
            _server.OnCountdownStarted += (a, e) =>
                _logger.Info($"Game countdown started: {e.TotalSeconds.ToString("N0")} seconds remaining.");
            _server.OnCountdownStopped += (a, e) =>
                _logger.Info("Game countdown stopped.");
            _server.SetGame(_server.Game);

            for (var i = 0; i < 1; i++)
                _server.AddPlayer(new ServerPlayer(_server, new Networking.Common.NetworkPlayer()
                {
                    Username = "ZZZZZ" + _server.Players.Count,
                    UniqueId = Guid.NewGuid(),
                    ClanName = ""
                }));

            Logger.Info("For help, type \"help\".");

            Stopwatch sw = Stopwatch.StartNew();
            GameTime gt = new GameTime();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                if (Console.CursorLeft <= 1)
                {
                    Console.CursorLeft = 0;
                    Console.Write(">> ");
                }
                Process();
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

        static void Process()
        {
            var info = WaitLine(0);
            if (info == null) { return; }
            info = info.ToLower();
            if (info.StartsWith("change-gamemode")) _gamemode = ChooseGamemode();
            else if (info.StartsWith("force-restart"))
                _server.SetGame(new GameCore(new NullLogger(),
                    _gamemode, _map, new EngineSettings("enginesettings.json")));
            else if (info.StartsWith("change-map"))
                _map = ChooseMap();
            else if (info.StartsWith("help"))
            {
                _logger.Info("Help menu");
                _logger.Info("help - Show this menu");
                _logger.Info("change-gamemode - Change the gamemode for the next game.");
                _logger.Info("change-map - Change the map for the next game.");
                _logger.Info("change-timescale {scale} - Change the timescale for the current game.");
                _logger.Info("force-restart - Force an immediate restart of the game," +
                    " updating to the new gamemode and map");
                _logger.Info("exit - Exits the game");
            }
            else if (info.StartsWith("change-timescale"))
            {
                if (info.Split(' ').Length < 2)
                {
                    _logger.Error("change-timescale requires a timescale parameter.");
                    return;
                }
                double scale;
                if (!double.TryParse(info.Split(' ')[1], out scale))
                {
                    _logger.Error($"{info.Split(' ')[1]} is not a valid timescale.");
                    return;
                }
                _server.Game.Timescale = new GameCore.TimescaleValue(scale, scale.ToString());
            }
            else if (info.StartsWith("exit"))
            {
                _server.Close();
                _logger.Info("Server closed.");
                Environment.Exit(0);
            }
            else _logger.Error("Command not recognized.");
        }
        static Stopwatch _lineSw = new Stopwatch();
        static string WaitLine(int timeout = int.MaxValue)
        {
            if (Console.CursorLeft <= 1)
            {
                Console.CursorLeft = 0;
                Console.Write(">> ");
            }
            return TimeoutReader.ReadLine(timeout);
        }

        static void LoadMods()
        {
            _logger.Info("Do you want to load any mods (y/n)?");
            bool shouldRead = false;
            string shouldLoad = WaitLine(30000);
            if (shouldLoad == null) shouldLoad = "n";
            if (shouldLoad.Equals("y", StringComparison.OrdinalIgnoreCase) ||
                shouldLoad.Equals("yes", StringComparison.OrdinalIgnoreCase))
                shouldRead = true;
            if (shouldRead)
            {
                while (shouldRead)
                {
                    _logger.Info("Enter mod filename:");
                    var file = WaitLine();
                    if (File.Exists(file))
                    {
                        var mod = Client.GameSandbox.Mods.ModLoader.LoadMod(file, GameSettings.Instance);
                        if (mod == null)
                            _logger.Error($"Could not load mod {file}.");
                        else _logger.Info($"Loaded mod {file}.");
                    }
                    else _logger.Error($"{file} not found.");

                    _logger.Info("Do you wish to load another mod?");
                    shouldLoad = WaitLine();
                    if (shouldLoad.Equals("y", StringComparison.OrdinalIgnoreCase) ||
                        shouldLoad.Equals("yes", StringComparison.OrdinalIgnoreCase))
                        shouldRead = true;
                    else shouldRead = false;
                }
            }
        }

        static string ChooseGamemode()
        {
            var gamemodes = new List<GamemodeType>();
            foreach (var mod in ModDatabase.LoadedModules)
                gamemodes.AddRange(mod.Gamemodes);

            _logger.Info($"Choose the gamemode (1-{gamemodes.Count})");
            _logger.Info("If you want the description, type desc {number}");
            int id = 1;
            foreach (var gamemode in gamemodes)
            {
                _logger.Info($"[{id}] - {gamemode.DisplayName}");
                id++;
            }

            GamemodeType selected = null;
            bool valid = false;
            while (!valid)
            {
                var line = WaitLine(30000);
                if (line == null) line = "1"; //Chose the first one if no input
                if (line.StartsWith("desc", StringComparison.OrdinalIgnoreCase))
                {
                    if (line.Split(' ').Length < 2)
                    {
                        _logger.Error("Missing mod number after desc.");
                        continue;
                    }
                    var number = line.Split(' ')[1];
                    int num;
                    if (!int.TryParse(number, out num))
                    {
                        _logger.Error($"{number} is not a valid number.");
                        continue;
                    }
                    if (num > gamemodes.Count || num < 1)
                    {
                        _logger.Error($"{num} is out of range. It must be between 1 and {gamemodes.Count}.");
                        continue;
                    }
                    _logger.Info($"Gamemode description for {gamemodes[num - 1].DisplayName}.");
                    _logger.Info(gamemodes[num - 1].DisplayDescription);
                    continue;
                }

                int selection;
                if (!int.TryParse(line, out selection))
                {
                    _logger.Error($"{line} is not a valid number.");
                    continue;
                }
                if (selection < 1 || selection > gamemodes.Count)
                {
                    _logger.Error($"{selection} is out of range. It must be between 1 and {gamemodes.Count}.");
                    continue;
                }

                selected = gamemodes[selection - 1];
                valid = true;
            }

            _logger.Info($"Chose gamemode: {selected.DisplayName}.");
            return selected.ReflectionTypeName;
        }

        static ModAssetInfo ChooseMap()
        {
            var maps = new List<ModAssetInfo>();
            foreach (var mod in ModDatabase.LoadedModules)
                maps.AddRange(mod.Header.MapFiles
                    .Select(a => new ModAssetInfo { AssetName = a, ModInfo = mod.ModInfo }));

            _logger.Info($"Choose map (1-{maps.Count})");
            int id = 1;
            foreach (var map in maps)
            {
                _logger.Info($"[{id}] - {map.AssetName} from {map.ModInfo.ModName}.");
            }

            ModAssetInfo info = new ModAssetInfo();
            bool valid = false;
            while (!valid)
            {
                var line = WaitLine(30000);
                if (line == null) line = "1"; //Chose the first one if no input
                int num;
                if (!int.TryParse(line, out num))
                {
                    _logger.Error($"{line} is not a valid number.");
                    continue;
                }
                if (num < 1 || num > maps.Count)
                {
                    _logger.Error($"{num} is out of range. It must be between 1 and {maps.Count}.");
                    continue;
                }
                valid = true;
                info = maps[num - 1];
            }

            _logger.Info($"Chose map: {info.AssetName} from {info.ModInfo.ModName}.");
            return info;
        }
    }
}
