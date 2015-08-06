using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.GameSandbox.Input
{
    public class KeyBindingCollection
    {
        private Dictionary<string, object> _keyBindings = new Dictionary<string, object>();
        public IReadOnlyDictionary<string, object> KeyBindings => _keyBindings;

        public void AddBinding(string task, object newKey)
        {
            _keyBindings.Add(task, newKey);
        }

        public void SetBinding(string task, object newKey)
        {
            if (_keyBindings.ContainsKey(task)) throw new Exception("Binding does not exist");
            _keyBindings.Remove(task);
            _keyBindings.Add(task, newKey);
        }

        public KeyBindingCollection(params KeyBinding[] bindings)
        {
            foreach (var binding in bindings)
                AddBinding(binding.Task, binding.DefaultValue);
        }

        public struct KeyBinding
        {
            public string Task;
            public object DefaultValue;
            public KeyBinding(string task, object defaultValue)
            {
                Task = task;
                DefaultValue = defaultValue;
            }
        }

        public KeyBindingCollection(string saved)
        {
            Load(saved);
        }

        public object this[string name] => _keyBindings[name];

        public void Load(string saved)
        {
            _keyBindings = JsonConvert.DeserializeObject<Dictionary<string, object>>(saved,
                 new JsonSerializerSettings()
                 {
                     PreserveReferencesHandling = PreserveReferencesHandling.All,
                     ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                     TypeNameHandling = TypeNameHandling.All,
                     Formatting = Formatting.Indented
                 });
        }

        public string Save()
        {
            return JsonConvert.SerializeObject(_keyBindings,
                 new JsonSerializerSettings()
                 {
                     PreserveReferencesHandling = PreserveReferencesHandling.All,
                     ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                     TypeNameHandling = TypeNameHandling.All,
                     Formatting = Formatting.Indented
                 });
        }
    }
}
