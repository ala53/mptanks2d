using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Serialization
{
    class GameObjectComponentsJSON
    {
        public string Name { get; set; }
        public string ReflectionName { get; set; }
        public float Lifespan { get; set; }
        /// <summary>
        /// The number of milliseconds to wait after death to remove the object from the game.
        /// </summary>
        public float RemoveAfter { get; set; }
        [JsonProperty("size")]
        public JSONVector DefaultSize { get; set; }
        public GameObjectSheetSpecifierJSON Sheet { get; set; }
        public GameObjectComponentJSON[] Components { get; set; }
        public GameObjectSheetSpecifierJSON[] OtherAssets { get; set; }
        public GameObjectEmitterJSON[] Emitters { get; set; }
        public GameObjectSpriteSpecifierJSON[] OtherSprites { get; set; }

        public static GameObjectComponentsJSON Create(string data)
        {
            //Deserialize
            var me = JsonConvert.DeserializeObject<GameObjectComponentsJSON>(data);
            //Make sure there are no null references
            if (me.Components == null) me.Components = new GameObjectComponentJSON[0];
            if (me.Sheet == null)
                me.Sheet = new GameObjectSheetSpecifierJSON
                {
                    ModName = "engine_base",
                    File = "assets/empty.png",
                    FromOtherMod = true
                };
            if (me.OtherAssets == null) me.OtherAssets = new GameObjectSheetSpecifierJSON[0];
            if (me.Emitters == null) me.Emitters = new GameObjectEmitterJSON[0];
            if (me.OtherSprites == null) me.OtherSprites = new GameObjectSpriteSpecifierJSON[0];

            //Sprites
            foreach (var sp in me.OtherSprites)
            {
                sp.IsAnimation = sp.IsAnimation || sp.Frame.StartsWith("[animation]");
                if (sp.Frame.StartsWith("[animation]"))
                    sp.Frame = sp.Frame.Substring("[animation]".Length);

                if (sp.Sheet == null) sp.Sheet = me.Sheet;

                if (sp.Sheet.Reference != null)
                    sp.Sheet = me.FindSheet(sp.Sheet.Reference);
            }

            //Go through the components
            foreach (var cmp in me.Components)
            {
                if (cmp.Name == null)
                    throw new Exception("Component missing name");
                //Handle unset colors
                if (cmp.Mask == null)
                    cmp.Mask = Color.White;
                //Handle scale being unset
                if (cmp.Scale == null)
                    cmp.Scale = Vector2.One;
                if (cmp.Frame != null && cmp.Frame.StartsWith("[ref]"))
                {
                    var sprite = me.FindSprite(cmp.Frame.Substring("[ref]".Length));
                    if (sprite.IsAnimation)
                        cmp.Frame = "[animation]" + sprite.Frame;
                    else cmp.Frame = sprite.Frame;
                    cmp.Sheet = sprite.Sheet;
                }

                //And again, null ref protection
                if (cmp.Sheet == null)
                    cmp.Sheet = me.Sheet;
                if (cmp.Sheet.Reference != null)
                    cmp.Sheet = me.FindSheet(cmp.Sheet.Reference);

            }
            foreach (var cmp in me.Emitters)
            {
                //Handle namelessness
                if (cmp.Name == null)
                    throw new Exception("Emitter missing name");
                //Deal with null refs in the sub-sprites
                foreach (var sprite in cmp.Sprites)
                {
                    if (sprite.Sheet == null)
                        sprite.Sheet = me.Sheet;
                    if (sprite.Sheet.Reference != null)
                        sprite.Sheet = me.FindSheet(sprite.Sheet.Reference);
                    if (sprite.Frame != null && sprite.Frame.StartsWith("[ref]"))
                    {
                        var spr = me.FindSprite(sprite.Frame.Substring("[ref]".Length));
                        if (spr.IsAnimation)
                            sprite.Frame = "[animation]" + sprite.Frame;
                        else sprite.Frame = sprite.Frame;
                        sprite.Sheet = sprite.Sheet;
                    }
                }

                //Handle activation triggers
                if (cmp.ActivatesOn == null)
                    cmp.ActivatesOn = "create";
                //And actually deal with the activation triggers
                if (cmp.ActivatesOn == "create") cmp.SpawnOnCreate = true;
                else if (cmp.ActivatesOn == "destroy") cmp.SpawnOnDestroy = true;
                else if (cmp.ActivatesOn == "destroy_ended") cmp.SpawnOnDestroyEnded = true;
                else if (cmp.ActivatesOn.StartsWith("t=") && cmp.ActivatesOn.Length > 2)
                {
                    float parse = 0;
                    if (float.TryParse(cmp.ActivatesOn.Substring(2), out parse))
                    {
                        cmp.SpawnAtTime = true;
                        cmp.TimeMsToSpawnAt = parse;
                    }
                }
                else cmp.SpawnOnCreate = true;
            }

            return me;
        }

        private GameObjectSheetSpecifierJSON FindSheet(string name)
        {
            foreach (var sheet in OtherAssets)
                if (sheet.Key != null &&
                    sheet.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    if (sheet.Reference == null)
                        return sheet;
                    else return FindSheet(sheet.Reference);

            foreach (var em in Emitters)
                foreach (var sp in em.Sprites)
                    if (sp.Sheet != null && sp.Sheet.Key != null &&
                        sp.Sheet.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        if (sp.Sheet.Reference == null)
                            return sp.Sheet;
                        else return FindSheet(sp.Sheet.Reference);
            foreach (var cmp in Components)
                if (cmp.Sheet != null && cmp.Sheet.Key != null &&
                    cmp.Sheet.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    if (cmp.Sheet.Reference == null)
                        return cmp.Sheet;
                    else return FindSheet(cmp.Sheet.Reference);
            foreach (var sp in OtherSprites)
                if (sp.Sheet != null && sp.Sheet.Key != null &&
                    sp.Sheet.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    if (sp.Sheet.Reference == null)
                        return sp.Sheet;
                    else return FindSheet(sp.Sheet.Reference);

            return Sheet;
        }

        private GameObjectSpriteSpecifierJSON FindSprite(string name)
        {
            foreach (var sprite in OtherSprites)
            {
                if (sprite.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return sprite;
            }
            return new GameObjectSpriteSpecifierJSON
            {
                Key = "ERROR_NOT_FOUND",
                Frame = null,
                Sheet = Sheet
            };
        }
    }

    class GameObjectComponentJSON
    {
        public int DrawLayer { get; set; }
        public JSONColor Mask { get; set; }
        public string Name { get; set; }
        public JSONVector Offset { get; set; }
        public float Rotation { get; set; }
        public float RotationVelocity { get; set; }
        public JSONVector RotationOrigin { get; set; }
        public JSONVector Scale { get; set; }
        public JSONVector Size { get; set; }
        [DefaultValue(true)]
        public bool Visible { get; set; }
        public string Frame { get; set; }
        public GameObjectSheetSpecifierJSON Sheet { get; set; }
    }
    class GameObjectSheetSpecifierJSON
    {
        public bool FromOtherMod { get; set; }
        public string Key { get; set; }
        [JsonProperty("ref")]
        public string Reference { get; set; }
        public string File { get; set; }
        public string ModName { get; set; }
    }

    class GameObjectSpriteSpecifierJSON
    {
        public string Frame { get; set; }
        public string Key { get; set; }
        public bool IsAnimation { get; set; }
        public GameObjectSheetSpecifierJSON Sheet { get; set; }
    }

    class GameObjectEmitterJSON
    {
        public string Name { get; set; }
        /// <summary>
        /// Can be "create", "destroy", "destroy_ended" "t=[milliseconds]"
        /// </summary>
        public string ActivatesOn { get; set; }
        public bool KeepAliveAfterDeath { get; set; }
        public bool ColorChangedByObjectMask { get; set; }

        public EmissionAreaJSON EmissionArea { get; set; }

        [JsonIgnore]
        public bool SpawnOnCreate { get; set; }
        [JsonIgnore]
        public bool SpawnOnDestroyEnded { get; set; }
        [JsonIgnore]
        public bool SpawnOnDestroy { get; set; }
        [JsonIgnore]
        public bool SpawnAtTime { get; set; }
        [JsonIgnore]
        public float TimeMsToSpawnAt { get; set; }

        public float Lifespan { get; set; }

        public JSONVector MaxAcceleration { get; set; }
        public JSONColor MaxColorMask { get; set; }
        public float MaxFadeInTime { get; set; }
        public float MaxFadeOutTime { get; set; }
        public float MaxLifeSpan { get; set; }
        public float MaxParticlesPerSecond { get; set; }
        public float MaxRotation { get; set; }
        public float MaxRotationVelocity { get; set; }
        public JSONVector MaxSize { get; set; }
        public JSONVector MaxVelocity { get; set; }
        public JSONVector MinAcceleration { get; set; }
        public JSONColor MinColorMask { get; set; }
        public float MinFadeInTime { get; set; }
        public float MinFadeOutTime { get; set; }
        public float MinLifeSpan { get; set; }
        public float MinParticlesPerSecond { get; set; }
        public float MinRotation { get; set; }
        public float MinRotationVelocity { get; set; }
        public JSONVector MinSize { get; set; }
        public JSONVector MinVelocity { get; set; }


        public bool RenderBelowObjects { get; set; }
        public bool ShrinkInsteadOfFadeOut { get; set; }
        public bool GrowInsteadOfFadeIn { get; set; }
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
