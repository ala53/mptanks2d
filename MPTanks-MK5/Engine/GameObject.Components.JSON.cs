using FarseerPhysics.Common;
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
    public class GameObjectComponentsJSON
    {
        public string Name { get; set; }
        public string ReflectionName { get; set; }
        public double Lifespan { get; set; }
        public int Health { get; set; }
        /// <summary>
        /// The number of milliseconds to wait after death to remove the object from the game.
        /// </summary>
        public float RemoveAfter { get; set; }
        [JsonProperty("size")]
        public JSONVector DefaultSize { get; set; }
        public GameObjectSheetSpecifierJSON Sheet { get; set; }
        public GameObjectComponentGroupJSON[] ComponentGroups { get; set; }
        public GameObjectComponentJSON[] Components { get; set; }
        public GameObjectSheetSpecifierJSON[] OtherAssets { get; set; }
        public GameObjectEmitterJSON[] Emitters { get; set; }
        public GameObjectSpriteSpecifierJSON[] OtherSprites { get; set; }
        public GameObjectLightJSON[] Lights { get; set; }
        public GameObjectAnimationJSON[] Animations { get; set; }
        public GameObjectBodySpecifierJSON Body { get; set; }
        public string __image__body { get; set; } //Disregard: just for compiler
        public string[] Flags { get; set; }
        public int DrawLayer { get; set; }
        public bool Invincible { get; set; }

        internal static GameObjectComponentsJSON Create(string data)
        {
            //Deserialize
            var me = JsonConvert.DeserializeObject<GameObjectComponentsJSON>(data);
            //Make sure there are no null references
            if (me.Sheet == null)
                me.Sheet = new GameObjectSheetSpecifierJSON
                {
                    ModName = "engine_base",
                    File = "assets/empty.png",
                    FromOtherMod = true
                };
            if (me.Health <= 0) me.Health = int.MaxValue;
            if (me.Components == null) me.Components = new GameObjectComponentJSON[0];
            if (me.OtherAssets == null) me.OtherAssets = new GameObjectSheetSpecifierJSON[0];
            if (me.Emitters == null) me.Emitters = new GameObjectEmitterJSON[0];
            if (me.OtherSprites == null) me.OtherSprites = new GameObjectSpriteSpecifierJSON[0];
            if (me.ComponentGroups == null) me.ComponentGroups = new GameObjectComponentGroupJSON[0];
            if (me.Animations == null) me.Animations = new GameObjectAnimationJSON[0];
            if (me.Lights == null) me.Lights = new GameObjectLightJSON[0];
            if (me.Flags == null) me.Flags = new string[0];

            me.ProcessSprites();
            me.ProcessComponents();
            me.ProcessEmitters();
            me.ProcessAnimations();
            me.ProcessLights();
            me.BuildGroups();

            return me;
        }

        private void ProcessSprites()
        {

            //Sprites
            foreach (var sp in OtherSprites)
            {
                ValidateKey(sp);

                HandleSprite(sp);

                HandleSheet(sp);
            }
        }

        private void ProcessComponents()
        {

            //Go through the components
            foreach (var cmp in Components)
            {
                Validate(cmp);
                //Handle unset colors
                if (cmp.Mask == null)
                    cmp.Mask = Color.White;
                //Handle scale being unset
                if (cmp.Scale == null)
                    cmp.Scale = Vector2.One;

                if (cmp.Image != null && cmp.Image.Frame != null)
                {
                    var img = ResolveSpriteObjectReference(cmp.Image.Frame);
                    if (img != null)
                        cmp.Image = img;

                    HandleSprite(cmp.Image);
                }

                if (cmp.Image != null && cmp.Image.DamageLevels != null)
                {
                    for (var i = 0; i < cmp.Image.DamageLevels.Length; i++)
                    {
                        var img = ResolveSpriteObjectReference(cmp.Image.DamageLevels[i].Sprite.Frame);
                        HandleSheet(cmp.Image.DamageLevels[i].Sprite);
                        if (img != null)
                            cmp.Image.DamageLevels[i].Sprite = img;

                        HandleSprite(cmp.Image.DamageLevels[i].Sprite);
                    }
                }

                HandleSheet(cmp.Image);
            }
        }

        private void ProcessEmitters()
        {
            foreach (var emitter in Emitters)
            {
                //Handle namelessness
                Validate(emitter);
                //Deal with null refs in the sub-sprites
                for (var i = 0; i < emitter.Sprites.Length; i++)
                {
                    var img = ResolveSpriteObjectReference(emitter.Sprites[i].Frame);
                    if (img != null)
                        emitter.Sprites[i] = img;
                    var sprite = emitter.Sprites[i];
                    HandleSheet(sprite);
                    HandleSprite(sprite);
                }

                HandleTriggers(emitter);
            }
        }

        private void ProcessAnimations()
        {
            foreach (var anim in Animations)
            {
                Validate(anim);
                HandleTriggers(anim);

                for (var i = 0; i < anim.SpriteOptions.Length; i++)
                {
                    var img = ResolveSpriteObjectReference(anim.SpriteOptions[i].Frame);
                    if (img != null)
                        anim.SpriteOptions[i] = img;
                    var sprite = anim.SpriteOptions[i];

                    HandleSheet(sprite);
                    HandleSprite(sprite);
                }
            }
        }

        private void ProcessLights()
        {
            foreach (var light in Lights)
            {
                Validate(light);
                var img = ResolveSpriteObjectReference(light.Image.Frame);
                if (img != null)
                    light.Image = img;
                HandleSheet(light.Image);
                HandleSprite(light.Image);
                HandleTriggers(light);

                //resolve the component
                if (light.TracksComponent && light.ComponentToTrack != null)
                    foreach (var cmp in Components)
                        if (cmp.Name.Equals(light.ComponentToTrack, StringComparison.InvariantCultureIgnoreCase))
                            light.Component = cmp;

                //And process team ids
                if (light.TeamsToDisplayFor == null)
                    light.ShowForAllTeams = true;
                else if (light.TeamsToDisplayFor.Equals("all", StringComparison.InvariantCultureIgnoreCase))
                    light.ShowForAllTeams = true;
                else if (light.TeamsToDisplayFor.Equals("tank.team", StringComparison.InvariantCultureIgnoreCase))
                    light.ShowForTankTeamOnly = true;
                else
                {
                    string[] split = light.TeamsToDisplayFor.Split(',');
                    var teams = new List<short>();
                    foreach (var team in split)
                    {
                        short parsed;
                        if (!short.TryParse(team, out parsed))
                            throw new Exception($"{light.TeamsToDisplayFor} contains an invalid item: {team}");

                        teams.Add(parsed);
                    }
                    light.TeamIds = teams.ToArray();
                }
            }
        }

        private void BuildGroups()
        {

            //And finally, resolve component groups
            foreach (var grp in ComponentGroups)
            {
                var cmps = new List<GameObjectComponentJSON>();
                foreach (var cmp in grp.ComponentStrings)
                    foreach (var component in Components)
                    {
                        if (component.Name.Equals(cmp, StringComparison.InvariantCultureIgnoreCase))
                        {
                            cmps.Add(component);
                            break;
                        }
                    }
            }
        }

        private void Validate(INameValidatable validatable)
        {
            if (validatable.Name != null && validatable.Name.Length > 0)
                return;
            throw new Exception("Name cannot be null");
        }

        private void HandleSheet(IHasSheet obj)
        {
            if (obj == null) return;
            if (obj.Sheet == null)
                obj.Sheet = Sheet;
            if (obj.Sheet.Reference != null)
                obj.Sheet = FindSheet(obj.Sheet.Reference);
        }

        private void HandleTriggers(ITriggerable obj)
        {
            //Handle activation triggers
            if (obj.ActivatesOn == null)
                obj.ActivatesOn = "create";
            //And actually deal with the activation triggers
            if (obj.ActivatesOn.StartsWith("t=") && obj.ActivatesOn.Length > 2)
            {
                double parse = 0;
                if (double.TryParse(obj.ActivatesOn.Substring(2), out parse))
                {
                    obj.ActivatesAtTime = true;
                    obj.TimeToSpawnAt = TimeSpan.FromMilliseconds(parse);
                }
            }
            else
            {
                obj.ActivationIsTriggered = true;
                obj.TriggerName = obj.ActivatesOn.ToLower();
            }
        }

        private void ValidateKey(IRequiresKey obj)
        {
            if (obj.Key != null && obj.Key.Length > 0)
                return;
            throw new Exception("Missing key!");
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
            foreach (var sp in OtherSprites)
                if (sp.Sheet != null && sp.Sheet.Key != null &&
                    sp.Sheet.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    if (sp.Sheet.Reference == null)
                        return sp.Sheet;
                    else return FindSheet(sp.Sheet.Reference);
            foreach (var anim in Animations)
                foreach (var sp in anim.SpriteOptions)
                    if (sp.Sheet != null && sp.Sheet.Key != null &&
                        sp.Sheet.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        if (sp.Sheet.Reference == null)
                            return sp.Sheet;
                        else return FindSheet(sp.Sheet.Reference);
            foreach (var light in Lights)
                if (light.Image != null && light.Image.Sheet != null && light.Image.Sheet.Key != null &&
                    light.Image.Sheet.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    if (light.Image.Sheet.Reference == null)
                        return light.Image.Sheet;
                    else return FindSheet(light.Image.Sheet.Reference);

            return Sheet;
        }

        private string ResolveSpriteReference(string reference, IHasSheet obj)
        {
            ResolveSpriteReference(ref reference, obj);
            return reference;
        }
        private void ResolveSpriteReference(ref string reference, IHasSheet obj)
        {
            if (reference == null || !reference.StartsWith("[ref]")) return;

            var spr = FindSprite(reference.Substring("[ref]".Length));
            if (spr.IsAnimation)
                reference = "[animation]" + spr.Frame;
            else reference = spr.Frame;

            obj.Sheet = spr.Sheet;
        }

        private GameObjectSpriteSpecifierJSON ResolveSpriteObjectReference(string reference)
        {
            if (reference == null || !reference.StartsWith("[ref]")) return null;

            var spr = FindSprite(reference.Substring("[ref]".Length));
            return spr;
        }

        private void HandleSprite(GameObjectSpriteSpecifierJSON sprite)
        {
            if (sprite != null && sprite.Frame != null && sprite.Frame.StartsWith("[animation]")) sprite.IsAnimation = true;
            if (sprite != null && sprite.Frame != null && sprite.Frame.StartsWith("[animation]"))
                sprite.Frame = sprite.Frame.Substring("[animation]".Length);
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

    public class GameObjectComponentGroupJSON : IRequiresKey
    {
        public string Key { get; set; }
        [JsonProperty("components")]
        public string[] ComponentStrings { get; set; }
        [JsonIgnore]
        public GameObjectComponentJSON[] Components { get; set; }
    }

    public class GameObjectComponentJSON : INameValidatable
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
        public GameObjectSpriteSpecifierJSON Image { get; set; }
        public bool IgnoresObjectMask { get; set; }
    }
    public class GameObjectSheetSpecifierJSON
    {
        public bool FromOtherMod { get; set; }
        public string Key { get; set; }
        [JsonProperty("ref")]
        public string Reference { get; set; }
        public string File { get; set; }
        public string ModName { get; set; }
    }

    public class GameObjectSpriteSpecifierJSON : IHasSheet, IRequiresKey
    {
        public string Frame { get; set; }
        public string Key { get; set; }
        public bool IsAnimation { get; set; }
        public GameObjectSpriteSpecifierDamageLevelJSON[] DamageLevels { get; set; }
        public GameObjectSheetSpecifierJSON Sheet { get; set; }

        public class GameObjectSpriteSpecifierDamageLevelJSON
        {
            public int MaxHealth { get; set; }
            public int MinHealth { get; set; }
            public GameObjectSpriteSpecifierJSON Sprite { get; set; }
        }
    }

    public class GameObjectEmitterJSON : ITriggerable, INameValidatable
    {
        public string Name { get; set; }
        public bool KeepAliveAfterDeath { get; set; }
        public bool ColorChangedByObjectMask { get; set; }

        public EmissionAreaJSON EmissionArea { get; set; }

        /// <summary>
        /// Can be "create", "destroy", "destroy_ended" "t=[milliseconds]"
        /// </summary>
        public string ActivatesOn { get; set; }
        public bool ActivationIsTriggered { get; set; }
        public string TriggerName { get; set; }
        public bool ActivatesAtTime { get; set; }
        public TimeSpan TimeToSpawnAt { get; set; }

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
        public GameObjectSpriteSpecifierJSON[] Sprites { get; set; }
        public bool VelocityAndAccelerationTrackRotation { get; set; }
        public bool VelocityRelativeToObject { get; set; }
        public class EmissionAreaJSON
        {
            public float H { get; set; }
            public float W { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            public bool TracksObject { get; set; }
        }
    }

    public class GameObjectLightJSON : ITriggerable, INameValidatable
    {
        public string Name { get; set; }
        public float Intensity { get; set; }
        public JSONColor Color { get; set; }
        public JSONVector Position { get; set; }
        public bool TracksObject { get; set; }
        public bool TracksComponent { get; set; }
        [JsonIgnore]
        public GameObjectComponentJSON Component { get; set; }
        public string ComponentToTrack { get; set; }
        public JSONVector Size { get; set; }
        public GameObjectSpriteSpecifierJSON Image { get; set; }
        public string ActivatesOn { get; set; }
        public bool ActivatesAtTime { get; set; }
        public TimeSpan TimeToSpawnAt { get; set; }
        public bool ActivationIsTriggered { get; set; }
        public string TriggerName { get; set; }
        /// <summary>
        /// can be tank.team, comma separated list of 1,2,3,4,5, or all
        /// </summary>
        public string TeamsToDisplayFor { get; set; }
        [JsonIgnore]
        public short[] TeamIds { get; set; }
        [JsonIgnore]
        public bool ShowForTankTeamOnly { get; set; }
        [JsonIgnore]
        public bool ShowForAllTeams { get; set; }
    }

    public class GameObjectAnimationJSON : ITriggerable, INameValidatable
    {
        public string ActivatesOn { get; set; }
        public bool ActivatesAtTime { get; set; }
        public TimeSpan TimeToSpawnAt { get; set; }
        public bool ActivationIsTriggered { get; set; }
        public string TriggerName { get; set; }
        public GameObjectSpriteSpecifierJSON[] SpriteOptions { get; set; }
        public int LoopCount { get; set; }
        public string Name { get; set; }
        public JSONVector Position { get; set; }
        public bool TracksObject { get; set; }
        public float Rotation { get; set; }
        public JSONVector Size { get; set; }
        public JSONColor Mask { get; set; }
        public JSONVector RotationOrigin { get; set; }
        public int DrawLayer { get; set; }
        public float StartPositionMs { get; set; }
    }

    public interface ITriggerable
    {
        string ActivatesOn { get; set; }
        [JsonIgnore]
        bool ActivatesAtTime { get; set; }
        [JsonIgnore]
        bool ActivationIsTriggered { get; set; }
        [JsonIgnore]
        TimeSpan TimeToSpawnAt { get; set; }
        [JsonIgnore]
        string TriggerName { get; set; }
    }

    public class GameObjectBodySpecifierJSON
    {
        public JSONVector Size { get; set; }
        public FixtureSpecifierJSON[] Fixtures { get; set; }
        private Dictionary<Vector2, List<Vertices>> _cache = new Dictionary<Vector2, List<Vertices>>();
        internal List<Vertices> GetFixtures(Vector2 desiredSize)
        {
            if (_cache.ContainsKey(desiredSize))
                return _cache[desiredSize];

            var vertList = new List<Vertices>();
            foreach (var fixture in Fixtures)
            {
                var vtx = fixture.Rebuild();
                vtx.Scale(desiredSize / Size);
                vertList.Add(vtx);
            }

            _cache.Add(desiredSize, vertList);
            return vertList;
        }
        public class FixtureSpecifierJSON
        {
            public JSONVector[] Vertices { get; set; }
            public HolesSpecifierJSON[] Holes { get; set; }

            internal Vertices Rebuild()
            {
                var verts = new Vertices(Vertices.Length);
                foreach (var vert in Vertices)
                    verts.Add(vert);

                if (Holes != null && Holes.Length > 0)
                    foreach (var hole in Holes)
                    {
                        var hl = new Vertices(hole.Vertices.Length);
                        foreach (var vert in hole.Vertices)
                            hl.Add(vert);

                        verts.Holes.Add(hl);
                    }
                return verts;
            }

            public class HolesSpecifierJSON
            {
                public JSONVector[] Vertices { get; set; }
            }
        }
    }

    public interface INameValidatable
    {
        string Name { get; set; }
    }

    public interface IHasSheet
    {
        GameObjectSheetSpecifierJSON Sheet { get; set; }
    }

    public interface IRequiresKey
    {
        string Key { get; set; }
    }
}