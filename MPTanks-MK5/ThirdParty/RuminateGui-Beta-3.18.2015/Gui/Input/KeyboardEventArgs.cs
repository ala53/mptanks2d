using System;

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>
    /// Delegate for non character related keyboard events.
    /// </summary>
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);

    /// <summary>
    /// Represents an EventArgs object that is for all keyboard events in Starbound.UI.
    /// </summary>
    public class KeyEventArgs : InputEventArgs {

        /// <summary>
        /// The current set of modifiers that are in use.
        /// </summary>
        public Modifiers Modifiers { get; protected set; }

        /// <summary>
        /// The key that is involved in the event.
        /// </summary>
        public Keys KeyCode { get; set; }

        /// <summary>
        /// Creates a new KeyboardEventArgs, given a time for the event, the key that was pressed, and
        /// the modifiers that were applied at the time of the press, as well as the keyboard state at 
        /// the time the event occurred.
        /// </summary>
        public KeyEventArgs(TimeSpan time, Keys key, Modifiers modifiers, KeyboardState state) : base(time) {

            Modifiers = modifiers;
            KeyCode = key;
        }
    }
}
