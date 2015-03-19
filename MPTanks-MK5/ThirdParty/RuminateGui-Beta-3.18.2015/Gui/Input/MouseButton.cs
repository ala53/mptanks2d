namespace Microsoft.Xna.Framework.Input
{
    /// <summary>
    /// An enumeration of the buttons on the mouse that are accessible through Starbound Input.
    /// </summary>
    public enum MouseButton
    {
        /// <summary>
        /// Represents the primary, left mouse button.
        /// </summary>
        Left,

        /// <summary>
        /// Represents the middle mouse button.
        /// </summary>
        Middle,

        /// <summary>
        /// Represents the secondary, right mouse button.
        /// </summary>
        Right,

        /// <summary>
        /// Represents an auxiliary button on the mouse. Keep in mind, many mice do not have
        /// the XButton1 and XButton2 buttons.
        /// </summary>
        XButton1,

        /// <summary>
        /// Represents an auxiliary button on the mouse. Keep in mind, many mice do not have
        /// the XButton1 and XButton2 buttons.
        /// </summary>
        XButton2,

        /// <summary>
        /// Represents no button being pressed. Usually for mouse events relating to movement.
        /// </summary>
        None
    };
}
