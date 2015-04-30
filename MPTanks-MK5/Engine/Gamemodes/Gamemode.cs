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
        public abstract int MinPlayerCount { get; }
        public bool AllowRespawn { get; protected set; }
        public float RespawnTimeMs { get; protected set; }

        //We cache the info for performance. Multiple calls only create one instance
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

        /// <summary>
        /// An event that is fired when the gamemode updates and changes state
        /// </summary>
        public event EventHandler<GamemodeChangedArgs> OnGamemodeStateChanged;

        public class GamemodeChangedArgs : EventArgs
        {
            public byte[] NewStateData;
        }
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
        public abstract void MakeTeams(Guid[] playerIds);

        /// <summary>
        /// Gets the tank type for a single player.
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public abstract PlayerTankType GetAssignedTankType(Guid playerId);

        /// <summary>
        /// Notifies the Gamemode that the game has started. So, it can do whatever it wants 
        /// related to that.
        /// </summary>
        public abstract void StartGame();
        /// <summary>
        /// Lets the game mode run its internal logic for players
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Sets the tank associated to a specific player.
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="tank"></param>
        public virtual void SetTank(Guid playerId, Tanks.Tank tank)
        {
            foreach (var team in Teams)
                for (var i = 0; i < team.Players.Length; i++)
                    if (team.Players[i].PlayerId == playerId)
                    {
                        //Simply do an in place replace and assign the new tank to 
                        var tmp = team.Players[i];
                        tmp.Tank = tank;
                        tank.Team = team;
                        team.Players[i] = tmp;
                        return;
                    }

        }

        public Team GetTeam(Guid playerId)
        {
            foreach (var team in Teams)
                for (var i = 0; i < team.Players.Length; i++)
                    if (team.Players[i].PlayerId == playerId)
                        return team;

            return null;
        }

        public int GetTeamIndex(Guid playerId)
        {
            var t = GetTeam(playerId);
            for (var i = 0; i < Teams.Length; i++)
                if (Teams[i] == t)
                    return i;

            return int.MaxValue;
        }

        public virtual void ReceiveStateData(byte[] data)
        {
        }

        public event EventHandler<Core.Events.Types.Gamemodes.StateChanged> StateChanged;

        protected void RaiseStateChanged(byte[] data)
        {
            if (StateChanged != null)
                StateChanged(this, new Core.Events.Types.Gamemodes.StateChanged() { StateData = data });
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

        public static T ReflectiveInitialize<T>(string tankName, GameCore game = null, byte[] state = null)
        where T : Gamemode
        {
            return (T)ReflectiveInitialize(tankName, game, state);
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
            Mods.CoreModLoader.LoadTrustedMods();
        }
    }
}
