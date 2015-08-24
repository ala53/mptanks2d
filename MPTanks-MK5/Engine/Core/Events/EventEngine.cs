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

        #region 'Game' Events
        private TickEventArgs _tickEventArgs = new TickEventArgs();
        private event EventHandler<TickEventArgs> _gameTick = delegate { };
        public event EventHandler<TickEventArgs> OnGameTick
        {
            add { _gameTick += value; }
            remove { _gameTick -= value; }
        }

        internal void RaiseOnUpdate(GameTime gameTime)
        {
            _tickEventArgs.TickTime = gameTime;
            _gameTick(Game, _tickEventArgs);
        }


        private event EventHandler _gameStarted;
        public event EventHandler OnGameStarted
        {
            add { _gameStarted += value; }
            remove { _gameStarted -= value; }
        }

        internal void RaiseGameStarted()
        {
            _gameStarted(Game, null);
        }

        private EndedEventArgs _gameEndedArgs = new EndedEventArgs();
        private event EventHandler<EndedEventArgs> _gameEnded = delegate { };
        public event EventHandler<EndedEventArgs> OnGameEnded
        {
            add { _gameEnded += value; }
            remove { _gameEnded -= value; }
        }

        internal void RaiseGameStarted(Gamemodes.Team winningTeam)
        {
            _gameEndedArgs.WinningTeam = winningTeam;
            _gameEnded(Game, _gameEndedArgs);
        }

        public event EventHandler<GameCore.TimescaleValue> OnGameTimescaleChanged = delegate { };

        internal void RaiseTimescaleChanged(GameCore.TimescaleValue scale) =>
            OnGameTimescaleChanged(Game, scale);
        #endregion

        #region 'GameObject' Events
        private DestroyedEventArgs _gameObjectDestroyedArgs = new DestroyedEventArgs();
        private event EventHandler<DestroyedEventArgs> _gameObjectDestroyed = delegate { };
        public event EventHandler<DestroyedEventArgs> OnGameObjectDestroyed
        {
            add { _gameObjectDestroyed += value; }
            remove { _gameObjectDestroyed -= value; }
        }

        public void RaiseGameObjectDestroyed(GameObject destroyed, GameObject destroyer = null)
        {
            _gameObjectDestroyedArgs.Destroyed = destroyed;
            _gameObjectDestroyedArgs.Destroyer = destroyer;
            _gameObjectDestroyedArgs.Time = DateTime.UtcNow;

            _gameObjectDestroyed(destroyed, _gameObjectDestroyedArgs);
        }

        private event EventHandler<StateChangedEventArgs> _gameObjectStateChanged = delegate { };
        public event EventHandler<StateChangedEventArgs> OnGameObjectStateChanged
        {
            add { _gameObjectStateChanged += value; }
            remove { _gameObjectStateChanged -= value; }
        }

        public void RaiseGameObjectStateChanged(StateChangedEventArgs args)
        {
            _gameObjectStateChanged(args.Object, args);
        }

        public event EventHandler<GameObject.BasicPropertyChangeArgs> OnGameObjectBasicPropertyChanged = delegate { };

        public void RaiseGameObjectBasicPropertyChanged(GameObject.BasicPropertyChangeArgs args)
        {
            OnGameObjectBasicPropertyChanged(args.Owner, args);
        }

        public event EventHandler<GameObject> OnGameObjectCreated = delegate { };

        public void RaiseGameObjectCreated(GameObject obj)
        {
            OnGameObjectCreated(obj, obj);
        }
        #endregion
    }
}
