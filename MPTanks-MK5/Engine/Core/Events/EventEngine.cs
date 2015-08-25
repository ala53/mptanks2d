using Microsoft.Xna.Framework;
using MPTanks.Engine.Core.Events.Types;
using MPTanks.Engine.Core.Events.Types.GameCore;
using MPTanks.Engine.Core.Events.Types.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Core.Events
{
    public class EventEngine
    {
        public GameCore Game { get; private set; }
        public EventEngine(GameCore game)
        {
            Game = game;
        }

        private bool _eventsEnabled = true;
        public bool UnsafeDisableEvents() => _eventsEnabled = false;
        public bool UnsafeEnableEvents() => _eventsEnabled = true;

        #region 'Game' Events
        private TickEventArgs _tickEventArgs = new TickEventArgs();
        private event EventHandler<TickEventArgs> OnGameTick = delegate { };

        internal void RaiseGameUpdate(GameTime gameTime)
        {
            _tickEventArgs.TickTime = gameTime;
            if (_eventsEnabled)
                OnGameTick(Game, _tickEventArgs);
        }


        public event EventHandler OnGameStarted = delegate { };

        internal void RaiseGameStarted()
        {
            if (_eventsEnabled)
                OnGameStarted(Game, null);
        }

        private EndedEventArgs _gameEndedArgs = new EndedEventArgs();
        public event EventHandler<EndedEventArgs> OnGameEnded = delegate { };
        internal void RaiseGameEnded(Gamemodes.Team winningTeam)
        {
            _gameEndedArgs.WinningTeam = winningTeam;
            if (_eventsEnabled)
                OnGameEnded(Game, _gameEndedArgs);
        }

        public event EventHandler<GameCore.TimescaleValue> OnGameTimescaleChanged = delegate { };

        internal void RaiseGameTimescaleChanged(GameCore.TimescaleValue scale)
        {
            if (_eventsEnabled) OnGameTimescaleChanged(Game, scale);
        }

        public event EventHandler<bool> OnGameCanRunChanged = delegate { };
        internal void RaiseGameCanRunChanged()
        {
            if (_eventsEnabled)
                OnGameCanRunChanged(Game, Game.CanRun);
        }
        #endregion

        #region 'GameObject' Events
        private DestroyedEventArgs _gameObjectDestroyedArgs = new DestroyedEventArgs();
        public event EventHandler<DestroyedEventArgs> OnGameObjectDestroyed = delegate { };

        internal void RaiseGameObjectDestroyed(GameObject destroyed, GameObject destroyer = null)
        {
            _gameObjectDestroyedArgs.Destroyed = destroyed;
            _gameObjectDestroyedArgs.Destroyer = destroyer;
            _gameObjectDestroyedArgs.Time = DateTime.UtcNow;

            if (_eventsEnabled)
                OnGameObjectDestroyed(Game, _gameObjectDestroyedArgs);
        }

        public event EventHandler<StateChangedEventArgs> OnGameObjectStateChanged = delegate { };

        internal void RaiseGameObjectStateChanged(StateChangedEventArgs args)
        {
            if (_eventsEnabled)
                OnGameObjectStateChanged(Game, args);
        }

        public event EventHandler<GameObject.BasicPropertyChangeArgs> OnGameObjectBasicPropertyChanged = delegate { };

        internal void RaiseGameObjectBasicPropertyChanged(GameObject.BasicPropertyChangeArgs args)
        {
            if (_eventsEnabled)
                OnGameObjectBasicPropertyChanged(Game, args);
        }

        public event EventHandler<GameObject> OnGameObjectCreated = delegate { };

        internal void RaiseGameObjectCreated(GameObject obj)
        {
            if (_eventsEnabled)
                OnGameObjectCreated(Game, obj);
        }

        public event EventHandler<GameObject> OnGameObjectDestructionEnded = delegate { };
        internal void RaiseGameObjectDestructionEnded(GameObject obj)
        {
            if (_eventsEnabled) OnGameObjectDestructionEnded(Game, obj);
        }
        #endregion

        #region Gamemode
        public event EventHandler<byte[]> OnGamemodeStateChanged = delegate { };
        internal void RaiseGamemodeStateChanged(byte[] state)
        {
            if (_eventsEnabled)
                OnGamemodeStateChanged(Game, state);
        }
        #endregion
    }
}
