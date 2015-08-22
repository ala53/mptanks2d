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
        public virtual Team[] Teams { get; protected set; } = new Team[0];
        public bool AllowRespawn { get; protected set; }
        public float RespawnTimeMs { get; protected set; }

        public byte[] FullState { get { return GetFullState(); } set { SetFullState(value); } }

        #region Reflection helper
        //We cache the info for performance. Multiple calls only create one instance
        private string _cachedReflectionName;
        public string ReflectionName
        {
            get
            {
                if (_cachedReflectionName == null)
                    _cachedReflectionName = ((GameObjectAttribute[])(GetType()
                          .GetCustomAttributes(typeof(GameObjectAttribute), true)))[0]
                          .ReflectionTypeName;

                return _cachedReflectionName;
            }
        }
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
                return ModDatabase.ReverseTypeTable[GetType()];
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
                    _defaultTankReflectionName = ((Modding.GamemodeAttribute)(GetType().
                         GetCustomAttributes(typeof(Modding.GamemodeAttribute), true))[0]).DefaultTankTypeReflectionName;

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
        public abstract bool SetPlayerTankType(GamePlayer player, string tankType);

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
        protected bool RaiseStateChangeEvent(byte[] newStateData)
        {
            if (!Game.Authoritative || newStateData == null || newStateData.Length == 0
                || newStateData.Length > Game.Settings.MaxStateChangeSize ||
                (Game.Time - _lastStateChange).TotalMilliseconds < Game.Settings.MaxStateChangeFrequency)
                return false;

            _args.Gamemode = this;
            _args.State = newStateData;
            OnGamemodeStateChanged(this, _args);

            return true;
        }

        protected bool RaiseStateChangeEvent(string state)
        {
            var count = Encoding.UTF8.GetByteCount(state);
            return RaiseStateChangeEvent(SerializationHelpers.Serialize(state));
        }

        /// <summary>
        /// Serializes the object to JSON before sending it.
        /// </summary>
        /// <param name="obj"></param>
        protected bool RaiseStateChangeEvent(object obj)
        {
            return RaiseStateChangeEvent(SerializationHelpers.Serialize(obj));
        }

        public void ReceiveStateData(byte[] stateData)
        {
            if (GlobalSettings.Debug)
                SerializationHelpers.ResolveDeserialize(stateData,
                     ReceiveStateDataInternal, ReceiveStateDataInternal, ReceiveStateDataInternal);
            else
                try
                {
                    SerializationHelpers.ResolveDeserialize(stateData,
                         ReceiveStateDataInternal, ReceiveStateDataInternal, ReceiveStateDataInternal);
                }
                catch (Exception ex)
                {
                    Game.Logger.Error($"Gamemode state parsing failed! Gamemode name: {ReflectionName}", ex);
                    ReceiveStateDataInternal(stateData);
                }
        }
        protected virtual void ReceiveStateDataInternal(byte[] stateData) { }

        protected virtual void ReceiveStateDataInternal(dynamic obj) { }

        protected virtual void ReceiveStateDataInternal(string state) { }
        #endregion

        public byte[] GetFullState()
        {
            //Header:
            // 2 byte reflection name length
            // variable reflection name string
            // 1 byte game ended
            // 1 byte allow respawn
            // 4 byte respawn time milliseconds
            // 1 bytes winning team id
            // 1 byte teams count
            //        for each team
            //          2 byte team object size
            //          1 byte team id
            //          4 byte team color 
            //          2 byte player count
            //          2 byte team name length
            //          variable team name string
            //          2 byte team objective length
            //          variable team objective string
            // 2 byte private size
            // variable private data

            byte[] privateStateBytes = SerializationHelpers.Serialize(GetPrivateStateData());

            //then encode the header

            var header = SerializationHelpers.AllocateArray(true,
                ReflectionName,
                GameEnded,
                AllowRespawn,
                RespawnTimeMs,
                WinningTeam.TeamId,
                (ushort)Teams.Length);

            foreach (var team in Teams)
                header = SerializationHelpers.MergeArrays(header, EncodeTeam(team));

            //And then the contents
            return SerializationHelpers.MergeArrays(header, privateStateBytes, true);
        }

        private byte[] EncodeTeam(Team team)
        {
            return SerializationHelpers.AllocateArray(true,
                team.TeamId,
                team.TeamColor,
                (ushort)team.Players.Length,
                team.TeamName,
                team.Objective);
        }

        /// <summary>
        /// Return a byte array for optimal performance, or either a string or other random object for ease of use.
        /// </summary>
        /// <returns></returns>
        protected virtual object GetPrivateStateData()
        {
            return SerializationHelpers.EmptyByteArray;
        }

        public void SetFullState(byte[] stateData)
        {
            int offset = 0;
            SetFullStateHeader(stateData, ref offset);

            var privateState = stateData.GetByteArray(offset);

            if (GlobalSettings.Debug)
                SerializationHelpers.ResolveDeserialize(stateData,
                    SetFullStateInternal, SetFullStateInternal, SetFullStateInternal);
            else
                try
                {
                    SerializationHelpers.ResolveDeserialize(stateData,
                        SetFullStateInternal, SetFullStateInternal, SetFullStateInternal);
                }
                catch (Exception ex)
                {
                    Game.Logger.Error($"Gamemode full state parsing failed! Gamemode name: {ReflectionName}", ex);
                    SetFullStateInternal(privateState);
                }
        }

        private void SetFullStateHeader(byte[] header, ref int offset)
        {
            var reflectionName = header.GetString(offset); offset += header.GetUShort(offset); offset += 2;
            var ended = header.GetBool(offset); offset++;
            var allowRespawn = header.GetBool(offset); offset++;
            var respawnTime = header.GetFloat(offset); offset += 4;
            var winningTeamId = header.GetShort(offset); offset += 2;
            var teamCount = header.GetUShort(offset); offset += 2;

            var teams = new List<Team>();
            if (Teams != null) teams.AddRange(Teams);
            for (var i = 0; i < teamCount; i++)
            {
                var t = MakeFullStateTeam(header, ref offset);
                bool teamIdExists = false;
                foreach (var team in Teams)
                    if (team.TeamId == t.TeamId)
                        teamIdExists = true;
                if (!teamIdExists) teams.Add(t);
            }
            GameEnded = ended;
            AllowRespawn = allowRespawn;
            RespawnTimeMs = respawnTime;

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

        private Team MakeFullStateTeam(byte[] header, ref int offset)
        {
            short id = header.GetShort(offset); offset += 2;
            var color = header.GetColor(offset); offset += 4;
            var playerCount = header.GetUShort(offset); offset += 2;
            var teamName = header.GetString(offset); offset += header.GetUShort(offset); offset += 2;
            var objective = header.GetString(offset); offset += header.GetUShort(offset); offset += 2;
            return new Team()
            {
                TeamId = id,
                TeamColor = color,
                Players = new GamePlayer[playerCount],
                Objective = objective,
                TeamName = teamName
            };
        }

        protected virtual void SetFullStateInternal(byte[] stateData) { }

        protected virtual void SetFullStateInternal(string state) { }

        protected virtual void SetFullStateInternal(dynamic state) { }

        public virtual void DeferredSetFullState() { }

        #region Hot Join
        public virtual bool HotJoinCanPlayerJoin(GamePlayer player) => HotJoinEnabled;
        public virtual Team HotJoinGetPlayerTeam(GamePlayer player) => null;
        public virtual string[] HotJoinGetAllowedTankTypes(GamePlayer player) =>
            GetPlayerAllowedTankTypes(player);
        public virtual bool HotJoinSetPlayerTankType(GamePlayer player, string tankType)
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
            new Dictionary<string, Type>();

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

        private static void RegisterType<T>() where T : Gamemode
        {
            //get the name
            var name = ((MPTanks.Modding.GameObjectAttribute)(typeof(T).
                GetCustomAttributes(typeof(MPTanks.Modding.GameObjectAttribute), true))[0]).ReflectionTypeName;
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
