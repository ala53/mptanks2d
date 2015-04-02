using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public class GamePlayer
    {
        public string SelectedTankReflectionName { get; set; }
        public bool HasSelectedTankYet { get; set; }
        public Guid Id { get; set; }
        public string[] AllowedTankTypes { get; set; }
        public Tanks.Tank Tank { get; set; }
    }
}
