using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace Ruminate.Utils {

    public static class RuntimeManager {

        #region internal statics and constants

        static readonly Dictionary<string, string> ConditionSnippet 
            = new Dictionary<string, string>();
        static readonly Dictionary<string, string> MethodSnippet 
            = new Dictionary<string, string>();

        static readonly List<string> References = new List<string>("System.dll,System.dll,System.Data.dll,System.Xml.dll,mscorlib.dll,System.Windows.Forms.dll".Split(new char[] { ',' }));

        static RuntimeManager() {
            Assembly = null;
        }

        const string CodeStart = "using System;\r\nusing System.Collections.Generic;\r\n//using System.Linq;\r\nusing System.Text;\r\nusing System.Data;\r\nusing System.Reflection;\r\nusing System.CodeDom.Compiler;\r\nusing Microsoft.CSharp;\r\nnamespace Ruminate.Utils\r\n{\r\n  public class Dynamic : DynamicBase\r\n  {\r\n";
        const string DynamicConditionPrefix = "__dm_";
        const string ConditionTemplate = "    bool {0}{1}(params object[] p) {{ return {2}; }}\r\n";
        const string MethodTemplate = "    object {0}(params object[] p) {{\r\n{1}\r\n    }}\r\n";
        const string CodeEnd = "  }\r\n}";        
        
        #endregion

        public static Assembly Assembly { get; private set; }

        #region manage snippets
        public static void Clear() {
            MethodSnippet.Clear();
            ConditionSnippet.Clear();
            Assembly = null;
        }
        public static void Clear(string name) {
            if (ConditionSnippet.ContainsKey(name)) {
                Assembly = null;
                ConditionSnippet.Remove(name);
            } else if (MethodSnippet.ContainsKey(name)) {
                Assembly = null;
                MethodSnippet.Remove(name);
            }
        }

        public static void AddCondition(string conditionName, string booleanExpression) {
            if (ConditionSnippet.ContainsKey(conditionName))
                throw new InvalidOperationException(string.Format("There is already a condition called '{0}'", conditionName));
            var src = new StringBuilder(CodeStart);
            src.AppendFormat(ConditionTemplate, DynamicConditionPrefix, conditionName, booleanExpression);
            src.Append(CodeEnd);
            Compile(src.ToString()); //if the condition is invalid an exception will occur here
            ConditionSnippet[conditionName] = booleanExpression;
            Assembly = null;
        }

        public static void AddMethod(string methodName, string methodSource) {
            if (MethodSnippet.ContainsKey(methodName))
                throw new InvalidOperationException(string.Format("There is already a method called '{0}'", methodName));
            if (methodName.StartsWith(DynamicConditionPrefix))
                throw new InvalidOperationException(string.Format("'{0}' is not a valid method name because the '{1}' prefix is reserved for internal use with conditions", methodName, DynamicConditionPrefix));
            var src = new StringBuilder(CodeStart);
            src.AppendFormat(MethodTemplate, methodName, methodSource);
            src.Append(CodeEnd);
            Trace.TraceError("SOURCE\r\n{0}", src);
            Compile(src.ToString()); //if the condition is invalid an exception will occur here
            MethodSnippet[methodName] = methodSource;
            Assembly = null;
        }
        #endregion

        #region use snippets
        public static object InvokeMethod(string methodName, params object[] p) {
            DynamicBase _dynamicMethod = null;
            if (Assembly == null) {
                Compile();
                _dynamicMethod = Assembly.CreateInstance("Dynamo.Ruminate.Utils") as DynamicBase;
            }
            return _dynamicMethod.InvokeMethod(methodName, p);
        }

        public static bool Evaluate(string conditionName, params object[] p) {
            DynamicBase _dynamicCondition = null;
            if (Assembly == null) {
                Compile();
                _dynamicCondition = Assembly.CreateInstance("Dynamo.Ruminate.Utils") as DynamicBase;
            }
            return _dynamicCondition.EvaluateCondition(conditionName, p);
        }

        public static double Transform(string functionName, params object[] p) {
            DynamicBase _dynamicCondition = null;
            if (Assembly == null) {
                Compile();
                _dynamicCondition = Assembly.CreateInstance("Dynamo.Ruminate.Utils") as DynamicBase;
            }
            return _dynamicCondition.Transform(functionName, p);
        }
        #endregion

        #region support routines
        public static string ProduceConditionName(Guid conditionId) {
            StringBuilder cn = new StringBuilder();
            foreach (char c in conditionId.ToString().ToCharArray()) if (char.IsLetterOrDigit(c)) cn.Append(c);
            string conditionName = cn.ToString();
            return string.Format("_dm_{0}", cn);
        }
        private static void Compile() {
            if (Assembly == null) {
                StringBuilder src = new StringBuilder(CodeStart);
                foreach (KeyValuePair<string, string> kvp in ConditionSnippet)
                    src.AppendFormat(ConditionTemplate, DynamicConditionPrefix, kvp.Key, kvp.Value);
                foreach (KeyValuePair<string, string> kvp in MethodSnippet)
                    src.AppendFormat(MethodTemplate, kvp.Key, kvp.Value);
                src.Append(CodeEnd);
                Trace.TraceError("SOURCE\r\n{0}", src);
                Assembly = Compile(src.ToString());
            }
        }
        private static Assembly Compile(string sourceCode) {
            var cp = new CompilerParameters();
            cp.ReferencedAssemblies.AddRange(References.ToArray());
            cp.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
            cp.CompilerOptions = "/target:library /optimize";
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;
            var cr = (new CSharpCodeProvider()).CompileAssemblyFromSource(cp, sourceCode);
            if (cr.Errors.Count > 0) throw new CompileException(cr.Output, cr.Errors);
            return cr.CompiledAssembly;
        }
        #endregion

        public static bool HasItem(string methodName) {
            return ConditionSnippet.ContainsKey(methodName) || MethodSnippet.ContainsKey(methodName);
        }

        public class CompileException : Exception
        {
            public System.Collections.Specialized.StringCollection Output;
            public System.CodeDom.Compiler.CompilerErrorCollection Errors;

            public CompileException(System.Collections.Specialized.StringCollection output, System.CodeDom.Compiler.CompilerErrorCollection errors)
                : base("Compile exception")
            {
                Output = output;
                Errors = errors;
            }
        };
    }

    public abstract class DynamicBase {
        public bool EvaluateCondition(string methodName, params object[] p) {
            methodName = string.Format("__dm_{0}", methodName);
            BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic;
            return (bool)GetType().InvokeMember(methodName, flags, null, this, p);
        }
        public object InvokeMethod(string methodName, params object[] p) {
            BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic;
            return GetType().InvokeMember(methodName, flags, null, this, p);
        }
        public double Transform(string functionName, params object[] p) {
            functionName = string.Format("__dm_{0}", functionName);
            BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic;
            return (double)GetType().InvokeMember(functionName, flags, null, this, p);
        }
    }
}
