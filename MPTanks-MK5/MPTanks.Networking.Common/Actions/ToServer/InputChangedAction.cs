﻿using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToServer
{
    public class InputChangedAction : ActionBase
    {
        public InputChangedAction(NetIncomingMessage message):base(message)
        {

        }

        public override void Serialize(NetOutgoingMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
