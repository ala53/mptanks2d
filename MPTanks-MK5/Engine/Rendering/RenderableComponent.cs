using Microsoft.Xna.Framework;
using MPTanks.Engine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Rendering
{
    public class RenderableComponent : IHasSpriteInfo
    {
        public Vector2 Offset { get; set; } = Vector2.Zero;
        /// <summary>
        /// The origin of the rotation, relative to the offset
        /// </summary>
        public Vector2 RotationOrigin { get; set; } = Vector2.Zero;
        private float _rotation;
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value % (float)(Math.PI * 2); }
        }
        public float RotationVelocity { get; set; } = 0;
        public Vector2 Scale { get; set; } = new Vector2();
        public Color Mask { get; set; } = Color.White;
        public Vector2 Size { get; set; }
        public bool Visible { get; set; } = true;
        public bool IgnoresObjectMask { get; set; } = true;

        /// <summary>
        /// The layer that the object draws on. Higher layers are drawn last while lower ones are drawn first.
        /// So, 0 is below 1 which is below 2 which...etc.
        /// </summary>
        public int DrawLayer { get; set; }

        //And for rendering, we let the renderer know what we want to show
        public SpriteInfo DefaultSprite { get; set; }
        public bool HasDamageLevels => DamageLevels != null && DamageLevels.Length > 0;
        public RenderableComponentDamageLevel[] DamageLevels { get; set; }
        public SpriteInfo SpriteInfo
        {
            get
            {
                if (!HasDamageLevels) return DefaultSprite;

                if (DamageLevels != null)
                    foreach (var damageLevel in DamageLevels)
                    {
                        if (Owner.Health >= damageLevel.MinHealth && Owner.Health <= damageLevel.MaxHealth)
                            return damageLevel.Info;
                    }

                return DefaultSprite;
            }
            set
            {
                if (!HasDamageLevels) DefaultSprite = value;
                if (DamageLevels != null)
                    foreach (var damageLevel in DamageLevels)
                    {
                        if (Owner.Health >= damageLevel.MinHealth && Owner.Health <= damageLevel.MaxHealth)
                            damageLevel.Info = value;
                    }

                DefaultSprite = value;
            }
        }
        public GameObject Owner { get; set; }
        public RenderableComponent(GameObject owner) { Owner = owner; }

        public class RenderableComponentDamageLevel
        {
            public int MaxHealth { get; set; }
            public int MinHealth { get; set; }
            public SpriteInfo Info { get; set; }
        }
    }
}
