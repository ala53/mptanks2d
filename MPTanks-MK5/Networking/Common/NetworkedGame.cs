﻿using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Engine.Gamemodes;
using MPTanks.Engine.Logging;
using MPTanks.Engine.Settings;
using MPTanks.Modding;
using MPTanks.Networking.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common
{
    /// <summary>
    /// A networked game. It contains the core properties and methods that
    /// both the server host and the client have access to.
    /// </summary>
    public class NetworkedGame
    {
        #region Properties
        public EngineSettings EngineSettings { get; set; } = EngineSettings.GetInstance();

        public FullGameState FullGameState
        {
            get { return FullGameState.Create(Game); }
            set
            {
                Game = value.CreateGameFromState(Logger, EngineSettings);
                Game.Authoritative = _authoritative;
                var oldGame = Game;
                GameChanged(this, new GameChangedArgs() { OldGame = oldGame, Game = Game });
            }
        }

        private PseudoFullGameWorldState _pseudoState = new PseudoFullGameWorldState();
        public PseudoFullGameWorldState CurrentState
        {
            get
            {
                return _pseudoState;
            }
            set
            {
                _pseudoState = value;
                _pseudoState.Apply(Game);
            }
        }
        public Engine.GameCore Game { get; private set; }
        public Engine.Diagnostics Diagnostics { get { return Game.Diagnostics; } }
        public ILogger Logger { get; set; }
        private bool _authoritative;
        public bool Authoritative
        {
            get { return Game.Authoritative; }
            set { _authoritative = value; Game.Authoritative = value; }
        }
        #endregion

        #region Events
        public event EventHandler<GameChangedArgs> GameChanged = delegate { };
        public struct GameChangedArgs
        {
            public GameCore OldGame { get; set; }
            public GameCore Game { get; set; }
        }
        #endregion

        public NetworkedGame(bool authoritative, Gamemode gamemode, ILogger gameLogger,
            ModAssetInfo mapData, EngineSettings settings = null)
        {
            Game = new Engine.GameCore(new NullLogger(), gamemode, mapData, settings);
            if (!authoritative) Game.BeginGame(false);
            Authoritative = authoritative;
        }

        public NetworkedGame(FullGameState fullState, ILogger gameLogger = null, EngineSettings settings = null)
        {
            if (fullState != null)
                Game = fullState.CreateGameFromState(gameLogger, settings);
            else Game = new GameCore(gameLogger, new NullGamemode(), new ModAssetInfo());
            Game.Authoritative = false;
        }

        public PseudoFullGameWorldState GetDeltaState(PseudoFullGameWorldState lastSentState) =>
            CurrentState.MakeDelta(lastSentState);

        #region Timing Management
        private double totalMilliseconds;
        private GameTime _gt = new GameTime();
        public void Tick(double milliseconds)
        {
            totalMilliseconds += milliseconds;
            _gt.ElapsedGameTime = TimeSpan.FromMilliseconds(milliseconds);
            _gt.TotalGameTime = TimeSpan.FromMilliseconds(totalMilliseconds);
            Tick(_gt);
        }
        public virtual void Tick(GameTime gameTime)
        {
            totalMilliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;
            TickGameState(gameTime);
        }
        #endregion
        #region Game state ticking
        private void TickGameState(GameTime gameTime)
        {
            Diagnostics.BeginMeasurement("TickGameState()", "Network Core");
            Game.Update(gameTime);
            Diagnostics.EndMeasurement("TickGameState()", "Network Core");
        }
        #endregion
    }
}
