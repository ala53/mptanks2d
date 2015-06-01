using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Gamemodes
{
    public abstract class Gamemode
    {
        public GameCore Game { get; private set; }
        public abstract bool GameEnded { get; }
        public virtual Team WinningTeam { get { return Team.Null; } }
        public abstract Team[] Teams { get; }
        public bool AllowRespawn { get; protected set; }
        public float RespawnTimeMs { get; protected set; }

        //We cache the info for performance. Multiple requests only do one call
        private string _cachedReflectionInfo;
        public string ReflectionName
        {
            get
            {
                //Because it's a requirement to have ReflectionTypeName, we do a reflection query on ourselves
                //to get the static property and then we cache the delegate before returning the data for the 
                //reflectiontypename property
                if (_cachedReflectionInfo == null)
                    _cachedReflectionInfo = ((MPTanks.Modding.GameObjectAttribute)(GetType().
                         GetCustomAttributes(typeof(MPTanks.Modding.GameObjectAttribute), true))[0]).ReflectionTypeName;

                //call the delegate
                return _cachedReflectionInfo;
            }
        }

        private int _cachedPlrCt = -799023;
        public int MinPlayerCount
        {
            get
            {
                if (_cachedPlrCt == -799023)
                    _cachedPlrCt = ((MPTanks.Modding.GamemodeAttribute)(GetType().
                         GetCustomAttributes(typeof(MPTanks.Modding.GamemodeAttribute), true))[0]).MinPlayersCount;

                return _cachedPlrCt;
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
        public abstract void StartGame();
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

        public virtual void ReceiveStateData(byte[] data)
        {
        }

        private Core.Events.Types.Gamemodes.StateChangedEventArgs _args = 
            new Core.Events.Types.Gamemodes.StateChangedEventArgs();
        protected void RaiseStateChanged(byte[] data)
        {
            _args.Gamemode = this;
            _args.State = data;
                OnGamemodeStateChanged(this, _args);
        }


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
