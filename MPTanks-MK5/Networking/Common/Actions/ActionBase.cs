using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions
{
    public abstract class ActionBase : MessageBase
    {
        public float GameTimeMilliseconds { get; set; }
        static ActionBase()
        {
            RegisterToClientActionType(typeof(ToClient.FullGameStateSentAction));
            RegisterToClientActionType(typeof(ToClient.GameCreatedAction));
            RegisterToClientActionType(typeof(ToClient.GameEndedAction));
            RegisterToClientActionType(typeof(ToClient.GamemodeFullStateSentAction));
            RegisterToClientActionType(typeof(ToClient.GamemodeStateChangedAction));
            RegisterToClientActionType(typeof(ToClient.GameObjectCreatedAction));
            RegisterToClientActionType(typeof(ToClient.GameObjectDestroyedAction));
            RegisterToClientActionType(typeof(ToClient.GameStartedAction));
            RegisterToClientActionType(typeof(ToClient.GameStatusChangedAction));
            RegisterToClientActionType(typeof(ToClient.ObjectStateChangedAction));
            RegisterToClientActionType(typeof(ToClient.OtherPlayerReadyToStartChangedAction));
            RegisterToClientActionType(typeof(ToClient.OtherPlayerSelectedTankAction));
            RegisterToClientActionType(typeof(ToClient.PlayerAllowedTankTypesSentAction));
            RegisterToClientActionType(typeof(ToClient.PlayerJoinedAction));
            RegisterToClientActionType(typeof(ToClient.PlayerKilledAction));
            RegisterToClientActionType(typeof(ToClient.PlayerLeftAction));
            RegisterToClientActionType(typeof(ToClient.PlayerPropertyChangedAction));
            RegisterToClientActionType(typeof(ToClient.PlayersListSentAction));
            RegisterToClientActionType(typeof(ToClient.PlayerTankAssignedAction));
            RegisterToClientActionType(typeof(ToClient.ReceivedChatMessageAction));
            RegisterToClientActionType(typeof(ToClient.TeamsCreatedAction));
            
            RegisterToServerActionType(typeof(ToServer.InputChangedAction));
            RegisterToServerActionType(typeof(ToServer.PlayerTankTypeSelectedAction));
            RegisterToServerActionType(typeof(ToServer.RequestFullGameStateAction));
            RegisterToServerActionType(typeof(ToServer.SentChatMessageAction));
        }
        
        public ActionBase(Lidgren.Network.NetIncomingMessage message) { }

        public ActionBase() { }

        public static void RegisterToClientActionType(Type actionType)
        {
            NetworkProcessorBase.RegisterToClientActionType(actionType);
        }

        public static void RegisterToServerActionType(Type actionType)
        {
            NetworkProcessorBase.RegisterToServerActionType(actionType);
        }
    }
}
