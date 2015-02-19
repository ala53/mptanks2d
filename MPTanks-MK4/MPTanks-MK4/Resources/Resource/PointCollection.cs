using Newtonsoft.Json;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MPTanks_MK4.Resources.Resource
{
    /// <summary>
    /// A collection of points that will be translated, binding to some other translation.
    /// </summary>
    public class PointCollection
    {
        /// <summary>
        /// The object that these particular points are bound to.
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public ushort BoundObject { get; set; }

        /// <summary>
        /// The represented points
        /// </summary>
        public Dictionary<string, Point> Points { get; private set; }
        public class Point
        {
            public Vector2 StartingLocation;
            public string BindingObject;
            public Vector2 CurrentLocation;
            public Animation.TransformationTypes BindingTransformations;
            public Point DeepClone()
            {
                return new Point()
                {
                    StartingLocation = StartingLocation,
                    BindingObject = BindingObject,
                    CurrentLocation = CurrentLocation,
                    BindingTransformations = BindingTransformations
                };
            }
        }

        public PointCollection DeepClone(ushort newObjectId)
        {
            var np = new PointCollection();
            np.BoundObject = newObjectId;
            np.Points = new Dictionary<string, Point>();
            foreach (var pt in Points)
                np.Points.Add(pt.Key, pt.Value.DeepClone());
            return np;
        }

    }

}
