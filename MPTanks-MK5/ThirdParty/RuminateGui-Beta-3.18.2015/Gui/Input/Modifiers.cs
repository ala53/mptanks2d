using System;

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>
    /// An enumeration of the modifiers that may apply to a key press, including Control, Shift, and Alt.
    /// </summary>
    [Flags]
    public enum Modifiers 
    {
        /// <summary>
        /// The left or right control key.
        /// </summary>
        Control = 1, 

        /// <summary>
        /// The left or right shift key.
        /// </summary>
        Shift = 2, 

        /// <summary>
        /// The left or right Alt key.
        /// </summary>
        Alt = 4, 

        /// <summary>
        /// No modifiers.
        /// </summary>
        None = 0 
    };
}
