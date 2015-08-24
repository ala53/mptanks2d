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
        public static readonly StringTable _ClientMenus = new StringTable(
            Path.Combine("assets", "strings", "clientmenus.{0}.txt"));
        public static readonly dynamic ClientMenus = _ClientMenus;

        public static readonly StringTable _Engine = new StringTable(
            Path.Combine("assets", "strings", "engine.{0}.txt"));
        public static readonly dynamic Engine = _Engine;

        public static readonly StringTable _Server = new StringTable(
            Path.Combine("assets", "strings", "server.{0}.txt"));
        public static readonly dynamic Server = _Server;

        const string notFoundError = "ERROR_TABLE_DOES_NOT_EXIST";
        public static string Format(string tableName, string name, params string[] arguments)
        {
            var fields = typeof(Strings).GetFields(
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            System.Reflection.FieldInfo fld = null;

            foreach (var field in fields)
                if (field.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase))
                {
                    fld = field;
                    continue;
                }

            if (fld == null)
                return notFoundError;

            StringTable table = (StringTable)fld.GetValue(null);

            return Format(table, name, arguments);
        }
        public static string Format(StringTable table, string name, params string[] arguments)
        {
            return String.Format(table.GetByName(name), arguments);
        }
    }
}
