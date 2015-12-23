using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

namespace MPTanks.Engine.Settings
{
    public abstract class SettingsBase
    {
        private string _file;
        public SettingsBase(string file)
        {
            //Handle raw file names
            if (!file.Contains(@"\") && !file.Contains("/"))
                file = Path.Combine(ConfigDir, file);

            _file = file;

            if (File.Exists(file))
            {
                _loading = true;
                SetDefaults();
                LoadFromFile(file);
                _loading = false;
            }
            else
            {
                _loading = true;
                SetDefaults();
                _loading = false;
                Save(file);
            }
        }

        public SettingsBase()
        {
            SetDefaults();
        }

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        { };

        static SettingsBase()
        {
            try
            {
                if (File.Exists("configpath.txt"))
#if DEBUG
                    ConfigDir = Environment.ExpandEnvironmentVariables(File.ReadAllLines("configpath.txt")[0]);
#else
                    ConfigDir = Environment.ExpandEnvironmentVariables(File.ReadAllLines("configpath.txt")[1]);
#endif
                Directory.CreateDirectory(ConfigDir);
            }
            catch { } //Ignore configuration issues
        }

        //We only look one place for the setting for this: configpath.txt in the current directory
        //The first line contains the debug path and the second contains the release path
        //If the file doesn't exist, go to default
        public static string ConfigDir
        { get; private set; }
        = Directory.GetCurrentDirectory();

        public void LoadFromFile(string file)
        {
            if (File.Exists(file))
                Load(File.ReadAllText(file));
        }
        private bool _loading;
        public void Load(string settingsData)
        {
            try { JsonConvert.PopulateObject(settingsData, this, _settings); }
            catch { }
        }

        protected abstract void SetDefaults();

        public void Save(string fileToSaveTo)
        {
            File.WriteAllText(fileToSaveTo, Save());
        }

        public string Save()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, _settings);
        }

        #region Change helper
        public virtual void OnSettingChanged(Setting setting)
        {
            if (_file == null || _loading) return;
            Save(_file);
        }
        #endregion

        public IEnumerable<Setting> GetAllSettings()
        {
            foreach (var field in GetType().GetFields())
                if (field.FieldType.IsSubclassOf(typeof(Setting)) || field.FieldType == typeof(Setting))
                    yield return (Setting)field.GetValue(this);

            foreach (var property in GetType().GetProperties())
                if (property.PropertyType.IsSubclassOf(typeof(Setting)) || property.PropertyType == typeof(Setting))
                    yield return (Setting)property.GetValue(this);
        }

        public class Setting
        {
            [JsonIgnore]
            public bool IsHidden { get; protected set; }
            private string _name;
            public string Name
            {
                get { return _name; }
                protected set { if (_name == null) _name = value; }
            }
            private string _description;
            public string Description
            {
                get { return _description; }
                protected set { if (_description == null) _description = value; }
            }
            private object _val;
            [JsonIgnore]
            public virtual dynamic ObjectValue
            {
                get
                {
                    return _val;
                }
                set
                {
                    _val = value;
                    SettingsInstance.OnSettingChanged(this);
                }
            }

            private object _default;

            [JsonIgnore]
            public virtual dynamic DefaultObjectValue
            {
                get
                {
                    return _default;
                }
                set
                {
                    _default = value;
                }
            }

            [JsonIgnore]
            public virtual IEnumerable<dynamic> AllowedObjectValues
            {
                get { return null; } //No whitelist
            }

            [JsonIgnore]
            public SettingsBase SettingsInstance { get; protected set; }

            protected Setting()
            {
            }

            public static Setting<T> Create<T>(SettingsBase settings, string name) => Setting<T>.CreateInternal(settings, name);
            /// <summary>
            /// Creates a setting as hidden from settings menus
            /// </summary>
            /// <returns></returns>
            public static Setting<TArg> Hidden<TArg>(SettingsBase provider, string name)
            {
                var st = Create<TArg>(provider, name);
                st.IsHidden = true;
                st._parser = (a) => st.DefaultValue;
                st._stringifier = (a) => "";
                return st;
            }
            /// <summary>
            /// Creates a setting as a number from settings menus
            /// </summary>
            /// <returns></returns>
            public static Setting<float> Number(SettingsBase provider, string name)
            {
                var st = Create<float>(provider, name);
                st.IsHidden = true;
                st._parser = (a) => float.Parse(a);
                st._stringifier = (a) => a.ToString();
                return st;
            }
            /// <summary>
            /// Creates a setting as a number from settings menus
            /// </summary>
            /// <returns></returns>
            public Setting<double> NumberDouble(SettingsBase provider, string name)
            {
                var st = Create<double>(provider, name);
                st.IsHidden = true;
                st._parser = (a) => double.Parse(a);
                st._stringifier = (a) => a.ToString();
                return st;
            }
            /// <summary>
            /// Creates a setting as a number (int) from settings menus
            /// </summary>
            /// <returns></returns>
            public static Setting<int> Int(SettingsBase provider, string name)
            {
                var st = Create<int>(provider, name);
                st.IsHidden = true;
                st._parser = (a) => int.Parse(a);
                st._stringifier = (a) => a.ToString();
                return st;
            }
            /// <summary>
            /// Creates a setting as a timespan from settings menus
            /// </summary>
            /// <returns></returns>
            public static Setting<TimeSpan> Time(SettingsBase provider, string name)
            {
                var st = Create<TimeSpan>(provider, name);
                st.IsHidden = true;
                st._parser = (a) => TimeSpan.Parse(a);
                st._stringifier = (a) => a.ToString();
                return st;
            }
            /// <summary>
            /// Creates a setting as a bool from settings menus
            /// </summary>
            /// <returns></returns>
            public static Setting<bool> Bool(SettingsBase provider, string name)
            {
                var st = Create<bool>(provider, name);
                st.IsHidden = true;
                st._parser = (a) => bool.Parse(a);
                st._stringifier = (a) => a.ToString();
                return st;
            }
            /// <summary>
            /// Marks a setting as a filesystem path from settings menus
            /// </summary>
            /// <returns></returns>
            public static Setting<string> Path(SettingsBase provider, string name)
            {
                var st = Create<string>(provider, name);
                st.IsHidden = true;
                st._parser = (a) => a;
                st._stringifier = (a) => a;
                st.SetValueVerifier(a => Directory.Exists(a) || File.Exists(a));
                return st;
            }
            /// <summary>
            /// Marks a setting as a string from settings menus
            /// </summary>
            /// <returns></returns>
            public static Setting<string> String(SettingsBase provider, string name)
            {
                var st = Create<string>(provider, name);
                st.IsHidden = true;
                st._parser = (a) => a; //passthrough
                st._stringifier = (a) => a;
                return st;
            }
            /// <summary>
            /// Marks a setting as a generic object from settings menus
            /// </summary>
            /// <returns></returns>
            public static Setting<TArg> Object<TArg>(SettingsBase provider, string name, Func<string, TArg> parser, Func<TArg, string> stringifier)
            {
                var st = Create<TArg>(provider, name);
                st.IsHidden = true;
                st._parser = parser;
                st._stringifier = stringifier;
                return st;
            }

            public override string ToString()
            {
                return $"(Setting {Name}) {ObjectValue.ToString()}";
            }
        }

        public class Setting<TArg> : Setting
        {
            public override object ObjectValue
            {
                get
                {
                    return Value;
                }
                set
                {
                    Value = (TArg)value;
                    SettingsInstance.OnSettingChanged(this);
                }
            }
            private TArg _value;
            public TArg Value
            {
                get { return _value; }
                set
                {
                    if (_verifier != null && !_verifier(value))
                    {
                        //If the current value is allowed, leave it
                        if (_verifier(Value)) return;
                        //Otherwise, remove it
                        _value = DefaultValue;
                        return; //Disallowed
                    }
                    _value = value;
                    SettingsInstance.OnSettingChanged(this);
                }
            }

            [JsonIgnore]
            public override dynamic DefaultObjectValue
            {
                get { return DefaultValue; }
                set { DefaultValue = (TArg)value; }
            }
            [JsonIgnore]
            public TArg DefaultValue { get; set; }

            [JsonIgnore]
            public override IEnumerable<dynamic> AllowedObjectValues
            {
                get
                {
                    if (_valueProvider == null) return null;
                    return _valueProvider().Select(a => (dynamic)a);
                }
            }

            private Func<TArg, bool> _verifier;
            private Func<IEnumerable<TArg>> _valueProvider;

            public bool CheckValue(string value)
            {
                if (_verifier == null) return true;
                return _verifier(_parser(value));
            }

            private Setting() { }

            internal Func<TArg, string> _stringifier;
            internal Func<string, TArg> _parser;
            public string AsString()
            {
                if (_stringifier == null) throw new Exception("Must set a type (Hidden(), String(), Number())");
                return _stringifier(Value);
            }
            internal static Setting<TArg> CreateInternal(SettingsBase settings, string name) =>
                new Setting<TArg>() { SettingsInstance = settings, Name = name };

            public Setting<TArg> SetDescription(string desc)
            {
                Description = desc;
                return this;
            }

            public Setting<TArg> SetName(string name)
            {
                Name = name;
                return this;
            }

            public Setting<TArg> SetDefault(TArg value)
            {
                DefaultValue = value;
                if (Equals(Value, default(TArg))) Value = value;
                return this;
            }

            public Setting<TArg> SetAllowedValues(Func<TArg> provider)
            {
                SetAllowedValues(() => new[] { provider() });
                return this;
            }
            public Setting<TArg> SetAllowedValues(Func<IEnumerable<TArg>> provider)
            {
                _valueProvider = provider;
                if (_verifier != null)
                    _verifier = (a) => provider().Contains(a);
                return this;
            }

            public Setting<TArg> SetAllowedValues(IEnumerable<TArg> whitelistedValues)
            {
                _valueProvider = () => whitelistedValues;
                if (_verifier != null)
                    _verifier = (a) => whitelistedValues.Contains(a);
                return this;
            }
            public Setting<TArg> SetAllowedValues(params TArg[] whitelistedValues)
            {
                _valueProvider = () => whitelistedValues;
                if (_verifier != null)
                    _verifier = (a) => whitelistedValues.Contains(a);
                return this;
            }
            /// <summary>
            /// Allows Just-in-time checking for whether an entered value is allowed
            /// </summary>
            /// <param name="verifier"></param>
            /// <returns></returns>
            public Setting<TArg> SetValueVerifier(Func<TArg, bool> verifier)
            {
                _verifier = verifier;
                return this;
            }

            public static implicit operator TArg(Setting<TArg> my)
            {
                return my.Value;
            }
        }
    }
}
