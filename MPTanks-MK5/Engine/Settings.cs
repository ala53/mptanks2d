using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public class Settings
    {
        /// <summary>
        /// The physics engine runs at 1/10 scale for some idiotic reason.
        /// Basically, that means that a 3x5 tank is 0.3x0.5 blocks because
        /// Box2D a.k.a. Farseer works best with objects between 0.1 and 10 units
        /// in size.
        /// </summary>
        public Setting<float> PhysicsScale { get; private set; }

        /// <summary>
        /// A list of mod files (relative or absolute) that should be loaded without 
        /// Code security verification. AKA trusted mods.
        /// </summary>
        public Setting<string[]> TrustedMods { get; private set; }

        public static readonly string[] DefaultTrustedMods = new[] {
                    "MPTanks.Modding.Mods.Core.dll"
                };

        /// <summary>
        /// The scale the rendering runs at relative to the blocks.
        /// This way, we can pass integers around safely.
        /// </summary>
        public Setting<float> RenderScale { get; private set; }

        /// <summary>
        /// The amount of "blocks" to compensate for in rendering because of the 
        /// skin on physics objects
        /// </summary>
        public Setting<float> PhysicsCompensationForRendering { get; private set; }

        public Setting<float> TankDensity { get; private set; }

        /// <summary>
        /// Number of milliseconds after GameCore initialization to wait before starting
        /// the game because we want all of the setup to be done and for people to be connected.
        /// </summary>
        public Setting<float> TimeToWaitBeforeStartingGame { get; private set; }

        /// <summary>
        /// The number of milliseconds after game to continue updating
        /// </summary>
        public Setting<float> TimePostGameToContinueRunning { get; private set; }

        /// <summary>
        /// The maximum number of particles to allow in game
        /// </summary>
        public Setting<int> ParticleLimit { get; private set; }


        /// <summary>
        /// The minimum delta time in milliseconds for a game tick. If DT is less than this,
        /// we do one tick, then skip the next one, etc. until we have a deficit of game time,
        /// then do another and repeat.
        /// </summary>
        public Setting<float> MinDeltaTimeGameTick { get; private set; }

        /// <summary>
        /// The maximum delta time for a game tick. If the time is greater, multiple steps will be taken.
        /// </summary>
        public Setting<float> MaxDeltaTimeGameTick { get; private set; }

        /// <summary>
        /// The maximum delta time in milliseconds that a tick of the particle emitter can be.
        /// If DT is greater than this, multiple steps will be taken rather than a single one.
        /// </summary>
        public Setting<float> ParticleEmitterMaxDeltaTime { get; private set; }

        public Settings()
        {
            PhysicsScale = new Setting<float>(this, "Physics Scale",
                "The scale of the physics engine relative to game world space",
                0.1f, Setting.SettingDisplayType.Percentage);

            TrustedMods = new Setting<string[]>(this, "Trusted Mods",
                "The paths of mods to load without whitelist verification", DefaultTrustedMods);

            RenderScale = new Setting<float>(this, "Render Scale",
            "The scale of rendering relative to game space so integer conversions work", 100f);

            PhysicsCompensationForRendering = new Setting<float>(this, "Physics Skin Compensation",
                "The amount in blocks to compensate for Farseer Physics's skin on bodies.", 0.085f);

            TankDensity = new Setting<float>(this, "Tank density",
            "The density of a tank in the physics engine.", 15f);

            TimeToWaitBeforeStartingGame = new Setting<float>(this, "Pre game connection wait time",
            "The amount of time, in seconds, to wait before starting a game." +
            " This is here primarily to give users time to connect to the server and download map" +
            " and server data (mods, etc) before the game starts.", 5000f, Setting.SettingDisplayType.TimeMS);

            TimePostGameToContinueRunning = new Setting<float>(this, "Post game time",
            "The amount of time to keep the game running after the winner has been determined." +
            " Sort of a post round deathmatch phase and a phase that helps people read the outcome" +
            " of the game.", 5000, Setting.SettingDisplayType.TimeMS);

            ParticleLimit = new Setting<int>(this, "Particle limit",
            "The maximum number of particles to allow in a game. The default is 20,000 for good reason." +
            " If it's more, there is a quite a chance of lag (even here, it happens on occasion)." +
            " If you set it past 100,000, you deserve to die a slow and painful lag death and you probably will.", 20000);

            MinDeltaTimeGameTick = new Setting<float>(this, "Minimum Game Tick Time",
            "The smallest amount of time allowed for a single game tick." +
            " This probably should not be less than 1/3 of a millisecond or we run into numeric" +
            " precision issues in the physics engine.", 0.5f, Setting.SettingDisplayType.TimeMS);

            MaxDeltaTimeGameTick = new Setting<float>(this, "Maximum Game Tick Time",
            "The maximum amount of time allowed for a single game tick. If it's greater," +
            " multiple game ticks will be performed for the one tick. Unfortunately, while" +
            " this improves the stability of collisions, it can cause a 'spiral of death' if the" +
            " game tick loop is the bottleneck of the game.", 34f, Setting.SettingDisplayType.TimeMS);

            ParticleEmitterMaxDeltaTime = new Setting<float>(this, "Particle Emitter max tick time",
            "The maximum amount of time each tick of the particle engine can be. Changes to this value" +
            " can stop some particle rendering bugs due to faulty velocity calculations. Higher values lower" +
            " CPU time needed while lower values increase particle trail accuracy.",
            17f, Setting.SettingDisplayType.TimeMS);
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
            public Settings SettingsInstance { get; private set; }

            protected Setting(Settings instance)
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

            public Setting(Settings settings, string name, string description, T defaultValue, params T[] allowedValues)
                : this(settings, name, description, defaultValue, allowedValues, DetectType(typeof(T)))
            {
            }

            public Setting(Settings settings, string name, string description, T defaultValue, SettingDisplayType displayType, params T[] allowedValues)
                : this(settings, name, description, defaultValue, allowedValues, displayType)
            {
            }

            public Setting(Settings settings, string name, string description, T defaultValue, IEnumerable<T> allowedValues)
                : this(settings, name, description, defaultValue, allowedValues, DetectType(typeof(T)))
            {

            }

            public Setting(Settings settings, string name, string description, T defaultValue, IEnumerable<T> allowedValues, SettingDisplayType displayType)
                : base(settings)
            {
                Name = name;
                Description = description;
                Value = defaultValue;
                _allowedValues = allowedValues;
                DisplayType = displayType;
            }

            public Setting(Settings settings, string name, string description, T defaultValue, Func<Setting<T>, IEnumerable<T>> valueProvider)
                : this(settings, name, description, defaultValue, valueProvider, DetectType(typeof(T)))
            {

            }

            public Setting(Settings settings, string name, string description, T defaultValue, Func<Setting<T>, IEnumerable<T>> valueProvider, SettingDisplayType displayType)
                : base(settings)
            {
                Name = name;
                Description = description;
                Value = defaultValue;
                _allowedValueProvider = valueProvider;
                DisplayType = displayType;
            }

            public Setting(Settings settings, string name, string description, T defaultValue)
                : this(settings, name, description, defaultValue, DetectType(typeof(T)))
            {

            }

            public Setting(Settings settings, string name, string description, T defaultValue, SettingDisplayType displayType)
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
