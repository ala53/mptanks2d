﻿using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Actions.ToClient
{
    public class TeamsCreatedAction : ActionBase
    {
        public TeamsCreatedAction(NetIncomingMessage message) : base(message)
        {

        }

        public override void Serialize(NetOutgoingMessage message)
        {
            throw new NotImplementedException();
        }
    }
}