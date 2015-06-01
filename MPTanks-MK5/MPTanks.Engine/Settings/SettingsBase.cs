using System;
using System.Collections.Generic;
using System.IO;

namespace MPTanks.Engine.Settings
{
    public abstract class SettingsBase
    {
        public static readonly string ConfigDir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "MP Tanks 2D");

        public void LoadFromFile(string file)
        {
            if (File.Exists(file))
                Load(File.ReadAllText(file));
        }
        public void Load(string settingsData)
        {
            Newtonsoft.Json.JsonConvert.PopulateObject(settingsData, this);
        }

        public void Save(string fileToSaveTo)
        {
            System.IO.File.WriteAllText(fileToSaveTo, Save());
        }

        public string Save()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        #region Change helper
        public virtual void OnSettingChanged(Setting setting)
        {

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
            public virtual string Name { get; protected set; }
            public virtual string Description { get; protected set; }
            private Object _val;
            public virtual Object ObjectValue
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
            public virtual bool HasWhitelistOfAllowedValues { get; protected set; }
            public virtual IEnumerable<Object> ObjectAllowedValues { get; protected set; }
            public virtual SettingDisplayType DisplayType { get; protected set; }
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
                }
            }
            public T Value { get; set; }

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
                    return (IEnumerable<Object>)AllowedValues;
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
