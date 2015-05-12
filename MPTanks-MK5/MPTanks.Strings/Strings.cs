using MPTanks.StringData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks
{
    public static class Strings
    {
        public static readonly dynamic ClientMenus = new StringTable(
            Path.Combine("assets", "strings", "clientmenus.{0}.txt"));
        public static readonly dynamic Engine = new StringTable(
            Path.Combine("assets", "strings", "engine.{0}.txt"));
    }
}
