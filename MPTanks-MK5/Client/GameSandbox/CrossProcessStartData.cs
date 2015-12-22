using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.GameSandbox
{
    public class CrossProcessStartData 
    {

        public CrossProcessStartData()
        {
        }

        public int WindowPositionX { get; set; }
        public int WindowPositionY { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public bool SandboxingEnabled { get; set; }

        public bool IsGameHost { get; set; }

        public string ServerEngineSettingsJSON { get; set; }

        public string DrmEngineSerializedInfo { get; set; }

        public string ConnectionFailureCause { get; set; }

        public string Ip { get; set; }
        public ushort Port { get; set; }
        public string Password { get; set; }
    }


}
