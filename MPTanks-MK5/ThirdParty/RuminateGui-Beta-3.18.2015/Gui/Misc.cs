namespace Ruminate.GUI.Framework {

    /* ## Enumerations ## */    

    public enum Direction { None, Vertical, Horizontal, Both }

    public enum CardinalDirection { North, South, East, West }

    public enum TextHorizontal { LeftAligned, CenterAligned, RightAligned }

    public enum TextVertical { TopAligned, CenterAligned, BottomAligned }

    /* ## Event Delegates ## */

    public delegate void WidgetEvent(Widget sender);
}
