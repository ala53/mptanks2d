using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using MPTanks.Engine;
using MPTanks.Networking.Common.Game;
using MPTanks.Engine.Logging;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class FullGameStateSentAction : ActionBase
    {
        public FullGameState State { get; set; }
        public Engine.Settings.EngineSettings EngineSettings { get; set; }
        public FullGameStateSentAction(GameCore game) { State = FullGameState.Create(game); EngineSettings = game.Settings; }
        public FullGameStateSentAction() { }
        protected override void DeserializeInternal(NetIncomingMessage message)
        {
            State = FullGameState.Read(message);
            EngineSettings = new Engine.Settings.EngineSettings();
            EngineSettings.Load(message.ReadString());
        }
        public override void Serialize(NetOutgoingMessage message)
        {
            State.Write(message);
            message.Write(EngineSettings.Save());
        }

        public GameCore CreateGame(ILogger logger = null)
        {
            return State.CreateGameFromState(logger, EngineSettings);
        }
    }
}
