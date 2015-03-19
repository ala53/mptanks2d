using Microsoft.Xna.Framework;

namespace Ruminate.GUI.Framework {

    // A component responsible for storing the location of an object
    // and aligning it in local space.
    public class LayoutData {

        /*####################################################################*/
        /*                              Location                              */
        /*####################################################################*/

        // #############################################################
        // All of these are calculated by what ever class extends this one.
        // The extending class should be rule based and calculate these
        // values in CalculateDimentions and CalculateTranslation.        

        protected Rectangle _renderArea;
        protected Rectangle _childArea;
        protected Rectangle _sissorArea;

        /// <summary>
        /// The screen space the widget occupies.
        /// </summary>
        public Rectangle RenderArea { get { return _renderArea; } }

        /// <summary>
        /// The area to the widget where the children can receive input.
        /// </summary>        
        public Rectangle ChildArea { get { return _childArea; } }

        /// <summary>
        /// Area children are allowed to be rendered.
        /// </summary>
        public Rectangle SissorArea { get { return _sissorArea; } }

        // #############################################################
        // Allows translation of child widgets. For use by containers
        // that modify the location of their children.

        // The amount child widget should be offset from their 
        // specified locations. Stacks.
        public Point ChildOffset { get; protected set; }

        public void SetChildOffsetX(int x) {
            if (ChildOffset.X == x) { return; }
            ChildOffset = new Point(x, ChildOffset.Y);
        }

        public void SetChildOffsetY(int y) {
            if (ChildOffset.Y == y) { return; }
            ChildOffset = new Point(ChildOffset.X, y);
        }     
    }
}
