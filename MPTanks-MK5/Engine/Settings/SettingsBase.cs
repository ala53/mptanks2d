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
            if (File.Exists("configpath.txt"))
#if DEBUG
                ConfigDir = Environment.ExpandEnvironmentVariables(File.ReadAllLines("configpath.txt")[0]);
#else
                ConfigDir = Environment.ExpandEnvironmentVariables(File.ReadAllLines("configpath.txt")[1]);
#endif

            if (Path.IsPathRooted(ConfigDir))
                Directory.CreateDirectory(ConfigDir);
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
            [JsonIgnore]
            public virtual bool HasWhitelistOfAllowedValues { get; protected set; }
            [JsonIgnore]
            public virtual IEnumerable<dynamic> ObjectAllowedValues { get; protected set; }
            [JsonIgnore]
            public virtual SettingDisplayType DisplayType { get; protected set; }
            [JsonIgnore]
            public SettingsBase SettingsInstance { get; protected set; }

            protected Setting()
            {
            }

            public static Setting<T> Create<T>(SettingsBase settings, string name, string description, T defaultValue, params T[] allowedValues)
           => Setting<T>.Create(settings, name, description, defaultValue, allowedValues, DetectType(typeof(T)));

            public static Setting<T> Create<T>(SettingsBase settings, string name, string description, T defaultValue, SettingDisplayType displayType, params T[] allowedValues)
            => Setting<T>.Create(settings, name, description, defaultValue, allowedValues, displayType);

            public static Setting<T> Create<T>(SettingsBase settings, string name, string description, T defaultValue, IEnumerable<T> allowedValues)
            => Setting<T>.Create(settings, name, description, defaultValue, allowedValues, DetectType(typeof(T)));

            public static Setting<T> Create<T>(SettingsBase settings, string name, string description, T defaultValue, IEnumerable<T> allowedValues, SettingDisplayType displayType)
            => Setting<T>.Create(settings, name, description, defaultValue, allowedValues, displayType);

            public static Setting<T> Create<T>(SettingsBase settings, string name, string description, T defaultValue, Func<Setting<T>, IEnumerable<T>> valueProvider)
            => Setting<T>.Create(settings, name, description, defaultValue, valueProvider, DetectType(typeof(T)));

            public static Setting<T> Create<T>(SettingsBase settings, string name, string description, T defaultValue, Func<Setting<T>, IEnumerable<T>> valueProvider, SettingDisplayType displayType)
            => Setting<T>.Create(settings, name, description, defaultValue, valueProvider, displayType);

            public static Setting<T> Create<T>(SettingsBase settings, string name, string description, T defaultValue)
            => Setting<T>.Create(settings, name, description, defaultValue, DetectType(typeof(T)));

            public static Setting<T> Create<T>(SettingsBase settings, string name, string description, T defaultValue, SettingDisplayType displayType)
            => Setting<T>.Create(settings, name, description, defaultValue, displayType);

            private static SettingDisplayType DetectType(Type t)
            {
                if (t == typeof(int) || t == typeof(uint) || t == typeof(short) || t == typeof(ushort) ||
                    t == typeof(byte) || t == typeof(sbyte) || t == typeof(long) || t == typeof(ulong))
                    return SettingDisplayType.Integer;

                if (t == typeof(float) || t == typeof(double))
                    return SettingDisplayType.FloatingPointNumber;

                if (t == typeof(string))
                    return SettingDisplayType.String;

                if (t == typeof(string[]))
                    return SettingDisplayType.StringArray;

                if (t == typeof(bool))
                    return SettingDisplayType.Boolean;

                return SettingDisplayType.Object;
            }

            public enum SettingDisplayType
            {
                Integer,
                FloatingPointNumber,
                Percentage,
                TimeMS,
                TimeS,
                Path,
                Boolean,
                String,
                /// <summary>
                /// An array of strings is accepted as the value
                /// </summary>
                StringArray,
                /// <summary>
                /// ToString() is called.
                /// </summary>
                Object
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
                    _value = value;
                    SettingsInstance.OnSettingChanged(this);
                }
            }
            [JsonIgnore]
            public IEnumerable<TArg> AllowedValues
            {
                get
                {
                    if (_allowedValueProvider != null) return _allowedValueProvider(this);
                    return _allowedValues;
                }
            }

            public override IEnumerable<object> ObjectAllowedValues
            {
                get
                {
                    return AllowedValues.Select((a) => (object)a);
                }
                protected set
                {
                    _allowedValues = value
                        .Where(a => a.GetType() == typeof(TArg) ||
                        a.GetType().IsSubclassOf(typeof(TArg)))
                        .Select(a => (TArg)a);
                }
            }

            private IEnumerable<TArg> _allowedValues;
            private Func<Setting<TArg>, IEnumerable<TArg>> _allowedValueProvider;


            private Setting() { }

            public static new Setting<T> Create<T>(SettingsBase settings, string name, string description, T defaultValue, IEnumerable<T> allowedValues, SettingDisplayType displayType)
            {
                var setting = new Setting<T>();
                setting.SettingsInstance = settings;
                setting.Name = name;
                setting.Description = description;
                setting.Value = defaultValue;
                setting._allowedValues = allowedValues;
                setting.DisplayType = displayType;
                return setting;
            }

            public static new Setting<T> Create<T>(SettingsBase settings, string name, string description, T defaultValue, Func<Setting<T>, IEnumerable<T>> valueProvider, SettingDisplayType displayType)
            {
                var setting = new Setting<T>();
                setting.SettingsInstance = settings;
                setting.Name = name;
                setting.Description = description;
                setting.Value = defaultValue;
                setting._allowedValueProvider = valueProvider;
                setting.DisplayType = displayType;
                return setting;
            }

            public static new Setting<T> Create<T>(SettingsBase settings, string name, string description, T defaultValue, SettingDisplayType displayType)
            {
                var setting = new Setting<T>();
                setting.SettingsInstance = settings;
                setting.Name = name;
                setting.Description = description;
                setting.Value = defaultValue;
                setting.DisplayType = displayType;
                return setting;
            }

            public static implicit operator TArg(Setting<TArg> my)
            {
                return my.Value;
            }
        }
    }
}
