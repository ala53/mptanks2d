using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
                LoadFromFile(file);
            else
                Task.Run(async () =>
                {
                    //Wait for the object to be initialized
                    await Task.Delay(100);
                    Save(file);
                });
        }

        public SettingsBase()
        {

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
            var dcr = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            dcr.DefaultMembersSearchFlags = dcr.DefaultMembersSearchFlags | System.Reflection.BindingFlags.NonPublic;
        }

        //We only look one place for the setting for this: configpath.txt in the current directory
        //The first line contains the debug path and the second contains the release path
        //If the file doesn't exist, go to default
        public static readonly string ConfigDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "MP Tanks 2D");

        public void LoadFromFile(string file)
        {
            if (File.Exists(file))
                Load(File.ReadAllText(file));
        }
        public void Load(string settingsData)
        {
            JsonConvert.PopulateObject(settingsData, this, _settings);
        }

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
            if (_file != null)
                Save(_file);
        }
        #endregion

        public IEnumerable<Setting> GetAllSettings()
        {
            foreach (var field in this.GetType().GetFields())
                if (field.FieldType.IsSubclassOf(typeof(Setting)) || field.FieldType == typeof(Setting))
                    yield return (Setting)field.GetValue(this);

            foreach (var property in this.GetType().GetProperties())
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
            public virtual object ObjectValue
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
            public virtual IEnumerable<object> ObjectAllowedValues { get; protected set; }
            [JsonIgnore]
            public virtual SettingDisplayType DisplayType { get; protected set; }
            [JsonIgnore]
            public SettingsBase SettingsInstance { get; private set; }

            protected Setting(SettingsBase instance)
            {
                SettingsInstance = instance;
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
        }

        public class Setting<T> : Setting
        {
            public override object ObjectValue
            {
                get
                {
                    return Value;
                }
                set
                {
                    Value = (T)value;
                    SettingsInstance.OnSettingChanged(this);
                }
            }
            public T Value { get; set; }
            [JsonIgnore]
            public IEnumerable<T> AllowedValues
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
                    return (IEnumerable<object>)AllowedValues;
                }
                protected set
                {
                    _allowedValues = (IEnumerable<T>)value;
                }
            }

            private IEnumerable<T> _allowedValues;
            private Func<Setting<T>, IEnumerable<T>> _allowedValueProvider;

            public Setting(SettingsBase settings, string name, string description, T defaultValue, params T[] allowedValues)
                : this(settings, name, description, defaultValue, allowedValues, DetectType(typeof(T)))
            {
            }

            public Setting(SettingsBase settings, string name, string description, T defaultValue, SettingDisplayType displayType, params T[] allowedValues)
                : this(settings, name, description, defaultValue, allowedValues, displayType)
            {
            }

            public Setting(SettingsBase settings, string name, string description, T defaultValue, IEnumerable<T> allowedValues)
                : this(settings, name, description, defaultValue, allowedValues, DetectType(typeof(T)))
            {

            }

            public Setting(SettingsBase settings, string name, string description, T defaultValue, IEnumerable<T> allowedValues, SettingDisplayType displayType)
                : base(settings)
            {
                Name = name;
                Description = description;
                Value = defaultValue;
                _allowedValues = allowedValues;
                DisplayType = displayType;
            }

            public Setting(SettingsBase settings, string name, string description, T defaultValue, Func<Setting<T>, IEnumerable<T>> valueProvider)
                : this(settings, name, description, defaultValue, valueProvider, DetectType(typeof(T)))
            {

            }

            public Setting(SettingsBase settings, string name, string description, T defaultValue, Func<Setting<T>, IEnumerable<T>> valueProvider, SettingDisplayType displayType)
                : base(settings)
            {
                Name = name;
                Description = description;
                Value = defaultValue;
                _allowedValueProvider = valueProvider;
                DisplayType = displayType;
            }

            public Setting(SettingsBase settings, string name, string description, T defaultValue)
                : this(settings, name, description, defaultValue, DetectType(typeof(T)))
            {

            }

            public Setting(SettingsBase settings, string name, string description, T defaultValue, SettingDisplayType displayType)
                : base(settings)
            {
                Name = name;
                Description = description;
                Value = defaultValue;
                DisplayType = displayType;
            }

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

            public static implicit operator T(Setting<T> my)
            {
                return my.Value;
            }
        }
    }
}
