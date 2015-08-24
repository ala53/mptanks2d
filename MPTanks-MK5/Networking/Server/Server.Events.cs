using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server
{
    public partial class Server
    {
        public void HookEvents()
        {
            GameInstance.GameChanged += (s, e) =>
             {
                 if (e.OldGame != null)
                 {

                 }
                 e.Game.EventEngine.OnGameEnded += Game_Ended;
                 e.Game.EventEngine.OnGameStarted += Game_Started;
                 e.Game.EventEngine.OnGameTimescaleChanged += Game_TimescaleChanged;

                 e.Game.EventEngine.OnGameObjectDestroyed += GameObject_Destroyed;
                 e.Game.EventEngine.OnGameObjectStateChanged += GameObject_StateChanged;
                 e.Game.EventEngine.OnGameObjectBasicPropertyChanged += GameObject_BasicPropertyChanged;
                 e.Game.EventEngine.OnGameObjectCreated += GameObject_Created;

                 //And send out the game state
                 MessageProcessor.SendMessage(new Common.Actions.ToClient.GameCreatedAction());
                 MessageProcessor.SendMessage(new Common.Actions.ToClient.FullGameStateSentAction(GameInstance.Game));
             };
        }

        private void GameObject_Created(object sender, GameObject e)
        {
            MessageProcessor.SendMessage(new Common.Actions.ToClient.GameObjectCreatedAction(e));
        }

        private void GameObject_BasicPropertyChanged(object sender, GameObject.BasicPropertyChangeArgs e)
        {
            MessageProcessor.SendMessage(new Common.Actions.ToClient.ObjectBasicPropertyChangedAction(e.Type, e));
        }

        private void GameObject_StateChanged(object sender, Engine.Core.Events.Types.GameObjects.StateChangedEventArgs e)
        {
            MessageProcessor.SendMessage(new Common.Actions.ToClient.ObjectStateChangedAction(e.Object, e.State));
        }

        private void Game_TimescaleChanged(object sender, Engine.GameCore.TimescaleValue e)
        {
            MessageProcessor.SendMessage(new Common.Actions.ToClient.TimescaleChangedAction(GameInstance.Game));
            MessageProcessor.SendMessage(new Common.Actions.ToClient.PartialGameStateUpdateAction(GameInstance.Game));
        }

        private void Game_Started(object sender, EventArgs e)
        {
        }

        private void GameObject_Destroyed(object sender, Engine.Core.Events.Types.GameObjects.DestroyedEventArgs e)
        {
            MessageProcessor.SendMessage(new Common.Actions.ToClient.GameObjectDestroyedAction(e.Destroyed));
        }

        private void Game_Ended(object sender, Engine.Core.Events.Types.GameCore.EndedEventArgs e)
        {
            MessageProcessor.SendMessage(new Common.Actions.ToClient.GameEndedAction());
        }
    }
}
