using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MPTanks.Engine.Tanks;
using Microsoft.Xna.Framework.Input;

namespace MPTanks.Client.GameSandbox.Input
{
    public class KeyboardMouseInputDriver : InputDriverBase
    {
        public enum MouseState
        {
            LeftClick,
            RightClick,
            MiddleClick,
            WheelUp,
            WheelDown
        }
        public static string Name => "Keyboard/Mouse input";
        public KeyboardMouseInputDriver(GameClient client)
            : base(client, new KeyBindingCollection(
                new KeyBindingCollection.KeyBinding("Move Forward", Keys.W),
                new KeyBindingCollection.KeyBinding("Move Left", Keys.A),
                new KeyBindingCollection.KeyBinding("Move Backward", Keys.S),
                new KeyBindingCollection.KeyBinding("Move Right", Keys.D),
                new KeyBindingCollection.KeyBinding("Fire", MouseState.LeftClick),
                new KeyBindingCollection.KeyBinding("Previous Weapon", MouseState.WheelDown),
                new KeyBindingCollection.KeyBinding("Next Weapon", MouseState.WheelUp),
                new KeyBindingCollection.KeyBinding("Open Targeting", MouseState.WheelUp)
                ))
        { }
        private bool _lastTickNextWeaponKeyWasActive = false;
        private bool _lastTickPreviousWeaponKeyWasActive = false;
        private int _weaponNumber;
        public override InputState GetInputState()
        {
            if (!Client.IsActive) return default(InputState);
            var inputState = new InputState();

            var screenCenter = new Vector2(Client.GraphicsDevice.Viewport.Bounds.Width / 2, //vertex
                Client.GraphicsDevice.Viewport.Bounds.Height / 2);

            var mousePos = new Vector2(Mouse.GetState().Position.X,  //point a
                Mouse.GetState().Position.Y);

            var ctr = screenCenter - mousePos;
            inputState.LookDirection = (float)-Math.Atan2(-ctr.X, -ctr.Y) + MathHelper.Pi;

            if (IsActive(KeyBindings["Move Forward"]))
                inputState.MovementSpeed = 1;
            else if (IsActive(KeyBindings["Move Backward"]))
                inputState.MovementSpeed = -1;

            if (IsActive(KeyBindings["Move Left"]))
                inputState.RotationSpeed = -1;
            else if (IsActive(KeyBindings["Move Right"]))
                inputState.RotationSpeed = 1;

            if (IsActive(KeyBindings["Fire"]))
                inputState.FirePressed = true;

            if (IsActive(KeyBindings["Previous Weapon"]))
            {
                if (!_lastTickPreviousWeaponKeyWasActive)
                    _weaponNumber--;
                NormalizeWeaponNumber();
                _lastTickPreviousWeaponKeyWasActive = true;
            }
            else
            {
                _lastTickPreviousWeaponKeyWasActive = false;
            }

            if (IsActive(KeyBindings["Next Weapon"]))
            {
                if (!_lastTickNextWeaponKeyWasActive)
                    _weaponNumber++;
                NormalizeWeaponNumber();
                _lastTickNextWeaponKeyWasActive = true;
            }
            else
            {
                _lastTickNextWeaponKeyWasActive = false;
            }

            inputState.WeaponNumber = _weaponNumber;

            return inputState;
        }

        private void NormalizeWeaponNumber()
        {
            if (_weaponNumber < 0) _weaponNumber = 3;
            if (_weaponNumber > 3) _weaponNumber = 0;
        }

        public override KeyBindingConfigurationGetPressedKey GetKeyForKeyConfigurationChange()
        {
            if (Keyboard.GetState().GetPressedKeys().Length == 1)
                return new KeyBindingConfigurationGetPressedKey(Keyboard.GetState().GetPressedKeys()[0], true);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                return new KeyBindingConfigurationGetPressedKey(MouseState.LeftClick, true);
            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
                return new KeyBindingConfigurationGetPressedKey(MouseState.MiddleClick, true);
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
                return new KeyBindingConfigurationGetPressedKey(MouseState.RightClick, true);
            if (_wheelPosDelta > 0)
                return new KeyBindingConfigurationGetPressedKey(MouseState.WheelUp, true);
            if (_wheelPosDelta < 0)
                return new KeyBindingConfigurationGetPressedKey(MouseState.WheelDown, true);

            return new KeyBindingConfigurationGetPressedKey(null, false);
        }

        private bool IsActive(object binding)
        {
            if (binding.GetType() == typeof(MouseState))
            {
                var mouse = (MouseState)binding;
                if (mouse == MouseState.LeftClick && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    return true;
                if (mouse == MouseState.MiddleClick && Mouse.GetState().MiddleButton == ButtonState.Pressed)
                    return true;
                if (mouse == MouseState.RightClick && Mouse.GetState().RightButton == ButtonState.Pressed)
                    return true;

                if (mouse == MouseState.WheelDown && _wheelPosDelta < 0)
                    return true;
                if (mouse == MouseState.WheelUp && _wheelPosDelta > 0)
                    return true;
            }

            if (binding.GetType() == typeof(Keys))
                return Keyboard.GetState().IsKeyDown((Keys)binding);

            return false;
        }

        public override string GetKeyBindingDisplayString(object binding)
        {
            if (binding.GetType() == typeof(Keys))
                return Enum.GetName(typeof(Keys), binding);
            if (binding.GetType() == typeof(MouseState))
                return "Mouse " + Enum.GetName(typeof(MouseState), binding);
            return "Unknown";
        }

        private int _wheelPos;
        private int _wheelPosDelta;
        public override void Update(GameTime gameTime)
        {
            _wheelPosDelta = Mouse.GetState().ScrollWheelValue - _wheelPos;
            _wheelPos = Mouse.GetState().ScrollWheelValue;
            LockCursor();
            base.Update(gameTime);
        }

        private void LockCursor()
        {
            if (!Client.IsActive) return; //Don't lock cursor when out of the window

            Client.IsMouseVisible = false;

            const float outAmount = 100;
            var offset = new Vector2(Client.GraphicsDevice.Viewport.Width / 2,
                Client.GraphicsDevice.Viewport.Height / 2);

            var rel = new Vector2(Math.Abs(Mouse.GetState().Position.X - offset.X),
                Math.Abs(Mouse.GetState().Position.Y - offset.Y));
            var relSigned = new Vector2(Mouse.GetState().Position.X - offset.X,
                Mouse.GetState().Position.Y - offset.Y);

            if (rel.X > outAmount || rel.Y > outAmount)
            {
                if (rel.X > rel.Y)
                {
                    var factor = outAmount / rel.X;
                    relSigned *= factor;
                    Mouse.SetPosition((int)(offset.X + relSigned.X), (int)(offset.Y + relSigned.Y));
                }
                else
                {
                    var factor = outAmount / rel.Y;
                    relSigned *= factor;
                    Mouse.SetPosition((int)(offset.X + relSigned.X), (int)(offset.Y + relSigned.Y));
                }
            }
        }
    }
}
