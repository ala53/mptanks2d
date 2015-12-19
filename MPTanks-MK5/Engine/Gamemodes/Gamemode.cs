using Microsoft.Xna.Framework;
using MPTanks.Engine.Helpers;
using MPTanks.Engine.Settings;
using MPTanks.Modding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Gamemodes
{
    public abstract class Gamemode
    {
        [JsonIgnore]
        public GameCore Game { get; private set; }
        public virtual bool GameEnded { get; protected set; }
        public virtual Team WinningTeam { get; protected set; } = Team.Null;
        public virtual Team[] Teams { get; set; } = new Team[0];
        public bool AllowRespawn { get; protected set; }
        public TimeSpan RespawnTime { get; protected set; }

        public byte[] FullState
        {
            get { return GetFullState(); }
            set
            {
                var rdr = ByteArrayReader.Get(value);
                SetFullState(rdr);
                rdr.Release();
            }
        }

        #region Reflection helper
        public string ReflectionName => ContainingModule.Name + "+" + GetType().Name;

        private string _cachedDisplayName;
        public string DisplayName
        {
            get
            {
                if (_cachedDisplayName == null)
                    _cachedDisplayName = ((GameObjectAttribute[])(GetType()
                          .GetCustomAttributes(typeof(GameObjectAttribute), true)))[0]
                          .DisplayName;

                return _cachedDisplayName;
            }
        }
        private string _cachedDescription;
        public string Description
        {
            get
            {
                if (_cachedDescription == null)
                    _cachedDescription = ((GameObjectAttribute[])(GetType()
                          .GetCustomAttributes(typeof(GameObjectAttribute), true)))[0]
                          .Description;

                return _cachedDescription;
            }
        }

        private Module _cachedModule;
        /// <summary>
        /// The module that contains this object
        /// </summary>
        [JsonIgnore]
        public Module ContainingModule
        {
            get
            {
                if (_cachedModule == null)
                    _cachedModule = ModDatabase.TypeToModuleTable[GetType()];

                return _cachedModule;
            }
        }

        private int? _cachedPlrCt;
        public int MinPlayerCount
        {
            get
            {
                if (_cachedPlrCt == null)
                    _cachedPlrCt = ((Modding.GamemodeAttribute)(GetType().
                         GetCustomAttributes(typeof(Modding.GamemodeAttribute), true))[0]).MinPlayersCount;

                return _cachedPlrCt.Value;
            }
        }

        private string _defaultTankReflectionName;
        public string DefaultTankTypeReflectionName
        {
            get
            {
                if (_defaultTankReflectionName == null)
                {
                    _defaultTankReflectionName = ((Modding.GamemodeAttribute)(GetType().
                         GetCustomAttributes(typeof(Modding.GamemodeAttribute), true))[0]).DefaultTankTypeReflectionName;

                    if (_defaultTankReflectionName.Split('+').Length == 1)
                    {
                        //Preface it with module name
                        _defaultTankReflectionName = ContainingModule.Name + "+" + _defaultTankReflectionName;
                    }

                }
                return _defaultTankReflectionName;
            }
        }
        #endregion


        private bool? _hotJoinEnabled;
        public bool HotJoinEnabled
        {
            get
            {
                if (_hotJoinEnabled == null)
                    _hotJoinEnabled = ((Modding.GamemodeAttribute)(GetType().
                         GetCustomAttributes(typeof(Modding.GamemodeAttribute), true))[0]).HotJoinEnabled;

                return _hotJoinEnabled.Value;
            }
        }

        /// <summary>
        /// An event that is fired when the gamemode updates and changes state
        /// </summary>
        public event EventHandler<Core.Events.Types.Gamemodes.StateChangedEventArgs> OnGamemodeStateChanged = delegate { };

        public Gamemode(byte[] serverState = null)
        {

        }

        internal void SetGame(GameCore game)
        {
            Game = game;
        }

        public virtual void Create()
        {

        }

        /// <summary>
        /// Puts all of the tanks on teams
        /// </summary>
        /// <param name="tanks"></param>
        public abstract void MakeTeams(GamePlayer[] players);

        /// <summary>
        /// Gets the tank types that a player can choose from (reflection names only)
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public abstract string[] GetPlayerAllowedTankTypes(GamePlayer player);

        /// <summary>
        /// Sets the tank type for a player, from the list of allowed types. 
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="tankType"></param>
        /// <returns>Returns true if the tank type was able to be set (still allowed) or false if it is not.</returns>
        public abstract bool CheckPlayerTankSelectionValid(GamePlayer player, string tankType);

        /// <summary>
        /// Notifies the Gamemode that the game has started. It can do whatever it wants 
        /// in terms of startup logic.
        /// </summary>
        public virtual void StartGame() { }
        /// <summary>
        /// Lets the game mode run its internal logic for players
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        public int GetTeamIndex(GamePlayer player)
        {
            for (var i = 0; i < Teams.Length; i++)
                if (Teams[i] == player.Team)
                    return i;

            return int.MaxValue;
        }
        #region State Changes
        private Core.Events.Types.Gamemodes.StateChangedEventArgs _args =
            new Core.Events.Types.Gamemodes.StateChangedEventArgs();

        private TimeSpan _lastStateChange = TimeSpan.Zero;
        protected bool RaiseStateChangeEvent(Action<ByteArrayWriter> eventWriter)
        {
            if (eventWriter == null) return false;

            var writer = ByteArrayWriter.Get();
            eventWriter(writer);

            if (!Game.Authoritative || writer.Size == 0
                || writer.Size > Game.Settings.MaxStateChangeSize ||
                (Game.Time - _lastStateChange) < Game.Settings.MaxStateChangeFrequency)
                return false;

            _args.Gamemode = this;
            _args.State = writer.Data;
            OnGamemodeStateChanged(this, _args);

            writer.Release();
            return true;
        }

        public void ReceiveStateData(byte[] state)
        {
            var rdr = ByteArrayReader.Get(state);
            ReceiveStateData(rdr);
            rdr.Release();
        }
        public void ReceiveStateData(ByteArrayReader reader)
        {
            if (GlobalSettings.Debug)
                ReceiveStateDataInternal(reader);
            else
                try
                {
                    ReceiveStateDataInternal(reader);
                }
                catch (Exception ex)
                {
                    Game.Logger.Error($"Gamemode state processing failed! Gamemode name: {ReflectionName}", ex);
                }
        }
        protected virtual void ReceiveStateDataInternal(ByteArrayReader reader) { }
        #endregion

        public byte[] GetFullState()
        {
            var writer = ByteArrayWriter.Get();
            GetFullState(writer);
            return writer.ReleaseAndReturnData();
        }
        public void GetFullState(ByteArrayWriter writer)
        {
            writer.Write(ReflectionName);
            writer.Write(GameEnded);
            writer.Write(AllowRespawn);
            writer.Write(RespawnTime);
            writer.Write(WinningTeam.TeamId);
            writer.Write((ushort)Teams.Length);
            foreach (var team in Teams)
            {
                writer.Write(team.TeamId);
                writer.Write(team.TeamColor);
                writer.Write((ushort)team.Players.Length);
                writer.Write(team.TeamName);
                writer.Write(team.Objective);
            }

            GetPrivateStateData(writer);
        }

        protected virtual void GetPrivateStateData(ByteArrayWriter writer)
        { }

        public void SetFullState(ByteArrayReader reader)
        {
            SetFullStateHeader(reader);

            if (GlobalSettings.Debug)
                SetFullStateInternal(reader);
            else
                try
                {
                    SetFullStateInternal(reader);
                }
                catch (Exception ex)
                {
                    Game.Logger.Error($"Gamemode full state parsing failed! Gamemode name: {ReflectionName}", ex);
                }
        }

        private void SetFullStateHeader(ByteArrayReader reader)
        {
            var reflectionName = reader.ReadString();
            GameEnded = reader.ReadBool();
            AllowRespawn = reader.ReadBool();
            RespawnTime = reader.ReadTimeSpan();
            var winningTeamId = reader.ReadShort();
            var teamCount = reader.ReadUShort();

            var teams = new List<Team>();
            if (Teams != null) teams.AddRange(Teams);
            for (var i = 0; i < teamCount; i++)
            {
                var t = MakeFullStateTeam(reader);
                bool teamIdExists = false;
                foreach (var team in Teams)
                    if (team.TeamId == t.TeamId)
                        teamIdExists = true;
                if (!teamIdExists) teams.Add(t);
            }

            if (winningTeamId == Team.Indeterminate.TeamId)
                WinningTeam = Team.Indeterminate;
            else if (winningTeamId == Team.Null.TeamId)
                WinningTeam = Team.Null;
            else
                foreach (var team in teams)
                    if (team.TeamId == winningTeamId)
                        WinningTeam = team;

            Teams = teams.ToArray();
        }

        private Team MakeFullStateTeam(ByteArrayReader reader)
        {
            short id = reader.ReadShort();
            var color = reader.ReadColor();
            var playerCount = reader.ReadUShort();
            var teamName = reader.ReadString();
            var objective = reader.ReadString();
            return new Team()
            {
                TeamId = id,
                TeamColor = color,
                Players = new GamePlayer[playerCount],
                Objective = objective,
                TeamName = teamName
            };
        }

        protected virtual void SetFullStateInternal(ByteArrayReader reader) { }

        public virtual void DeferredSetFullState() { }

        #region Hot Join
        public virtual bool HotJoinCanPlayerJoin(GamePlayer player) => HotJoinEnabled;
        public virtual Team HotJoinGetPlayerTeam(GamePlayer player) => null;
        public virtual string[] HotJoinGetAllowedTankTypes(GamePlayer player) =>
            GetPlayerAllowedTankTypes(player);
        public virtual bool HotJoinCheckPlayerSelectionValid(GamePlayer player, string tankType)
        {
            if (GetPlayerAllowedTankTypes(player).Contains(tankType))
            {
                player.SelectedTankReflectionName = tankType;
                return true;
            }
            return false;
        }
        #endregion

        #region Static initialization
        private static Dictionary<string, Type> _gamemodeTypes =
            new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);
        public static IReadOnlyDictionary<string, Type> AvailableTypes => _gamemodeTypes;

        public static Gamemode ReflectiveInitialize(string gamemodeName, GameCore game = null, byte[] state = null)
        {
            if (!_gamemodeTypes.ContainsKey(gamemodeName.ToLower())) throw new Exception("Gamemode type does not exist.");

            var inst = (Gamemode)Activator.CreateInstance(_gamemodeTypes[gamemodeName.ToLower()]);
            if (game != null) inst.SetGame(game);
            if (state != null) inst.ReceiveStateData(state);

            return inst;
        }

        public static T ReflectiveInitialize<T>(string gamemodeName, GameCore game = null, byte[] state = null)
        where T : Gamemode
        {
            return (T)ReflectiveInitialize(gamemodeName, game, state);
        }

        private static void RegisterType<T>(Module module) where T : Gamemode
        {
            //get the name
            var name = module.Name + "+" + typeof(T).Name;
            if (_gamemodeTypes.ContainsKey(name)) throw new Exception("Already registered!");

            _gamemodeTypes.Add(name.ToLower(), typeof(T));
        }

        public static ICollection<string> GetAllGamemodeTypes()
        {
            return _gamemodeTypes.Keys;
        }
        #endregion

        static Gamemode()
        {
        }
    }
}
