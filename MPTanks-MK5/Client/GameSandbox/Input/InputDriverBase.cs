using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.GameSandbox.Input
{
    public abstract class InputDriverBase
    {
        public string DisplayName { get; private set; }
        public GameClient Client { get; private set; }
        public bool Activated => Client.IsActive && _active;
        private bool _active;

        public InputDriverBase(GameClient client, KeyBindingCollection bindings)
        {
            var name = GetType().GetProperty("Name");
            if (name == null || name.PropertyType != typeof(string) ||
                !name.GetMethod.IsStatic)
                throw new Exception("Every input driver must declare a static property \"Name\" of type string");

            DisplayName = (string)name.GetValue(null);

            Client = client;
            KeyBindings = bindings;
        }

        public abstract Engine.Tanks.InputState GetInputState();
        protected virtual void DriverActiveStateChanged(bool isNowActive)
        {

        }
        public void Activate()
        {
            if (_active) return; //no state change
            _active = true;
            DriverActiveStateChanged(true);
        }
        public void Deactivate()
        {
            if (!_active) return; //no state change
            _active = false;
            DriverActiveStateChanged(false);
        }
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Dispose()
        {

        }

        public KeyBindingCollection KeyBindings { get; protected set; }

        public virtual string GetKeyBindingDisplayString(object binding)
        {
            return binding.ToString();
        }

        public struct KeyBindingConfigurationGetPressedKey
        {
            public readonly object Key;
            public readonly bool HasKeyPressed;
            public KeyBindingConfigurationGetPressedKey(object key, bool hasKeyPressed)
            {
                Key = key;
                HasKeyPressed = hasKeyPressed;
            }
        }

        public abstract KeyBindingConfigurationGetPressedKey GetKeyForKeyConfigurationChange();

        public virtual void SetKeyBindings(string saved) => KeyBindings.Load(saved);

        public virtual string SaveKeyBindings() => KeyBindings.Save();

        #region Register drivers
        private static Dictionary<string, Type> _drivers =
            new Dictionary<string, Type>();
        public static IReadOnlyDictionary<string, Type> Drivers => _drivers;

        static InputDriverBase()
        {
            RegisterDriver(KeyboardMouseInputDriver.Name, typeof(KeyboardMouseInputDriver));
            RegisterDriver(GamePadInputDriver.Name, typeof(GamePadInputDriver));
        }

        public static void RegisterDriver(string name, Type type)
        {
            _drivers.Add(name, type);
        }

        public static InputDriverBase GetDriver(string name, GameClient client) =>
            (InputDriverBase)Activator.CreateInstance(_drivers[name], client);
        #endregion
    }
}
