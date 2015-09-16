using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Logging
{
    public struct ModuleLogger : ILogger
    {
        private ILogger _writes;
        public ILogger WritesTo { get { return _writes; } set { _writes = value; } }
        private string _moduleName;
        //Optimization
        private static Dictionary<string, string> _nameCache = new Dictionary<string, string>();
        public ModuleLogger(ILogger writesTo, string moduleName)
        {
            _writes = writesTo;
            //Because moduleloggers are initialized often, we cache the names
            //to avoid any allocations from string concentation
            if (!_nameCache.ContainsKey(moduleName))
                _nameCache.Add(moduleName, "[" + moduleName + "] ");

            _moduleName = _nameCache[moduleName];
        }

        public void Trace(string message)
        {
            _writes.Trace(_moduleName + message);
        }

        public void Trace(object data)
        {
            _writes.Trace(_moduleName + JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public void Trace(Exception ex)
        {
            _writes.Trace(_moduleName, ex);
        }

        public void Trace(string message, Exception ex)
        {
            _writes.Trace(_moduleName + message, ex);
        }

        public void Debug(string message)
        {
            _writes.Debug(_moduleName + message);
        }

        public void Info(string message)
        {
            _writes.Info(_moduleName + message);
        }

        public void Info(object data)
        {
            _writes.Info(_moduleName + JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public void Warning(string message)
        {
            _writes.Warning(_moduleName + message);
        }

        public void Warning(object data)
        {
            _writes.Warning(_moduleName + JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        public void Error(string message)
        {
            _writes.Error(_moduleName + message);
        }

        public void Error(Exception ex)
        {
            _writes.Error(_moduleName, ex);
        }

        public void Error(string message, Exception ex)
        {
            _writes.Error(_moduleName + message, ex);
        }

        public void Fatal(string message)
        {
            _writes.Fatal(_moduleName + message);
        }

        public void Fatal(Exception ex)
        {
            _writes.Fatal(_moduleName, ex);
        }

        public void Fatal(string message, Exception ex)
        {
            _writes.Fatal(_moduleName + message, ex);
        }
    }
}
