using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding.Compiliation.Verification
{
    public class WhitelistVerify
    {
        private static List<string> WhitelistedNamespaces = new List<string>(new[] {
        "System.Text",
        "System.Collections",
        "Engine",
        "System.Timers",
        "System.Globalization",
        "Newtonsoft.Json",
        "Microsoft.Xna.Framework",
        "Lidgren.Network",
        "Microsoft.VisualBasic",
        "MPTanks_M"
        });

        private static List<string> WhitelistedTypes = new List<string>(new[] { 
        "System.Nullable",
        "System.Object",
        "System.IDisposable",
        "System.String",
        "System.Math",
        "System.Enum",
        "System.ValueType",
        "System.Int16",
        "System.Int32",
        "System.Int64",
        "System.UInt16",
        "System.UInt32",
        "System.UInt64",
        "System.Double",
        "System.Single",
        "System.Boolean",
        "System.Byte",
        "System.SByte",
        "System.Char",
        "System.Decimal",
        "System.DateTime",
        "System.TimeSpan",
        "System.Array",
        "System.Runtime.CompilerServices.RuntimeHelpers",
        "System.IO.Stream",
        "System.IO.TextReader",
        "System.IO.TextWriter",
        "System.NullReferenceException",
        "System.ArgumentException",
        "System.ArgumentNullException",
        "System.InvalidOperationException",
        "System.FormatException",
        "System.Exception",
        "System.DivideByZeroException",
        "System.InvalidCastException",
        "System.IO.FileNotFoundException",
        "System.IO.Path",
        "System.Random",
        "System.Convert",
        "System.Nullable",
        "System.StringComparer",
        "System.IComparable",
        });

        /// <summary>
        /// Blacklisted types in whitelisted namespaces
        /// </summary>
        private static List<string> BlacklistedTypes = new List<string>(new[] { 
            "Engine.Logging.ILogger", //Don't let it thrash the HDD
            "Microsoft.Xna.Framework.Content.ContentManager" //Or load content from the hard drive
        });

        public static bool IsAssemblySafe(Assembly assembly, out string error)
        {
            error = "";
            bool hasBadTypes = false;
            foreach (var type in assembly.DefinedTypes)
            {
            }

            return true;
        }

        private bool IsTypeWhitelisted(Type type)
        {
            foreach (var ns in WhitelistedNamespaces)
                if (type.Namespace.StartsWith(ns))
                {
                    foreach (var t in BlacklistedTypes)
                        if (type.FullName == t)
                            return false; //blacklisted subtype]

                    return true; //in whitelisted namespace and not blacklisted
                }

            foreach (var t in WhitelistedTypes)
                if (type.FullName.StartsWith(t))
                    return true;

            return false;

        }
    }
}
