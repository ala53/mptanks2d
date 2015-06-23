﻿using Microsoft.Xna.Framework;
using MPTanks.Engine.Assets;
using MPTanks.Engine.Core;
using MPTanks.Engine.Rendering;
using MPTanks.Engine.Rendering.Animations;
using MPTanks.Engine.Rendering.Lighting;
using MPTanks.Engine.Rendering.Particles;
using MPTanks.Engine.Serialization;
using MPTanks.Modding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MPTanks.Engine
{
    public partial class GameObject
    {
        #region Associated Properties
        #region Components
        protected Dictionary<string, RenderableComponent> _components;
        [JsonIgnore]
        public IReadOnlyDictionary<string, RenderableComponent> Components { get { return _components; } }
        protected Dictionary<string, RenderableComponentGroup> _groups;
        [JsonIgnore]
        public IReadOnlyDictionary<string, RenderableComponentGroup> ComponentGroups { get { return _groups; } }
        #endregion
        #region Emitters
        private List<EmitterData> _emittersWithData =
            new List<EmitterData>();

        private struct EmitterData
        {
            public ParticleEngine.Emitter AttachedEmitter { get; set; }
            public GameObjectEmitterJSON Information { get; set; }
        }
        protected Dictionary<string, ParticleEngine.Emitter> _emitters;
        [JsonIgnore]
        public IReadOnlyDictionary<string, ParticleEngine.Emitter> Emitters { get { return _emitters; } }
        #endregion

        #region Assets
        protected Dictionary<string, string> _assets;
        [JsonIgnore]
        public IReadOnlyDictionary<string, string> Assets { get { return _assets; } }
        protected Dictionary<string, SpriteInfo> _sprites;
        [JsonIgnore]
        public IReadOnlyDictionary<string, SpriteInfo> Sprites { get { return _sprites; } }
        #endregion
        protected Dictionary<string, SpriteAnimationInfo> _animatedSprites;
        [JsonIgnore]
        public IReadOnlyDictionary<string, SpriteAnimationInfo> AnimatedSprites { get { return _animatedSprites; } }

        protected Dictionary<string, Animation> _animations;
        [JsonIgnore]
        public IReadOnlyDictionary<string, Animation> Animations { get { return _animations; } }

        private List<AnimationData> _animationsWithData = new List<AnimationData>();

        private struct AnimationData
        {
            public Animation Animation { get; set; }
            public GameObjectAnimationJSON Information { get; set; }
        }

        protected Dictionary<string, Light> _lights;
        [JsonIgnore]
        public IReadOnlyDictionary<string, Light> Lights { get { return _lights; } }

        private List<LightData> _lightsWithData = new List<LightData>();

        private struct LightData
        {
            public Light Light { get; set; }
            public GameObjectLightJSON Information { get; set; }
        }
        #endregion

        /// <summary>
        /// Called when the object is supposed to add the rendering components. Usually 
        /// on the first access.
        /// Please note: the components are already loaded from the components file beforehand,
        /// so you only need this if you're programatically generating extra ones.
        /// NOTE: DO NOT USE THIS UNLESS YOU NEED TO. 
        /// </summary>
        protected virtual void AddComponents(Dictionary<string, RenderableComponent> components)
        {
        }


        private static Dictionary<string, GameObjectComponentsJSON> _componentJSONCache =
            new Dictionary<string, GameObjectComponentsJSON>();

        #region Loading
        /// <summary>
        /// Loads the components from the specified asset and adds them to the internal dictionary.
        /// </summary>
        /// <param name="assetName"></param>
        protected void LoadComponentsFromFile(string assetName)
        {
            Game.Logger.Trace("Loading Components: " + assetName);
            if (!_componentJSONCache.ContainsKey(assetName))
            {
                _componentJSONCache.Add(assetName, GameObjectComponentsJSON.Create(File.ReadAllText(assetName)));
            }
            GameObjectComponentsJSON deserialized = _componentJSONCache[assetName];

            Game.Logger.Trace("Begin load: " + deserialized.Name);

            if (deserialized.ReflectionName != ReflectionName)
                Game.Logger.Warning(
                    $"GameObject-{ObjectId}.LoadComponentsFromFile():" +
                    $"{deserialized.ReflectionName} does not match {ReflectionName}");

            LifespanMs = deserialized.Lifespan;
            PostDeathExistenceTime = deserialized.RemoveAfter;
            DefaultSize = deserialized.DefaultSize;

            LoadComponents(deserialized.Components);
            LoadComponentGroups(deserialized.ComponentGroups);
            LoadKeyedAssets(deserialized.OtherAssets, deserialized.Components, deserialized.Emitters,
                deserialized.Animations, deserialized.Lights);
            LoadOtherSprites(deserialized.OtherSprites);
            LoadEmitters(deserialized.Emitters);
        }

        private void LoadComponents(GameObjectComponentJSON[] components)
        {
            foreach (var cmp in components)
            {
                SpriteInfo asset = new SpriteInfo();
                if (cmp.Frame != null)
                {
                    if (cmp.Frame.StartsWith("[animation]"))
                        asset = new SpriteAnimationInfo(cmp.Frame.Substring("[animation]".Length),
                            ResolveJSONSheet(cmp.Sheet));
                    else
                        asset = new SpriteInfo(cmp.Frame, ResolveJSONSheet(cmp.Sheet));
                }
                _components.Add(cmp.Name, new RenderableComponent
                {
                    DrawLayer = cmp.DrawLayer,
                    FrameName = asset.FrameName,
                    Mask = (cmp.Mask == null) ? Color.White : (Color)cmp.Mask,
                    Offset = cmp.Offset,
                    Rotation = cmp.Rotation,
                    RotationVelocity = cmp.RotationVelocity,
                    RotationOrigin = cmp.RotationOrigin,
                    Scale = cmp.Scale,
                    SheetName = asset.SheetName,
                    Size = cmp.Size,
                    Visible = cmp.Visible
                });
            }
        }

        private void LoadComponentGroups(GameObjectComponentGroupJSON[] groups)
        {
            foreach (var group in groups)
            {
                var rGroup = new RenderableComponentGroup();
                rGroup.Components = new RenderableComponent[group.ComponentStrings.Length];
                for (var i = 0; i < rGroup.Components.Length; i++)
                    rGroup.Components[i] = Components[group.ComponentStrings[i]];
                _groups.Add(group.Key, rGroup);
            }
        }

        private void LoadKeyedAssets(GameObjectSheetSpecifierJSON[] assets, GameObjectComponentJSON[] components,
            GameObjectEmitterJSON[] emitters, GameObjectAnimationJSON[] anims, GameObjectLightJSON[] lights)
        {
            foreach (var cmp in components)
                if (cmp.Sheet.Key != null && !_assets.ContainsKey(cmp.Sheet.Key))
                    _assets.Add(cmp.Sheet.Key, ResolveJSONSheet(cmp.Sheet));

            foreach (var emitter in emitters)
                foreach (var sp in emitter.Sprites)
                    if (sp.Sheet.Key != null && !_assets.ContainsKey(sp.Sheet.Key))
                        _assets.Add(sp.Sheet.Key, ResolveJSONSheet(sp.Sheet));

            foreach (var asset in assets)
                if (asset.Key != null && !_assets.ContainsKey(asset.Key))
                    _assets.Add(asset.Key, ResolveJSONSheet(asset));

            foreach (var anim in anims)
                foreach (var sp in anim.SpriteOptions)
                    if (sp.Sheet.Key != null && !_assets.ContainsKey(sp.Sheet.Key))
                        _assets.Add(sp.Sheet.Key, ResolveJSONSheet(sp.Sheet));

            foreach (var light in lights)
                if (light.Sheet.Key != null && !_assets.ContainsKey(light.Sheet.Key))
                    _assets.Add(light.Sheet.Key, ResolveJSONSheet(light.Sheet));
        }

        private void LoadOtherSprites(GameObjectSpriteSpecifierJSON[] sprites)
        {
            foreach (var sprite in sprites)
            {
                if (sprite.IsAnimation)
                {
                    _animatedSprites.Add(sprite.Key,
                        new SpriteAnimationInfo(sprite.Frame, ResolveJSONSheet(sprite.Sheet)));
                    _sprites.Add(sprite.Key,
                        new SpriteAnimationInfo(sprite.Frame, ResolveJSONSheet(sprite.Sheet)));
                }
                else
                {
                    _sprites.Add(sprite.Key,
                        new SpriteInfo(sprite.Frame, ResolveJSONSheet(sprite.Sheet)));
                }
            }
        }

        private void LoadEmitters(GameObjectEmitterJSON[] emitters)
        {
            foreach (var emitter in emitters)
            {
                var infos = new List<SpriteInfo>();
                foreach (var sprite in emitter.Sprites)
                {
                    if (sprite.Frame.StartsWith("[animation]"))
                        infos.Add(
                            new SpriteAnimationInfo(sprite.Frame.Substring("[animation]".Length),
                            ResolveJSONSheet(sprite.Sheet)));
                    else
                        infos.Add(new SpriteInfo(sprite.Frame,
                            ResolveJSONSheet(sprite.Sheet)));
                }

                var em = Game.ParticleEngine.CreateEmitter(infos.ToArray(),
                    emitter.MinFadeInTime, emitter.MaxFadeInTime,
                    emitter.MinFadeOutTime, emitter.MaxFadeOutTime,
                    emitter.MinLifeSpan, emitter.MaxFadeOutTime,
                    new RectangleF(
                    emitter.EmissionArea.X, emitter.EmissionArea.Y,
                    emitter.EmissionArea.W, emitter.EmissionArea.H),
                    emitter.MinVelocity, emitter.MaxVelocity,
                    emitter.MinAcceleration, emitter.MaxAcceleration,
                    emitter.VelocityAndAccelerationTrackRotation,
                    emitter.MinSize, emitter.MaxSize,
                    emitter.MinColorMask, emitter.MaxColorMask,
                    emitter.MinRotation, emitter.MaxRotation,
                    emitter.MinRotationVelocity, emitter.MaxRotationVelocity,
                    emitter.MinParticlesPerSecond, emitter.MaxParticlesPerSecond,
                    emitter.Lifespan == 0 ? float.PositiveInfinity : emitter.Lifespan,
                    emitter.GrowInsteadOfFadeIn,
                    emitter.ShrinkInsteadOfFadeOut,
                    emitter.SizeScalingUniform,
                    emitter.RenderBelowObjects,
                    Vector2.Zero);

                em.Paused = true;

                _emitters.Add(emitter.Name, em);
                _emittersWithData.Add(new EmitterData { AttachedEmitter = em, Information = emitter });
            }
        }

        private void LoadAnimations(GameObjectAnimationJSON[] animations)
        {
            foreach (var anim in animations)
            {
                var sprite = anim.SpriteOptions[Game.SharedRandom.Next(0, anim.SpriteOptions.Length)];
                var animation = new Animation(
                    sprite.Frame,
                    anim.Position,
                    anim.Size,
                    ResolveJSONSheet(sprite.Sheet),
                    null,
                    anim.LoopCount
                    );

                _animations.Add(anim.Name, animation);
                _animationsWithData.Add(new AnimationData
                {
                    Animation = animation,
                    Information = anim
                });
            }
        }

        private void LoadLights(GameObjectLightJSON[] lights)
        {
            foreach (var light in lights)
            {
                var l = new Light()
                {
                    AssetName = light.Frame,
                    Color = light.Color,
                    //Intensity is nothing when not activated yet
                    Intensity = (light.ActivatesAtTime || light.ActivationIsTriggered) ? 0 : light.Intensity,
                    PositionCenter = light.Position,
                    SpriteName = ResolveJSONSheet(light.Sheet),
                    Size = light.Size
                };

                if (light.ShowForAllTeams)
                    l.ShowForAllTeams = true;
                else if (light.ShowForTankTeamOnly && GetType().IsSubclassOf(typeof(Tanks.Tank)))
                    l.TeamIds = new[] { ((Tanks.Tank)this).Team.TeamId };
                else if (light.TeamIds != null)
                    l.TeamIds = light.TeamIds;

                _lights.Add(light.Name, l);
                _lightsWithData.Add(new LightData
                {
                    Light = l,
                    Information = light
                });
            }
        }

        private string ResolveJSONSheet(GameObjectSheetSpecifierJSON sheet)
        {
            if (sheet.FromOtherMod)
                return ResolveAsset(sheet.ModName, sheet.File);
            else return ResolveAsset(sheet.File);
        }

        #endregion

        /// <summary>
        /// Loads the components specified in the file specified in the attribute for this object's type,
        /// as well as requesting that the user provide theirs
        /// </summary>
        private void LoadBaseComponents()
        {
            var attrib = ((GameObjectAttribute[])(GetType()
                  .GetCustomAttributes(typeof(GameObjectAttribute), true)))[0];
            var componentFile = attrib.ComponentFile;

            _emitters = new Dictionary<string, ParticleEngine.Emitter>(StringComparer.InvariantCultureIgnoreCase);
            _assets = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            _components = new Dictionary<string, RenderableComponent>(StringComparer.InvariantCultureIgnoreCase);
            _groups = new Dictionary<string, RenderableComponentGroup>(StringComparer.InvariantCultureIgnoreCase);
            _sprites = new Dictionary<string, SpriteInfo>(StringComparer.InvariantCultureIgnoreCase);
            _animatedSprites = new Dictionary<string, SpriteAnimationInfo>(StringComparer.InvariantCultureIgnoreCase);
            _animations = new Dictionary<string, Animation>(StringComparer.InvariantCultureIgnoreCase);
            _lights = new Dictionary<string, Light>();

            LoadComponentsFromFile(ResolveAsset(componentFile));
            AddComponents(_components);
        }

        #region Emitters

        private void UpdateEmitters(GameTime gameTime)
        {
            foreach (var em in _emittersWithData)
            {
                if (em.Information.ColorChangedByObjectMask)
                {
                    em.AttachedEmitter.MinColorMask = new Color(((Color)em.Information.MinColorMask).ToVector4() * ColorMask.ToVector4());
                    em.AttachedEmitter.MaxColorMask = new Color(((Color)em.Information.MaxColorMask).ToVector4() * ColorMask.ToVector4());
                }
                if (em.Information.EmissionArea.TracksObject)
                {
                    var emissionAreaNew = new RectangleF(
                        em.Information.EmissionArea.X + Position.X,
                        em.Information.EmissionArea.Y + Position.Y,
                        em.Information.EmissionArea.W,
                        em.Information.EmissionArea.H);

                    em.AttachedEmitter.EmissionArea = emissionAreaNew;
                }
                if (em.Information.VelocityRelativeToObject)
                    em.AttachedEmitter.EmitterVelocity = LinearVelocity;
            }
        }

        private void DestroyEmitters()
        {
            foreach (var emitter in _emittersWithData)
                if (!emitter.Information.KeepAliveAfterDeath)
                    emitter.AttachedEmitter.Kill();
        }

        private void TriggerEmitters(string trigger)
        {
            foreach (var emitter in _emittersWithData)
                if (emitter.Information.ActivationIsTriggered &&
                    emitter.Information.TriggerName.Equals(trigger, StringComparison.InvariantCultureIgnoreCase))
                    emitter.AttachedEmitter.Paused = false;
        }
        private void ActivateAtTimeEmitters()
        {
            foreach (var emitter in _emittersWithData)
                if (emitter.Information.ActivatesAtTime)
                    if (emitter.Information.TimeMsToSpawnAt < TimeAliveMs)
                        emitter.AttachedEmitter.Paused = false;
                    else emitter.AttachedEmitter.Paused = true;
        }

        #endregion

        #region Animations
        private void UpdateAnimations(GameTime gameTime)
        {
            foreach (var anim in _animationsWithData)
                if (anim.Information.ActivatesAtTime && anim.Information.TimeMsToSpawnAt < TimeAliveMs &&
                    !Game.AnimationEngine.Animations.Contains(anim.Animation))
                    Game.AnimationEngine.AddAnimation(anim.Animation);

        }

        private void TriggerAnimations(string triggerName)
        {
            foreach (var anim in _animationsWithData)
                if (anim.Information.ActivationIsTriggered &&
                    anim.Information.TriggerName.Equals(triggerName, StringComparison.InvariantCultureIgnoreCase))
                    Game.AnimationEngine.AddAnimation(anim.Animation);
        }
        #endregion

        #region Lights
        private void UpdateLights(GameTime gameTime)
        {
            foreach (var light in _lightsWithData)
                if (light.Information.ActivatesAtTime &&
                    light.Information.TimeMsToSpawnAt < TimeAliveMs &&
                    light.Light.Intensity == 0)
                    light.Light.Intensity = light.Information.Intensity;
        }

        private void TriggerLights(string triggerName)
        {
            foreach (var light in _lightsWithData)
                if (light.Information.ActivationIsTriggered &&
                    light.Information.TriggerName.Equals(triggerName, StringComparison.InvariantCultureIgnoreCase))
                    light.Light.Intensity = light.Information.Intensity;
        }
        #endregion

        #region Triggers
        protected void InvokeTrigger(string triggerName)
        {
            TriggerEmitters(triggerName);
            TriggerAnimations(triggerName);
            TriggerLights(triggerName);
            //Look for a function with the name and signature of "void On[TriggerName]() {}"
            try
            {
                var method = GetType().GetMethod("on" + triggerName,
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.IgnoreCase);

                if (method != null && method.ReturnType == typeof(void))
                    method.Invoke(this, null);
            }
            catch (Exception ex)
            {
                Game.Logger.Error($"GameObject (ID {ObjectId}): Tried to call \"On{triggerName}\" as triggered" +
                    " but the call failed (method does exist).", ex);
            }
        }
        #endregion

        #region Component Groups 
        #endregion
    }
}
