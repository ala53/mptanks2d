using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace MPTanks.Modding.Compiliation
{
    public class Compiler
    {
        private static Assembly CompileAssembly(string sourceCode, out string errors)
        {
            errors = "";

            var compiler = new CSharpCodeProvider();
            var param = new CompilerParameters();
            param.ReferencedAssemblies.Add("MPTanks.Engine.dll");
            param.ReferencedAssemblies.Add("MPTanks.Modding.dll");
            param.ReferencedAssemblies.Add("mscorlib.dll");
            param.ReferencedAssemblies.Add("FarseerPhysics.Portable.dll");
            param.ReferencedAssemblies.Add("Newtonsoft.Json.dll");
            param.ReferencedAssemblies.Add("System.Xml.dll");
            param.ReferencedAssemblies.Add("System.Xml.Linq.dll");
            param.ReferencedAssemblies.Add("Monogame.Framework.dll");

            param.GenerateInMemory = true;

            CompilerResults results = compiler.CompileAssemblyFromSource(param, sourceCode);

            if (results.Errors.HasErrors)
            {
                foreach (CompilerError error in results.Errors)
                {
                    errors += String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText);
                }

                return null;
            }
            else
            {
                return results.CompiledAssembly;
            }
        }

        public static Module Compile(string sourceCode, out string errorList)
        {
            var errors = "";
            var asm = CompileAssembly(sourceCode, out errors);
            errorList = errors;

            if (errors != "")
                return null;

            var output = "";
            if (!Verification.WhitelistVerify.IsAssemblySafe(asm, out output))
            {
                errorList = output;
                return null;
            }

            var module = new Module();
            module.Assembly = asm;

            return module;
        }
    }
}
