using System;

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>
    /// An abstraction around keyboard input that turns XNA's underlying polling model into an event-based
    /// model for keyboard input.
    /// </summary>
    public class KeyboardEvents
    {
        /// <summary>
        /// Represents the amount of time between a key being pressed, and the time that key typed events
        /// start repeating. This is measured in milliseconds. The initial delay is traditionally 
        /// significantly longer than other delays. The default is 800 milliseconds.
        /// </summary>
        public static int InitialDelay { get; set; }

        /// <summary>
        /// Represents the amount of time delay between key typed events after the first repeat. This 
        /// "normal" repeat delay is typically much faster than the initial. The default is 50 milliseconds
        /// (20 times per second).
        /// </summary>
        public static int RepeatDelay { get; set; }
        
        /// <summary>
        /// Stores the last keyboard state from the previous update.
        /// </summary>
        private KeyboardState _previous;

        /// <summary>
        /// Stores the last key that was pressed. Used in tracking when keys are held down for
        /// a prolonged time and need to repeatedly raise key typed events.
        /// </summary>
        private Keys _lastKey;

        /// <summary>
        /// Stores the last time that a key was pressed. Used in tracking when keys are held down for
        /// a prolonged time and need to repeatedly raise key typed events.
        /// </summary>
        private TimeSpan _lastPress;

        /// <summary>
        /// Indicates whether the last key press was an initial press or not.
        /// </summary>
        private bool _isInitial;

        /// <summary>
        /// Sets up the class with defaults.
        /// </summary>
        static KeyboardEvents()
        {
            InitialDelay = 800;
            RepeatDelay = 50;
        }

        /// <summary>
        /// Updates the component, turning XNA's polling model into an event-based model, raising
        /// events as they happen.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            var current = Keyboard.GetState();

            // Build the modifiers that currently apply to the current situation.
            var modifiers = Modifiers.None;
            if (current.IsKeyDown(Keys.LeftControl) || current.IsKeyDown(Keys.RightControl)) {
                modifiers |= Modifiers.Control;
            }
            if (current.IsKeyDown(Keys.LeftShift) || current.IsKeyDown(Keys.RightShift)) {
                modifiers |= Modifiers.Shift;
            }
            if (current.IsKeyDown(Keys.LeftAlt) || current.IsKeyDown(Keys.RightAlt)) {
                modifiers |= Modifiers.Alt;
            }
            
            // Key pressed and initial key typed events for all keys.
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (current.IsKeyDown(key) && _previous.IsKeyUp(key))
                {
                    OnKeyPressed(this, new KeyEventArgs(gameTime.TotalGameTime, key, modifiers, current));
                    var ch = KeyboardUtil.ToChar(key, modifiers);
                    if (ch.HasValue) {
                        OnKeyTyped(this, new CharacterEventArgs(gameTime.TotalGameTime, ch.Value, modifiers, current));
                    }

                    // Maintain the state of last key pressed.
                    _lastKey = key;
                    _lastPress = gameTime.TotalGameTime;
                    _isInitial = true;
                }
            }

            // Key released events for all keys.
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (current.IsKeyUp(key) && _previous.IsKeyDown(key)) 
                {
                    OnKeyReleased(this, new KeyEventArgs(gameTime.TotalGameTime, key, modifiers, current));
                }
            }

            // Handle keys being held down and getting multiple KeyTyped events in sequence.
            var elapsedTime = (gameTime.TotalGameTime - _lastPress).TotalMilliseconds;

            if (current.IsKeyDown(_lastKey) 
                && ((_isInitial && elapsedTime > InitialDelay) 
                || (!_isInitial && elapsedTime > RepeatDelay)))
            {
                var ch = KeyboardUtil.ToChar(_lastKey, modifiers);
                if (ch.HasValue) 
                {
                    OnKeyTyped(this, new CharacterEventArgs(gameTime.TotalGameTime, ch.Value, modifiers, current));
                    _lastPress = gameTime.TotalGameTime;
                    _isInitial = false;
                }
            }

            _previous = current;
        }

        /// <summary>
        /// Raises the KeyPressed event. This is done automatically by a correctly configured component,
        /// but this is exposed publicly to allow programmatic key press events to occur.
        /// </summary>
        public void OnKeyPressed(object sender, KeyEventArgs args)
        {
            if (KeyPressed != null) { KeyPressed(sender, args); }
        }

        /// <summary>
        /// Raises the KeyReleased event. This is done automatically by a correctly configured component,
        /// but this is exposed publicly to allow programmatic key release events to occur.
        /// </summary>
        public void OnKeyReleased(object sender, KeyEventArgs args)
        {
            if (KeyReleased != null) { KeyReleased(sender, args); }
        }

        /// <summary>
        /// Raises the KeyTyped event. This is done automatically by a correctly configured component,
        /// but this is exposed publicly to allow programmatic key typed events to occur.
        /// </summary>
        public void OnKeyTyped(object sender, CharacterEventArgs args)
        {
            if (KeyTyped != null) { KeyTyped(sender, args); }
        }

        /// <summary>
        /// An event that is raised when a key is first pressed.
        /// </summary>
        public static event KeyEventHandler KeyPressed;

        /// <summary>
        /// An event that is raised when a key is released.
        /// </summary>
        public static event KeyEventHandler KeyReleased;

        /// <summary>
        /// An event that is raised when a key is first pressed, and then periodically again afterwards
        /// until the key is released. There is a longer initial delay, determined by 
        /// KeyboardEvents.InitialDelay, and then subsequent repeats happen at regular intervals as 
        /// determined by KeyboardEvents.RepeatDelay.
        /// </summary>
        public static event CharEnteredHandler KeyTyped;
    }
}