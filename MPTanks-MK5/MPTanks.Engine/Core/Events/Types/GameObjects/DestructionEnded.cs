﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Core.Events.Types.GameObjects
{
    public class DestructionEnded : EventArgs
    {
        public GameObject Object { get; set; }
    }
}