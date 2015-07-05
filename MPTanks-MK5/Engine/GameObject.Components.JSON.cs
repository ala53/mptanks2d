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
        public GameObjectComponentGroupJSON[] ComponentGroups { get; set; }
        public GameObjectComponentJSON[] Components { get; set; }
        public GameObjectSheetSpecifierJSON[] OtherAssets { get; set; }
        public GameObjectEmitterJSON[] Emitters { get; set; }
        public GameObjectSpriteSpecifierJSON[] OtherSprites { get; set; }
        public GameObjectLightJSON[] Lights { get; set; }
        public GameObjectAnimationJSON[] Animations { get; set; }

        public static GameObjectComponentsJSON Create(string data)
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
            if (me.Components == null) me.Components = new GameObjectComponentJSON[0];
            if (me.OtherAssets == null) me.OtherAssets = new GameObjectSheetSpecifierJSON[0];
            if (me.Emitters == null) me.Emitters = new GameObjectEmitterJSON[0];
            if (me.OtherSprites == null) me.OtherSprites = new GameObjectSpriteSpecifierJSON[0];
            if (me.ComponentGroups == null) me.ComponentGroups = new GameObjectComponentGroupJSON[0];
            if (me.Animations == null) me.Animations = new GameObjectAnimationJSON[0];
            if (me.Lights == null) me.Lights = new GameObjectLightJSON[0];

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

                sp.IsAnimation = sp.IsAnimation || sp.Frame.StartsWith("[animation]");
                if (sp.Frame.StartsWith("[animation]"))
                    sp.Frame = sp.Frame.Substring("[animation]".Length);

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

                cmp.Frame = ResolveSpriteReference(cmp.Frame, cmp);

                HandleSheet(cmp);
            }
        }

        private void ProcessEmitters()
        {
            foreach (var emitter in Emitters)
            {
                //Handle namelessness
                Validate(emitter);
                //Deal with null refs in the sub-sprites
                foreach (var sprite in emitter.Sprites)
                {
                    HandleSheet(sprite);
                    sprite.Frame = ResolveSpriteReference(sprite.Frame, sprite);
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

                foreach (var cmp in anim.SpriteOptions)
                {
                    HandleSheet(cmp);
                    cmp.Frame = ResolveSpriteReference(cmp.Frame, cmp);
                }
            }
        }

        private void ProcessLights()
        {
            foreach (var light in Lights)
            {
                Validate(light);
                HandleSheet(light);
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
                float parse = 0;
                if (float.TryParse(obj.ActivatesOn.Substring(2), out parse))
                {
                    obj.ActivatesAtTime = true;
                    obj.TimeMsToSpawnAt = parse;
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
            foreach (var anim in Animations)
                foreach (var sp in anim.SpriteOptions)
                    if (sp.Sheet != null && sp.Sheet.Key != null &&
                        sp.Sheet.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        if (sp.Sheet.Reference == null)
                            return sp.Sheet;
                        else return FindSheet(sp.Sheet.Reference);
            foreach (var light in Lights)
                if (light.Sheet != null && light.Sheet.Key != null &&
                    light.Sheet.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    if (light.Sheet.Reference == null)
                        return light.Sheet;
                    else return FindSheet(light.Sheet.Reference);

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

    class GameObjectComponentGroupJSON : IRequiresKey
    {
        public string Key { get; set; }
        [JsonProperty("components")]
        public string[] ComponentStrings { get; set; }
        [JsonIgnore]
        public GameObjectComponentJSON[] Components { get; set; }
    }

    class GameObjectComponentJSON : INameValidatable, IHasSheet
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

    class GameObjectSpriteSpecifierJSON : IHasSheet, IRequiresKey
    {
        public string Frame { get; set; }
        public string Key { get; set; }
        public bool IsAnimation { get; set; }
        public GameObjectSheetSpecifierJSON Sheet { get; set; }
    }

    class GameObjectEmitterJSON : ITriggerable, INameValidatable
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
        public class SpriteJSON : IHasSheet
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

    class GameObjectLightJSON : ITriggerable, INameValidatable, IHasSheet
    {
        public string Name { get; set; }
        public string Frame { get; set; }
        public float Intensity { get; set; }
        public JSONColor Color { get; set; }
        public JSONVector Position { get; set; }
        public bool TracksObject { get; set; }
        public bool TracksComponent { get; set; }
        [JsonIgnore]
        public GameObjectComponentJSON Component { get; set; }
        public string ComponentToTrack { get; set; }
        public JSONVector Size { get; set; }
        public GameObjectSheetSpecifierJSON Sheet { get; set; }
        public string ActivatesOn { get; set; }
        public bool ActivatesAtTime { get; set; }
        public float TimeMsToSpawnAt { get; set; }
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

    class GameObjectAnimationJSON : ITriggerable, INameValidatable
    {
        public string ActivatesOn { get; set; }
        public bool ActivatesAtTime { get; set; }
        public float TimeMsToSpawnAt { get; set; }
        public bool ActivationIsTriggered { get; set; }
        public string TriggerName { get; set; }
        public GameObjectSpriteSpecifierJSON[] SpriteOptions { get; set; }
        public int LoopCount { get; set; }
        public string Name { get; set; }
        public JSONVector Position { get; set; }
        public bool TracksObject { get; set; }
        public float Rotation { get; set; }
        public JSONVector Size { get; set; }
        public float StartPositionMs { get; set; }
    }

    interface ITriggerable
    {
        string ActivatesOn { get; set; }
        [JsonIgnore]
        bool ActivatesAtTime { get; set; }
        [JsonIgnore]
        bool ActivationIsTriggered { get; set; }
        [JsonIgnore]
        float TimeMsToSpawnAt { get; set; }
        [JsonIgnore]
        string TriggerName { get; set; }
    }

    interface INameValidatable
    {
        string Name { get; set; }
    }

    interface IHasSheet
    {
        GameObjectSheetSpecifierJSON Sheet { get; set; }
    }

    interface IRequiresKey
    {
        string Key { get; set; }
    }
}