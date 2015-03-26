using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;

namespace MPTanks.Modding.Compiliation
{
    public class Compiler
    {
        private static Assembly CompileAssembly(string sourceCode, out string errors)
        {
            errors = "Not implemented";
            return null;
        }

        public static Module Compile(string sourceCode, out string errorList)
        {
            var errors = "";
            var asm = CompileAssembly(sourceCode, out errors);
            errorList = errors;

            if (errors != "")
                return null;

            var module = new Module();

            return module;
        }
    }
}
