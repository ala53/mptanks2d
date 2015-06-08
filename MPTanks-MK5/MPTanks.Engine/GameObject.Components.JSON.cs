using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Serialization
{
    class GameObjectComponentsJSON
    {
        public string Name { get; set; }
        public string ReflectionName { get; set; }
        public GameObjectSheetSpecifierJSON Sheet { get; set; }
        public GameObjectComponentJSON[] Components { get; set; }
        public GameObjectSheetSpecifierJSON[] OtherAssets { get; set; }
        public GameObjectEmitterJSON[] Emitters { get; set; }

        public static GameObjectComponentsJSON Create(string data)
        {
            var me = JsonConvert.DeserializeObject<GameObjectComponentsJSON>(data);
            foreach (var cmp in me.Components)
            {
                if (cmp.Name == null)
                {
                    throw new Exception("Component missing name");
                }
                if (cmp.Sheet == null)
                    cmp.Sheet = me.Sheet;
            }
            foreach (var cmp in me.Emitters)
            {
                if (cmp.Name == null)
                    throw new Exception("Emitter missing name");
                foreach (var sprite in cmp.Sprites)
                {
                    if (sprite.Sheet == null)
                        sprite.Sheet = me.Sheet;
                }
            }

            return me;
        }
    }
    class GameObjectComponentJSON
    {
        public int DrawLayer { get; set; }
        public Color Mask { get; set; }
        public string Name { get; set; }
        public JSONVector Offset { get; set; }
        public float Rotation { get; set; }
        public JSONVector RotationOrigin { get; set; }
        public float Scale { get; set; }
        public JSONVector Size { get; set; }
        public bool Visible { get; set; }

        public string Frame { get; set; }
        public GameObjectSheetSpecifierJSON Sheet { get; set; }
    }
    class GameObjectSheetSpecifierJSON
    {
        public bool FromOtherMod { get; set; }
        public string Key { get; set; }
        public string AssetName { get; set; }
        public string ModName { get; set; }
    }
    
    class GameObjectEmitterJSON
    {
        public string Name { get; set; }
        public bool Paused { get; set; }
        public bool ColorChangedByObjectMask { get; set; }

        public EmissionAreaJSON EmissionArea { get; set; }

        public JSONVector MaxAcceleration { get; set; }
        public Color MaxColorMask { get; set; }
        public float MaxFadeInTime { get; set; }
        public float MaxFadeOutTime { get; set; }
        public float MaxLifeSpan { get; set; }
        public float MaxParticlesPerSecond { get; set; }
        public float MaxRotation { get; set; }
        public float MaxRotationVelocity { get; set; }
        public JSONVector MaxSize { get; set; }
        public JSONVector MaxVelocity { get; set; }
        public JSONVector MinAcceleration { get; set; }
        public Color MinColorMask { get; set; }
        public float MinFadeInTime { get; set; }
        public float MinFadeOutTime { get; set; }
        public float MinLifeSpan { get; set; }
        public float MinParticlesPerSecond { get; set; }
        public float MinRotation { get; set; }
        public float MinRotationVelocity { get; set; }
        public JSONVector MinSize { get; set; }
        public JSONVector MinVelocity { get; set; }


        public bool RenderBelowObjects { get; set; }
        public bool ShrinkInsteadOfFade { get; set; }
        public bool SizeScalingUniform { get; set; }
        public SpriteJSON[] Sprites { get; set; }
        public bool VelocityAndAccelerationTrackRotation { get; set; }
        public bool VelocityRelativeToObject { get; set; }
        public class SpriteJSON
        {
            public string Frame { get; set; }
            public GameObjectSheetSpecifierJSON Sheet { get; set; }
        }
        public class EmissionAreaJSON
        {
            public float H { get; set; }
            public float W { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            public bool TracksObject { get; set; }
        }
    }
}
