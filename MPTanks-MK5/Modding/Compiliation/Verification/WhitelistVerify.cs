using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding.Compiliation.Verification
{
    public class WhitelistVerify
    {
        private static List<string> WhitelistedNamespaces = new List<string>(new[] {
        "System.Text",
        "System.Linq",
        "System.Collections",
        "MPTanks.Engine",
        "System.Timers",
        "System.Globalization",
        "Newtonsoft.Json",
        "Microsoft.Xna.Framework",
        "Lidgren.Network",
        "Microsoft.VisualBasic",
        "MPTanks.Modding",
        "Starbound.Input",
        "MPTanks.Clients"
        });

        private static List<string> WhitelistedTypes = new List<string>(new[] { 
        "System.Nullable",
        "System.Object",
        "System.Void",
        "System.IDisposable",
        "System.String",
        "System.Math",
        "System.Enum",
        "System.ValueType",
        "System.Guid",
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
        "System.Diagnostics.Debugger",
        "System.IO.Stream",
        "System.IO.TextReader",
        "System.IO.TextWriter",
        "System.NullReferenceException",
        "System.ArgumentException",
        "System.ArgumentNullException",
        "System.InvalidOperationException",
        "System.FormatException",
        "System.Exception",
        "System.UnhandledExceptionEventArgs",
        "System.DivideByZeroException",
        "System.InvalidCastException",
        "System.IO.FileNotFoundException",
        "System.IO.Path",
        "System.EventArgs",
        "System.Random",
        "System.Convert",
        "System.Nullable",
        "System.StringComparer",
        "System.IComparable",
        "System.Diagnostics.Stopwatch",
        "System.GC",
        "System.GCSettings"
        });

        /// <summary>
        /// Blacklisted types in whitelisted namespaces
        /// </summary>
        private static List<string> BlacklistedTypes = new List<string>(new[] { 
            "Engine.Logging.ILogger", //Don't let it thrash the HDD
            "Microsoft.Xna.Framework.Content.ContentManager", //Or load content from the hard drive
            "MPTanks.Modding.Compiliation.Compiler"
        });

        public static bool IsAssemblySafe(Assembly assembly, out string error)
        {
            var definition = LoadAssembly(assembly);
            error = "";
            bool hasBadTypes = false;
            foreach (var module in definition.Modules)
            {
                int amount = 0;
                foreach (var type in module.Types)
                {
                    amount++;
                    foreach (var method in type.Methods)
                    {
                        var err = "";
                        if (!CheckMethod(method, out err))
                        {
                            hasBadTypes = true; //Has error
                            error += "\n\n\nMethod: " + method.FullName + " has bad types\n" + err;
                        }
                    }

                    foreach (var property in type.Properties)
                    {
                        var err = "";
                        if (property.GetMethod != null && !CheckMethod(property.GetMethod, out err))
                        {
                            hasBadTypes = true; //Has error
                            error += "\n\n\nProperty: " + property.GetMethod.FullName + "'s getter has bad types\n" + err;
                        }

                        if (property.SetMethod != null && !CheckMethod(property.SetMethod, out err))
                        {
                            hasBadTypes = true; //Has error
                            error += "\n\n\nProperty: " + property.SetMethod.FullName + "'s setter has bad types\n" + err;
                        }
                    }

                    foreach (var field in type.Fields)
                    {
                        var whitelisted = IsTypeWhitelisted(field.FieldType.FullName);

                        if (!whitelisted)
                        {
                            hasBadTypes = true;
                            error += "\n\n\nField: " + field.FullName + "'s type (" + field.FieldType.FullName + ") is disallowed.";
                        }
                    }
                }
            }

            
            return hasBadTypes;
        }

        private static bool CheckMethod(Mono.Cecil.MethodDefinition def, out string error)
        {
            error = "";
            if (!def.HasBody) return true;
            bool safe = true;

            if (!IsTypeWhitelisted(def.ReturnType.FullName))
            {
                safe = false;
                error = "\n\nReturn type: " + def.ReturnType.FullName + " is disallowed.";
            }

            foreach (var variable in def.Body.Variables)
            {
                if (!IsTypeWhitelisted(variable.VariableType.FullName))
                {
                    safe = false;
                    error = "\n\nVariable in body: " + variable.Name + " (Type: " +
                        variable.VariableType.FullName + ") is disallowed.";
                }
            }
            foreach (var param in def.Parameters)
            {
                if (!IsTypeWhitelisted(param.ParameterType.FullName))
                {
                    safe = false;
                    error = "\n\nMethod parameter: " + param.Name + " (Type: " +
                        param.ParameterType.FullName + ") is disallowed.";
                }
            }

            foreach (var instr in def.Body.Instructions)
            {
                if (instr.OpCode.Equals(Mono.Cecil.Cil.OpCodes.Call) ||
                    instr.OpCode.Equals(Mono.Cecil.Cil.OpCodes.Callvirt) ||
                    instr.OpCode.Equals(Mono.Cecil.Cil.OpCodes.Calli))
                {
                    var reference = (Mono.Cecil.MethodReference)(instr.Operand);
                    //Call
                    if (!IsTypeWhitelisted(reference.DeclaringType.FullName))
                    {
                        safe = false;
                        error = "\n\nMethod call: " + reference.FullName.Split(' ')[1] + " is disallowed (Contained in " +
                            def.FullName + ")";
                    }
                }

            }

            if (!safe) 
                return false;
            return safe;
        }

        private static bool IsTypeWhitelisted(string typeName)
        {
            foreach (var ns in WhitelistedNamespaces)
                if (typeName.StartsWith(ns))
                {
                    foreach (var t in BlacklistedTypes)
                        if (typeName == t)
                            return false; //blacklisted subtype

                    return true; //in whitelisted namespace and not blacklisted
                }

            foreach (var t in WhitelistedTypes)
                if (typeName.StartsWith(t))
                    return true;

            return false;

        }

        #region Loading helper
        private static AssemblyDefinition LoadAssembly(Assembly asm)
        {
            var mStream = new MemoryStream(DllToByteArray(asm));
            var def = AssemblyDefinition.ReadAssembly(mStream);
            mStream.Dispose();
            return def;
        }

        private static byte[] DllToByteArray(Assembly asm)
        {
            return File.ReadAllBytes(asm.Location);
        }

        #endregion
    }
}
