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
        public static Assembly CompileAssembly(string[] sourceCode, out string errors, params string[] otherReferences)
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
            param.ReferencedAssemblies.Add("System.Linq.dll");
            param.ReferencedAssemblies.Add("System.Linq.Expressions.dll");
            param.ReferencedAssemblies.Add("System.Linq.Parallel.dll");
            param.ReferencedAssemblies.Add("System.Linq.Queryable.dll");
            param.ReferencedAssemblies.Add("MonoGame.Framework.dll");

            foreach (var reference in otherReferences)
                param.ReferencedAssemblies.Add(reference);

            CompilerResults results = compiler.CompileAssemblyFromSource(param, sourceCode);

            if (results.Errors.HasErrors)
            {
                foreach (CompilerError error in results.Errors)
                    errors += String.Format("Error ({0}): {1}\n\n", error.ErrorNumber, error.ErrorText);

                return null;
            }
            else
            {
                return results.CompiledAssembly;
            }
        }
    }
}
