using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK5.Javascript
{
    /// <summary>
    /// A generic / non-implementation-specific implementation of a JS script engine.
    /// Right now, we support ClearScript and Jint based off of settings.
    /// </summary>
    public class ScriptEngine
    {
        #region Engine Type Helpers
        public static bool UsingClearScript
        {
            get
            {
                return MPTanks_MK4.Settings.Modifiable.Engine ==
                    MPTanks_MK4.Settings.Modifiable.ScriptEngineType.ClearScriptV8;
            }
        }
        public static bool UsingVroom
        {
            get
            {
                return MPTanks_MK4.Settings.Modifiable.Engine ==
                    MPTanks_MK4.Settings.Modifiable.ScriptEngineType.VroomJs;
            }
        }
        public static bool UsingJint
        {
            get
            {
                return MPTanks_MK4.Settings.Modifiable.Engine ==
                    MPTanks_MK4.Settings.Modifiable.ScriptEngineType.Jint;
            }
        }
        #endregion

        #region Static init and parsing of helper js
        private static Jint.Parser.Ast.Program[] jintBasePrograms;
        private static V8Script[] clearScriptBasePrograms;
        private static V8Runtime clearScriptBaseRuntime;
        static ScriptEngine()
        {
            if (MPTanks_MK4.Settings.Modifiable.Engine ==
                MPTanks_MK4.Settings.Modifiable.ScriptEngineType.ClearScriptV8)
                clearScriptBaseRuntime = new V8Runtime();

            ParseBasePrograms();
        }

        private static void ParseBasePrograms()
        {
            #region Jint
            if (UsingJint)
            {
                //Here, we load the basic programs from assets/js
                //And then we can add them to each engine instance
                //without the parser overhead
                var programs = new List<Jint.Parser.Ast.Program>();

                var files = System.IO.Directory.GetFiles(MPTanks_MK4.Settings.Modifiable.CoreAssets.JS);

                var parser = new Jint.Parser.JavaScriptParser(true);

                foreach (var file in files)
                {
                    //TODO use something other than FileInfo for portability
                    var fi = new FileInfo(file);
                    //If it's a demand loaded file...
                    if (fi.Name.StartsWith("_demandload_"))
                        continue;

                    programs.Add(parser.Parse(System.IO.File.ReadAllText(file)));
                }

                jintBasePrograms = programs.ToArray();
            }
            #endregion
            #region ClearScript
            if (UsingClearScript)
            {
                var programs = new List<V8Script>();

                var files = System.IO.Directory.GetFiles(MPTanks_MK4.Settings.Modifiable.CoreAssets.JS);

                foreach (var file in files)
                {
                    //TODO use something other than FileInfo for portability
                    var fi = new FileInfo(file);
                    //If it's a demand loaded file...
                    if (fi.Name.StartsWith("_demandload_"))
                        continue;

                    programs.Add(
                        clearScriptBaseRuntime.Compile(System.IO.File.ReadAllText(file)));
                }

                clearScriptBasePrograms = programs.ToArray();
            }
            #endregion
        }
        #endregion

        private Jint.Engine jintEngine;
        private V8ScriptEngine clearScriptEngine;

        #region Init

        public ScriptEngine(string script)
        {
            if (UsingJint)
                jintEngine = new Jint.Engine();

            if (UsingClearScript)
                clearScriptEngine = clearScriptBaseRuntime.CreateScriptEngine();

            LoadBaseScripts();
        }

        private bool baseScriptsLoaded = false;
        private void LoadBaseScripts()
        {
            if (baseScriptsLoaded)
                return;
            baseScriptsLoaded = true;
            if (UsingJint)
            {
                foreach (var program in jintBasePrograms)
                    jintEngine.Execute(program);
            }

            if (UsingClearScript)
            {
                foreach (var program in clearScriptBasePrograms)
                    clearScriptEngine.Execute(program);
            }
        }
        #endregion

        #region Clearscript helpers
        private static object GetProperty(object target, string name)
        {
            var site = System.Runtime.CompilerServices.CallSite<Func<System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, name, target.GetType(), new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) }));
            return site.Target(site, target);
        }
        #endregion
        public bool GetBool(string name)
        {
            if (UsingJint)
                return jintEngine.GetValue(name).AsBoolean();

            if (UsingClearScript)
                return (bool)GetProperty(clearScriptEngine.Script, name);

            return false;
        }

        public float GetInt(string name)
        {
            if (UsingJint)
                return (float)jintEngine.GetValue(name).AsNumber();

            if (UsingClearScript)
                return (float)GetProperty(clearScriptEngine.Script, name);

            return 0;
        }

        public string GetString(string name)
        {
            if (UsingJint)
                return jintEngine.GetValue(name).AsString();

            if (UsingClearScript)
                return (string)GetProperty(clearScriptEngine.Script, name);

            return null;
        }

        public dynamic GetObject(string name)
        {
            if (UsingJint)
                return jintEngine.GetValue(name);

            if (UsingClearScript)
                return GetProperty(clearScriptEngine.Script, name);

            return false;
        }

        public void SetValue(string name, object value)
        {
            if (UsingJint)
                jintEngine.SetValue(name, value);

            if (UsingClearScript)
                clearScriptEngine.AddHostObject(name, value);
        }
        public void SetValue(string name, bool value)
        {
            if (UsingJint)
                jintEngine.SetValue(name, value);

            if (UsingClearScript)
                clearScriptEngine.AddHostObject(name, value);
        }
        public void SetValue(string name, double value)
        {
            if (UsingJint)
                jintEngine.SetValue(name, value);

            if (UsingClearScript)
                clearScriptEngine.AddHostObject(name, value);
        }
        public void SetValue(string name, int value)
        {
            if (UsingJint)
                jintEngine.SetValue(name, value);

            if (UsingClearScript)
                clearScriptEngine.AddHostObject(name, value);
        }
        public void RegisterHandler(string name, Delegate value)
        {
            if (UsingJint)
                jintEngine.SetValue(name, value);

            if (UsingClearScript)
                clearScriptEngine.AddHostObject(name, value);
        }

        public void CallFunctionNoReturn(string name, params object[] args)
        {
            if (UsingJint)
            {
                Jint.Native.JsValue[] vals = new Jint.Native.JsValue[args.Length];

                for (int i = 0; i < args.Length; i++)
                    vals[i] = Jint.Native.JsValue.FromObject(jintEngine, args[i]);

                jintEngine.GetValue(name).Invoke(vals);
            }

            if (UsingClearScript)
            {
                clearScriptEngine.Invoke(name, args);
            }
        }
        public bool CallFunctionBool(string name, params object[] args)
        {
            if (UsingJint)
            {
                Jint.Native.JsValue[] vals = new Jint.Native.JsValue[args.Length];

                for (int i = 0; i < args.Length; i++)
                    vals[i] = Jint.Native.JsValue.FromObject(jintEngine, args[i]);

                return jintEngine.GetValue(name).Invoke(vals).AsBoolean();
            }

            if (UsingClearScript)
            {
                return (bool)clearScriptEngine.Invoke(name, args);
            }

            return false;
        }
        public int CallFunctionInt(string name, params object[] args)
        {
            if (UsingJint)
            {
                Jint.Native.JsValue[] vals = new Jint.Native.JsValue[args.Length];

                for (int i = 0; i < args.Length; i++)
                    vals[i] = Jint.Native.JsValue.FromObject(jintEngine, args[i]);

                return (int)jintEngine.GetValue(name).Invoke(vals).AsNumber();
            }

            if (UsingClearScript)
                return (int)clearScriptEngine.Invoke(name, args);

            return 0;
        }
        public float CallFunctionFloat(string name, params object[] args)
        {
            if (UsingJint)
            {
                Jint.Native.JsValue[] vals = new Jint.Native.JsValue[args.Length];

                for (int i = 0; i < args.Length; i++)
                    vals[i] = Jint.Native.JsValue.FromObject(jintEngine, args[i]);

                return (float)jintEngine.GetValue(name).Invoke(vals).AsNumber();
            }

            if (UsingClearScript)
                return (float)clearScriptEngine.Invoke(name, args);

            return 0;
        }
        public string CallFunctionString(string name, params object[] args)
        {
            if (UsingJint)
            {
                Jint.Native.JsValue[] vals = new Jint.Native.JsValue[args.Length];

                for (int i = 0; i < args.Length; i++)
                    vals[i] = Jint.Native.JsValue.FromObject(jintEngine, args[i]);

                return jintEngine.GetValue(name).Invoke(vals).AsString();
            }

            if (UsingClearScript)
                return (string)clearScriptEngine.Invoke(name, args);

            return null;
        }
        public dynamic CallFunctionObject(string name, params object[] args)
        {
            if (UsingJint)
            {
                Jint.Native.JsValue[] vals = new Jint.Native.JsValue[args.Length];

                for (int i = 0; i < args.Length; i++)
                    vals[i] = Jint.Native.JsValue.FromObject(jintEngine, args[i]);

                return jintEngine.GetValue(name).Invoke(vals).AsObject();
            }

            if (UsingClearScript)
                return clearScriptEngine.Invoke(name, args);

            return null;
        }

        /// <summary>
        /// Loads a script for this ScriptEngine instance.
        /// </summary>
        /// <param name="script"></param>
        public void LoadScript(string script)
        {
            if (UsingJint)
                jintEngine.Execute(script);

            if (UsingClearScript)
                clearScriptEngine.Execute(script);
        }
        /// <summary>
        /// Loads a program from a previously compiled blob object retrieved from LoadScriptCachable.
        /// </summary>
        /// <param name="scriptCached"></param>
        public void LoadScript(object scriptCached)
        {
            if (UsingJint)
                jintEngine.Execute((Jint.Parser.Ast.Program)scriptCached);

            if (UsingClearScript)
                clearScriptEngine.Execute((V8Script)scriptCached);
        }

        /// <summary>
        /// Parses the script and returns an object that represents the script data.
        /// This can later be used in a loadscript call to avoid overhead.
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public object LoadScriptCachable(string script)
        {
            var parsed = PreParseScript(script);
            LoadScript(parsed);
            return parsed;
        }

        public static object PreParseScript(string script)
        {
            if (UsingJint)
            {
                var parser = new Jint.Parser.JavaScriptParser(true);
                var parsed = parser.Parse(script);
                return parsed;
            }

            if (UsingClearScript)
            {
                var compiled = clearScriptBaseRuntime.Compile(script);
                clearScriptBaseRuntime.CollectGarbage(true); //safety: gc after compiling
                return compiled;
            }

            return null;
        }

    }

}
