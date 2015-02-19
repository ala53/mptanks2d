using OpenTK.Graphics;
using MPTanks_MK4.GameObjects;
using MPTanks_MK4.Rendering;
using MPTanks_MK4.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using MPTanks_MK4.Resources.Resource;

namespace MPTanks_MK4.Animation
{
    class TransformationHelper
    {
        private GameObjects.GameObject bindingObject;

        public TransformationHelper(GameObject obj)
        {
            bindingObject = obj;
        }

        public void Rotate(RenderableComponent renderComponent, PointCollection.Point center, float rotateAmount)
        {
            var offset = renderComponent.Offset;   
            //Assume we are using degrees
            offset *= Matrix4.CreateRotationZ(rotateAmount * (float)(Math.PI / 180));
            renderComponent.Offset = offset;

            //And auto move the points with it.
            foreach (PointCollection.Point p in bindingObject.Points.Points.Values)
            {
                if (p.BindingObject == renderComponent.Name)
                {
                    //Transform it too
                    var v3 = Vector3.Transform(new Vector3(p.StartingLocation.X, p.StartingLocation.Y, 0), offset);
                    p.CurrentLocation = new Vector2(v3.X, v3.Y);
                }
            }
        }
        public void Translate(RenderableComponent renderComponent, PointCollection.Point center, Vector2 translation)
        {

        }
        public void Scale(RenderableComponent renderComponent, PointCollection.Point center, Vector2 scale)
        {

        }
        public void ScaleScalar(RenderableComponent renderComponent, PointCollection.Point center, double scale)
        {

        }
        public void ClearTransforms(RenderableComponent renderComponent)
        {

        }
    }
}
