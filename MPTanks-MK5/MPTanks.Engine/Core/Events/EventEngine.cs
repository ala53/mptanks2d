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
        private event EventHandler<TickEventArgs> _gameTick;
        public event EventHandler<TickEventArgs> OnGameTick
        {
            add { _gameTick += value; }
            remove { _gameTick -= value; }
        }

        internal void RaiseOnUpdate(GameTime gameTime)
        {
            _tickEventArgs.TickTime = gameTime;
            if (_gameTick != null)
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
            if (_gameStarted != null)
                _gameStarted(Game, null);
        }

        private EndedEventArgs _gameEndedArgs = new EndedEventArgs();
        private event EventHandler<EndedEventArgs> _gameEnded;
        public event EventHandler<EndedEventArgs> OnGameEnded
        {
            add { _gameEnded += value; }
            remove { _gameEnded -= value; }
        }

        internal void RaiseGameStarted(Gamemodes.Team winningTeam)
        {
            _gameEndedArgs.WinningTeam = winningTeam;
            if (_gameEnded != null)
                _gameEnded(Game, _gameEndedArgs);
        }

        #endregion

        #region 'GameObject' Events
        private DestroyedEventArgs _gameObjectDestroyedArgs = new DestroyedEventArgs();
        private event EventHandler<DestroyedEventArgs> _gameObjectDestroyed;
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

            if (_gameObjectDestroyed != null)
                _gameObjectDestroyed(destroyed, _gameObjectDestroyedArgs);
        }
        
        private event EventHandler<StateChangedEventArgs> _gameObjectStateChanged;
        public event EventHandler<StateChangedEventArgs> OnGameObjectStateChanged
        {
            add { _gameObjectStateChanged += value; }
            remove { _gameObjectStateChanged -= value; }
        }

        public void RaiseGameObjectStateChanged(StateChangedEventArgs args)
        {
            if (_gameObjectStateChanged != null)
                _gameObjectStateChanged(args.Object, args);
        }
        #endregion
    }
}
