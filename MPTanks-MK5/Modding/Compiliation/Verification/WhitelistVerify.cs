using Mono.Cecil;
using System.Collections.Generic;
using System.IO;

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
            "Microsoft.VisualBasic",
            "MPTanks.Mods",
            "MPTanks.CoreAssets",
            "Starbound.Input",
            "MPTanks.Clients",
            "System.Dynamic",
            "FarseerPhysics"
        });

        private static List<string> WhitelistedTypes = new List<string>(new[] {
            //Primitive types
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
            //Debug helpers
            "System.Runtime.CompilerServices.RuntimeHelpers",
            "System.Diagnostics.Debugger",
            //Streams
            "System.IO.Stream",
            "System.IO.TextReader",
            "System.IO.TextWriter",
            "System.IO.MemoryStream",
            //Exceptions
            "System.NullReferenceException",
            "System.ArgumentException",
            "System.ArgumentNullException",
            "System.InvalidOperationException",
            "System.FormatException",
            "System.Exception",
            "System.UnhandledExceptionEventArgs",
            "System.DivideByZeroException",
            "System.InvalidCastException",
            //Path concentation tools
            "System.IO.Path",
            //Event args
            "System.EventArgs",
            "System.EventHandler",
            //Helpers
            "System.Random",
            "System.Convert",
            "System.Nullable",
            "System.StringComparer",
            "System.IComparable",
            "System.Diagnostics.Stopwatch",
            //GC
            "System.GC",
            //Lidgren
            "Lidgren.Network.NetBuffer",
            "Lidgren.Network.NetException",
            "Lidgren.Network.NetIncomingMessage",
            "Lidgren.Network.NetOutgoingMessage",
            "Lidgren.Network.NetConnection",
            //Tasks
            "System.Threading.Tasks.Task",
            //Functions
            "System.Action",
            "System.Func",
            "System.Delegate",
            "System.MulticastDelegate",
            //Custom
            "MPTanks.Modding.Module",
            "MPTanks.Modding.GameObjectType",
            "MPTanks.Modding.MapObjectType",
            "MPTanks.Modding.TankType",
            "MPTanks.Modding.ProjectileType",
            "MPTanks.Modding.GamemodeType",
        });

        /// <summary>
        /// Blacklisted types in whitelisted namespaces
        /// </summary>
        private static List<string> BlacklistedTypes = new List<string>(new[] {
            "MPTanks.Engine.Settings",
            "Microsoft.Xna.Framework.Content.ContentManager", //Or load content from the hard drive

        });

        public static bool IsAssemblySafe(string assembly, out string error)
        {
            var definition = LoadAssembly(assembly);
            error = "";
            bool hasBadTypes = false;
            foreach (var module in definition.Modules)
            {
                if (IsUnsafe(module))
                {
                    error = "\n\n\nModule " + module.Name + ": Unsafe code is disallowed.\n";
                    hasBadTypes = false;
                }

                int amount = 0;
                foreach (var type in module.Types)
                {
                    amount++;
                    //Ignore unnecessary
                    if (type == null || type.FullName == "<Module>") continue;
                    
                    if (!IsTypeWhitelisted(type.FullName))
                    {
                        error += "\n\n\nType " + type.FullName + " is not allowed. Are you in the MPTanks.Modding.Mods namespace?";
                        hasBadTypes = true;
                    }

                    var inheritanceErr = "";
                    if (!CheckTypeInheritanceTree(type, out inheritanceErr))
                    {
                        error += "\n\n\nType inheritance tree for " + type.FullName + ": \n" +
                            "========================================================\n\n" + inheritanceErr;
                        hasBadTypes = true;
                    }


                    foreach (var method in type.Methods)
                    {
                        var err = "";
                        if (!CheckMethod(method, out err))
                        {
                            hasBadTypes = true; //Has error
                            error += "\n\n\nMethod: " + method.FullName + " has bad types\n" +
                                "----------------------------------------------------------" + err;
                        }
                    }

                    foreach (var property in type.Properties)
                    {
                        var err = "";
                        if (property.GetMethod != null && !CheckMethod(property.GetMethod, out err))
                        {
                            hasBadTypes = true; //Has error
                            error += "\n\n\nProperty: " + property.GetMethod.FullName + "'s getter has bad types\n" +
                                "----------------------------------------------------------" + err;
                        }

                        if (property.SetMethod != null && !CheckMethod(property.SetMethod, out err))
                        {
                            hasBadTypes = true; //Has error
                            error += "\n\n\nProperty: " + property.SetMethod.FullName + "'s setter has bad types\n" +
                                "----------------------------------------------------------" + err;
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


            return !hasBadTypes;
        }

        private static bool CheckTypeInheritanceTree(Mono.Cecil.TypeDefinition def, out string error)
        {
            error = "";
            var baseType = def.BaseType;
            bool safe = true;
            while (baseType != null)
            {
                if (!IsTypeWhitelisted(baseType.FullName))
                {
                    error += "\n\n" + baseType.FullName + " is not allowed.";
                }
                baseType = baseType.Resolve().BaseType;
            }

            return safe;
        }

        private static bool CheckMethod(Mono.Cecil.MethodDefinition def, out string error)
        {
            error = "";
            bool safe = true;
            if (!def.HasBody) return true;
            if (def.IsUnmanagedExport || def.IsUnmanaged)
            {
                error = "\n\nUnmanaged exports are disallowed.";
                safe = false;
            }

            if (def.IsPInvokeImpl)
            {
                error = "\n\nPInvoke is disallowed.";
                safe = false;
            }

            if (def.IsNative)
            {
                error = "\n\nNative methods are disallowed.";
                safe = false;
            }


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

        private static bool IsUnsafe(ModuleDefinition module)
        {
            foreach (CustomAttribute cattr in module.CustomAttributes)
                if (cattr.Constructor.DeclaringType.Name == "UnverifiableCodeAttribute")
                    return true;
            return false;
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
        private static AssemblyDefinition LoadAssembly(string asm)
        {
            var mStream = new MemoryStream(DllToByteArray(asm));
            var def = AssemblyDefinition.ReadAssembly(mStream);
            mStream.Dispose();
            return def;
        }
        
        private static byte[] DllToByteArray(string asm)
        {
            return File.ReadAllBytes(asm);
        }

        #endregion
    }
}
