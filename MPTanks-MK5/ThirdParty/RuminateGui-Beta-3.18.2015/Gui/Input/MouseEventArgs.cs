using System;

namespace Microsoft.Xna.Framework.Input
{
    /// <summary>
    /// Delegate for mouse events.
    /// </summary>
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);

    /// <summary>
    /// An EventArgs type that is used for mouse-based input events in Starbound.UI.
    /// </summary>
    public sealed class MouseEventArgs : InputEventArgs
    {
        /// <summary>
        /// Gets or sets the mouse button that the event occurred for.
        /// </summary>
        public MouseButton Button { get; private set; }

        /// <summary>
        /// Gets or sets the location of the mouse when the event occured.
        /// </summary>
        public Point Location { get; private set; }

        /// <summary>
        /// Gets or sets the delta of the mouse wheel.
        /// </summary>
        public int Delta { get; private set; }

        /// <summary>
        /// Gets or sets the previous mouse state for the given event. This is what the mouse looked like
        /// in the previous Update.
        /// </summary>
        public MouseState Previous { get; private set; }

        /// <summary>
        /// Gets or sets the current mouse state for the given event. This is what the mouse looked like
        /// at the time the event occurred.
        /// </summary>
        public MouseState Current { get; private set; }

        /// <summary>
        /// Creates a new MouseEventArgs object, based on a time for the event, and the previous and
        /// current mouse states.
        /// </summary>
        public MouseEventArgs(TimeSpan time, MouseState previous, MouseState current, MouseButton button) : base(time)
        {
            Previous = previous;
            Current = current;

            Button = button;
            Location = current.Position;

            Delta = current.ScrollWheelValue - previous.ScrollWheelValue;
        }
    }
}
