using MPTanks_MK4.Animation;
using MPTanks_MK4.Resources;
using MPTanks_MK4.Resources.Resource;
using MPTanks_MK4.StateManagement.States;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4.GameObjects
{
    abstract class GameObject
    {
        public Rendering.RenderableComponent[] Components { get; set; }
        public Matrix4 Matrix { get; set; }
        public ushort ObjectId { get; set; }
        public TransformationHelper Animator { get; set; }
        public PointCollection Points { get; set; }
        public abstract ObjectState GetState();
    }
}
