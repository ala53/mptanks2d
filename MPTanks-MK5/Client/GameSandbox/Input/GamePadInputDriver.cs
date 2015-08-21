using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Tanks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MPTanks.Client.GameSandbox.Input
{
    public class GamePadInputDriver : InputDriverBase
    {
        public static string Name => "Xbox 360 Controller / Generic Gamepad input";

        public GamePadInputDriver(GameClient client)
            : base(client, new KeyBindingCollection(
                new KeyBindingCollection.KeyBinding("Fire", Buttons.RightTrigger),
                new KeyBindingCollection.KeyBinding("Open Targeting", Buttons.LeftTrigger),
                new KeyBindingCollection.KeyBinding("Pause", Buttons.Start),
                new KeyBindingCollection.KeyBinding("Look Left", Buttons.RightThumbstickLeft),
                new KeyBindingCollection.KeyBinding("Look Right", Buttons.RightThumbstickRight),
                new KeyBindingCollection.KeyBinding("Look Forward", Buttons.RightThumbstickUp),
                new KeyBindingCollection.KeyBinding("Look Backward", Buttons.RightThumbstickDown),
                new KeyBindingCollection.KeyBinding("Rotate Left", Buttons.LeftThumbstickLeft),
                new KeyBindingCollection.KeyBinding("Rotate Right", Buttons.LeftThumbstickRight),
                new KeyBindingCollection.KeyBinding("Move Forward", Buttons.LeftThumbstickUp),
                new KeyBindingCollection.KeyBinding("Move Backward", Buttons.LeftThumbstickDown),
                new KeyBindingCollection.KeyBinding("Previous Weapon", Buttons.X),
                new KeyBindingCollection.KeyBinding("Next Weapon", Buttons.Y)
                ))
        {

        }

        private float _lastRotation;
        public override InputState GetInputState()
        {
            var state = new InputState();
            state.FirePressed = IsPressed((Buttons)KeyBindings["Fire"]);

            if (IsPressed((Buttons)KeyBindings["Move Forward"]))
                state.MovementSpeed = 
                    MathHelper.Clamp(PressedLevel((Buttons)KeyBindings["Move Forward"]) * 2, 0, 1);
            else if (IsPressed((Buttons)KeyBindings["Move Backward"]))
                state.MovementSpeed = 
                    -MathHelper.Clamp(PressedLevel((Buttons)KeyBindings["Move Backward"]) * 2, 0, 1);

            if (IsPressed((Buttons)KeyBindings["Rotate Left"]))
                state.RotationSpeed = 
                    -MathHelper.Clamp(PressedLevel((Buttons)KeyBindings["Rotate Left"]) * 2, 0, 1);
            else if (IsPressed((Buttons)KeyBindings["Rotate Right"]))
                state.RotationSpeed = 
                    MathHelper.Clamp(PressedLevel((Buttons)KeyBindings["Rotate Right"]) * 2, 0, 1);

            float lookX = 0;
            float lookY = 0;

            if (IsPressed((Buttons)KeyBindings["Look Left"]))
                lookX = -PressedLevel((Buttons)KeyBindings["Look Left"]);
            else if (IsPressed((Buttons)KeyBindings["Look Right"]))
                lookX = PressedLevel((Buttons)KeyBindings["Look Right"]);

            if (IsPressed((Buttons)KeyBindings["Look Forward"]))
                lookY = -PressedLevel((Buttons)KeyBindings["Look Forward"]);
            else if (IsPressed((Buttons)KeyBindings["Look Backward"]))
                lookY = PressedLevel((Buttons)KeyBindings["Look Backward"]);

            if (lookX == 0 && lookY == 0)
                state.LookDirection = _lastRotation;
            else
            {
                state.LookDirection = (float)-Math.Atan2(lookX, lookY) + MathHelper.Pi;
                _lastRotation = state.LookDirection;
            }
            return state;
        }

        public override KeyBindingConfigurationGetPressedKey GetKeyForKeyConfigurationChange()
        {
            foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(button))
                    return new KeyBindingConfigurationGetPressedKey(button, true);

            return new KeyBindingConfigurationGetPressedKey(null, false);
        }

        private bool IsPressed(Buttons state)
        {
            return GamePad.GetState(PlayerIndex.One).IsButtonDown(state);
        }

        public override string GetKeyBindingDisplayString(object binding)
        {
            return Enum.GetName(typeof(GamePadState), binding);
        }

        private float PressedLevel(Buttons state)
        {
            if (!IsPressed(state)) return 0;
            //Non pressure sensitive
            if (state == Buttons.A) return 1;
            if (state == Buttons.B) return 1;
            if (state == Buttons.X) return 1;
            if (state == Buttons.Y) return 1;
            if (state == Buttons.Back) return 1;
            if (state == Buttons.Start) return 1;
            if (state == Buttons.BigButton) return 1;

            if (state == Buttons.LeftShoulder) return 1;
            if (state == Buttons.RightShoulder) return 1;
            if (state == Buttons.LeftStick) return 1;
            if (state == Buttons.RightStick) return 1;

            if (state == Buttons.DPadUp) return 1;
            if (state == Buttons.DPadDown) return 1;
            if (state == Buttons.DPadLeft) return 1;
            if (state == Buttons.DPadRight) return 1;

            if (state == Buttons.LeftTrigger)
                return GamePad.GetState(PlayerIndex.One).Triggers.Left;
            if (state == Buttons.RightTrigger)
                return GamePad.GetState(PlayerIndex.One).Triggers.Right;

            if (state == Buttons.LeftThumbstickUp)
                return Math.Abs(NormalizedThumbStick(true).Y);
            if (state == Buttons.LeftThumbstickDown)
                return Math.Abs(NormalizedThumbStick(true).Y);
            if (state == Buttons.LeftThumbstickRight)
                return Math.Abs(NormalizedThumbStick(true).X);
            if (state == Buttons.LeftThumbstickLeft)
                return Math.Abs(NormalizedThumbStick(true).X);

            if (state == Buttons.RightThumbstickUp)
                return Math.Abs(NormalizedThumbStick(false).Y);
            if (state == Buttons.RightThumbstickDown)
                return Math.Abs(NormalizedThumbStick(false).Y);
            if (state == Buttons.RightThumbstickRight)
                return Math.Abs(NormalizedThumbStick(false).X);
            if (state == Buttons.RightThumbstickLeft)
                return Math.Abs(NormalizedThumbStick(false).X);

            return 0;
        }

        private Vector2 NormalizedThumbStick(bool left)
        {
            Vector2 vect;
            if (left)
                vect = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
            else
                vect = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;

            if (vect.X > 0.45 || vect.Y > 0.45)
                vect.Normalize();

            return vect;
        }
    }
}
