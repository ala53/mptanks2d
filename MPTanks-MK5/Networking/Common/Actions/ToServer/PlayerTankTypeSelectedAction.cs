using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToServer
{
    public class PlayerTankTypeSelectedAction : ActionBase
    {
        public string SelectedTypeReflectionName { get; private set; }
        public PlayerTankTypeSelectedAction()
        {
        }

        public PlayerTankTypeSelectedAction(string selectedTypeReflectionName)
        {
            SelectedTypeReflectionName = selectedTypeReflectionName;
        }
        protected override void DeserializeInternal(NetIncomingMessage message)
        {
            SelectedTypeReflectionName = message.ReadString();
        }
        public override void Serialize(NetOutgoingMessage message)
        {
            message.Write(SelectedTypeReflectionName);
        }
    }
}
