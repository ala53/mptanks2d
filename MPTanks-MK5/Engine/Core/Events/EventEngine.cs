using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Events
{
    public class EventEngine
    {
        public GameCore Game { get; private set; }
        public EventEngine(GameCore game)
        {
            Game = game;
        }

        #region 'Game' Events
        private event EventHandler<Types.Game.Tick> _gameTick;
        public event EventHandler<Types.Game.Tick> OnGameTick
        {
            add { _gameTick += value; }
            remove { _gameTick -= value; }
        }

        internal void RaiseOnUpdate(GameTime gameTime)
        {
            if (_gameTick != null)
                _gameTick(Game, new Types.Game.Tick() { Game = Game, Time = gameTime });
        }


        private event EventHandler<Types.Game.Started> _gameStarted;
        public event EventHandler<Types.Game.Started> OnGameStarted
        {
            add { _gameStarted += value; }
            remove { _gameStarted -= value; }
        }

        internal void RaiseGameStarted()
        {
            if (_gameStarted != null)
                _gameStarted(Game, new Types.Game.Started() { Game = Game, StartTime = DateTime.UtcNow });
        }

        private event EventHandler<Types.Game.Ended> _gameEnded;
        public event EventHandler<Types.Game.Ended> OnGameEnded
        {
            add { _gameEnded += value; }
            remove { _gameEnded -= value; }
        }

        internal void RaiseGameStarted(Gamemodes.Team winningTeam)
        {
            if (_gameEnded != null)
                _gameEnded(Game, new Types.Game.Ended()
                {
                    Game = Game,
                    GameIsDraw = (winningTeam == Gamemodes.Team.Indeterminate),
                    EndTime = DateTime.Now,
                    WinningTeam = winningTeam
                });
        }

        #endregion

        #region 'GameObject' Events
        private event EventHandler<Types.GameObjects.Destroyed> _gameObjectDestroyed;
        public event EventHandler<Types.GameObjects.Destroyed> OnGameObjectDestroyed
        {
            add { _gameObjectDestroyed += value; }
            remove { _gameObjectDestroyed -= value; }
        }

        public void RaiseGameObjectDestroyed(Types.GameObjects.Destroyed destroyedEvent)
        {
            if (_gameObjectDestroyed != null)
                _gameObjectDestroyed(destroyedEvent.DeadObject, destroyedEvent);
        }

        private event EventHandler<Types.GameObjects.StateChanged> _gameObjectStateChanged;
        public event EventHandler<Types.GameObjects.StateChanged> OnGameObjectStateChanged
        {
            add { _gameObjectStateChanged += value; }
            remove { _gameObjectStateChanged -= value; }
        }

        public void RaiseGameObjectStateChanged(Types.GameObjects.StateChanged stateChange)
        {
            if (_gameObjectStateChanged != null)
                _gameObjectStateChanged(stateChange.GameObject, stateChange);
        }
        #endregion
    }
}
