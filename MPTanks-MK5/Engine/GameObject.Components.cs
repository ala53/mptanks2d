using Microsoft.Xna.Framework;
using MPTanks.Engine.Assets;
using MPTanks.Engine.Core;
using MPTanks.Engine.Rendering;
using MPTanks.Engine.Rendering.Animations;
using MPTanks.Engine.Rendering.Lighting;
using MPTanks.Engine.Rendering.Particles;
using MPTanks.Engine.Serialization;
using MPTanks.Engine.Settings;
using MPTanks.Modding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MPTanks.Engine
{
    public partial class GameObject
    {
        #region Associated Properties
        public GameObjectComponentsJSON BaseComponents { get; private set; }
        protected List<GameObjectComponentsJSON> _otherComponentFiles = new List<GameObjectComponentsJSON>();
        public IReadOnlyList<GameObjectComponentsJSON> OtherComponentFiles { get { return _otherComponentFiles; } }
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

        protected Dictionary<string, Animation> _animations;
        [JsonIgnore]
        public IReadOnlyDictionary<string, Animation> Animations { get { return _animations; } }

        private List<AnimationData> _animationsWithData = new List<AnimationData>();

        private struct AnimationData
        {
            public Animation Animation { get; set; }
            public GameObjectAnimationJSON Information { get; set; }
        }

        protected Dictionary<string, Sound.Sound> _sounds;
        public IReadOnlyDictionary<string, Sound.Sound> Sounds { get { return _sounds; } }

        private List<SoundData> _soundsWithData = new List<SoundData>();
        private struct SoundData
        {
            public Sound.Sound Sound { get; set; }
            public GameObjectSoundJSON Information { get; set; }
            public bool HasBeenTriggered { get; set; }
        }
        protected HashSet<string> _flags = new HashSet<string>();
        [JsonIgnore]
        public ISet<string> Flags { get { return _flags; } }

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
        protected GameObjectComponentsJSON LoadComponentsFromFile(string assetName)
        {
            if (GlobalSettings.Debug)
                Game.Logger.Trace("Loading Components: " + assetName);
            if (!_componentJSONCache.ContainsKey(assetName))
            {
                Stopwatch sw = null;
                if (GlobalSettings.Debug)
                    sw = Stopwatch.StartNew();

                _componentJSONCache.Add(assetName, GameObjectComponentsJSON.Create(File.ReadAllText(assetName)));

                if (GlobalSettings.Debug)
                {
                    Game.Logger.Trace($"Components from {assetName} were not cached." +
                        $"Took {sw.Elapsed.TotalMilliseconds.ToString("N1")}ms to parse");
                    sw.Stop();
                }
            }

            GameObjectComponentsJSON deserialized = _componentJSONCache[assetName];

            if (_otherComponentFiles.Contains(deserialized)) return null; //already loaded
            _otherComponentFiles.Add(deserialized);

            if (GlobalSettings.Debug)
                Game.Logger.Trace("Begin load: " + deserialized.Name);

            if (deserialized.ReflectionName != ReflectionName)
                Game.Logger.Warning(
                    $"GameObject-{ObjectId}.LoadComponentsFromFile():" +
                    $"{deserialized.ReflectionName} does not match {ReflectionName}");

            Lifespan = TimeSpan.FromMilliseconds(deserialized.Lifespan);
            PostDeathExistenceTime = deserialized.RemoveAfter;
            DefaultSize = deserialized.DefaultSize;
            DrawLayer = deserialized.DrawLayer;
            Health = deserialized.Health;
            CanBeDestroyed = !deserialized.Invincible;
            foreach (var flag in deserialized.Flags)
                _flags.Add(flag);

            LoadComponents(deserialized.Components);
            LoadComponentGroups(deserialized.ComponentGroups);
            LoadKeyedAssets(deserialized.OtherAssets, deserialized.Components, deserialized.Emitters,
                deserialized.Animations, deserialized.Lights);
            LoadAnimations(deserialized.Animations);
            LoadOtherSprites(deserialized.OtherSprites);
            LoadEmitters(deserialized.Emitters);
            LoadSounds(deserialized.Sounds);


            return deserialized;
        }
        #region Components
        private void LoadComponents(GameObjectComponentJSON[] components)
        {
            foreach (var cmp in components)
            {
                SpriteInfo asset = new SpriteInfo();
                if (cmp.Image != null && cmp.Image.Frame != null)
                {
                    if (cmp.Image.IsAnimation)
                        asset = new SpriteInfo(cmp.Image.Frame,
                            ResolveJSONSheet(cmp.Image.Sheet), true, int.MaxValue);
                    else
                        asset = new SpriteInfo(cmp.Image.Frame, ResolveJSONSheet(cmp.Image.Sheet));
                }
                var component = new RenderableComponent(this)
                {
                    DrawLayer = cmp.DrawLayer,
                    DefaultSprite = asset,
                    Mask = (cmp.Mask == null) ? Color.White : (Color)cmp.Mask,
                    Offset = cmp.Offset,
                    Rotation = cmp.Rotation,
                    RotationVelocity = cmp.RotationVelocity,
                    RotationOrigin = cmp.RotationOrigin,
                    Scale = cmp.Scale,
                    Size = cmp.Size,
                    Visible = cmp.Visible,
                    IgnoresObjectMask = cmp.IgnoresObjectMask
                };
                if (cmp.Image != null && cmp.Image.DamageLevels != null && cmp.Image.DamageLevels.Length > 0)
                {
                    component.DamageLevels =
                        new RenderableComponent.RenderableComponentDamageLevel[cmp.Image.DamageLevels.Length];
                    for (var i = 0; i < component.DamageLevels.Length; i++)
                    {
                        var dmgLevel = cmp.Image.DamageLevels[i];
                        SpriteInfo dmgAsset;
                        if (dmgLevel.Sprite.IsAnimation)
                            dmgAsset = new SpriteInfo(dmgLevel.Sprite.Frame,
                                ResolveJSONSheet(dmgLevel.Sprite.Sheet), true, int.MaxValue);
                        else
                            dmgAsset = new SpriteInfo(dmgLevel.Sprite.Frame, ResolveJSONSheet(dmgLevel.Sprite.Sheet));
                        component.DamageLevels[i] = new RenderableComponent.RenderableComponentDamageLevel()
                        {
                            Info = dmgAsset,
                            MinHealth = dmgLevel.MinHealth,
                            MaxHealth = dmgLevel.MaxHealth
                        };
                    }
                }
                _components.Add(cmp.Name, component);
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
        #endregion
        #region Sounds
        private void LoadSounds(GameObjectSoundJSON[] sounds)
        {
            foreach (var sound in sounds)
            {
                string asset;
                if (sound.FromOtherMod)
                    asset = ResolveAsset(sound.AssetModName, sound.Asset);
                else asset = ResolveAsset(sound.Asset);
                var snd = new Sound.Sound(Game.SoundEngine, asset);
                snd.LoopCount = sound.LoopCount;
                snd.Pitch = sound.Pitch;
                snd.Positional = sound.Positional;
                snd.Position = TransformPoint(sound.Position);
                snd.Time = TimeSpan.FromMilliseconds(sound.OffsetMs);
                snd.Timescale = sound.Timescale;
                snd.Velocity = TransformPoint(sound.Velocity);
                snd.Volume = sound.Volume;
                _sounds.Add(sound.Name, snd);
                _soundsWithData.Add(new SoundData()
                {
                    Information = sound,
                    Sound = snd
                });
            }
        }
        private void TriggerSounds(string triggerName)
        {
            for (var i = 0; i < _soundsWithData.Count; i++)
            {
                var snd = _soundsWithData[i];
                if (snd.HasBeenTriggered) continue;
                if (snd.Information.TriggerName.Equals(triggerName, StringComparison.OrdinalIgnoreCase))
                {
                    snd.HasBeenTriggered = true;
                    Game.SoundEngine.AddSound(snd.Sound);
                    snd.Sound.Position = TransformPoint(snd.Information.Position);
                    if (snd.Information.TracksObject)
                        snd.Sound.Velocity = TransformPoint(snd.Information.Velocity);
                }
                _soundsWithData[i] = snd;
            }
        }

        private void TriggerAtTimeSounds()
        {
            for (var i = 0; i < _soundsWithData.Count; i++)
            {
                var snd = _soundsWithData[i];
                if (snd.HasBeenTriggered) continue;
                if (snd.Information.TimeToSpawnAt < TimeAlive)
                {
                    snd.HasBeenTriggered = true;
                    Game.SoundEngine.AddSound(snd.Sound);
                    snd.Sound.Position = TransformPoint(snd.Information.Position);
                    if (snd.Information.TracksObject)
                        snd.Sound.Velocity = TransformPoint(snd.Information.Velocity);
                }
                _soundsWithData[i] = snd;
            }
        }

        private void UpdateSounds(GameTime gameTime)
        {
            foreach (var snd in _soundsWithData)
            {
                if (snd.Information.TracksObject)
                {
                    snd.Sound.Position = TransformPoint(snd.Information.Position);
                    snd.Sound.Velocity = TransformPoint(snd.Information.Velocity);
                }
            }
        }
        #endregion

        private void LoadKeyedAssets(GameObjectSheetSpecifierJSON[] assets, GameObjectComponentJSON[] components,
            GameObjectEmitterJSON[] emitters, GameObjectAnimationJSON[] anims, GameObjectLightJSON[] lights)
        {
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
                if (light.Image != null && light.Image.Sheet != null &&
                    light.Image.Sheet.Key != null && !_assets.ContainsKey(light.Image.Sheet.Key))
                    _assets.Add(light.Image.Sheet.Key, ResolveJSONSheet(light.Image.Sheet));
        }

        private void LoadOtherSprites(GameObjectSpriteSpecifierJSON[] sprites)
        {
            foreach (var sprite in sprites)
            {
                if (sprite.IsAnimation)
                {
                    _sprites.Add(sprite.Key,
                        new SpriteInfo(sprite.Frame, ResolveJSONSheet(sprite.Sheet), true, int.MaxValue));
                }
                else
                {
                    _sprites.Add(sprite.Key,
                        new SpriteInfo(sprite.Frame, ResolveJSONSheet(sprite.Sheet)));
                }
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
            _animations = new Dictionary<string, Animation>(StringComparer.InvariantCultureIgnoreCase);
            _lights = new Dictionary<string, Light>();
            _sounds = new Dictionary<string, Sound.Sound>();

            BaseComponents = LoadComponentsFromFile(ResolveAsset(componentFile));

            AddComponents(_components);
        }

        #region Emitters
        private void LoadEmitters(GameObjectEmitterJSON[] emitters)
        {
            foreach (var emitter in emitters)
            {
                var infos = new List<SpriteInfo>();
                foreach (var sprite in emitter.Sprites)
                {
                    if (sprite.Frame != null)
                    {
                        if (sprite.IsAnimation)
                            infos.Add(
                                new SpriteInfo(sprite.Frame,
                                ResolveJSONSheet(sprite.Sheet), true, int.MaxValue));
                        else
                            infos.Add(new SpriteInfo(sprite.Frame,
                                ResolveJSONSheet(sprite.Sheet)));
                    }
                    else
                    {
                        infos.Add(new SpriteInfo(null, null));
                    }
                }

                var emissionAreaTransformed = TransformPoint(
                    new Vector2(emitter.EmissionArea.X, emitter.EmissionArea.Y));
                var em = Game.ParticleEngine.CreateEmitter(infos.ToArray(),
                    emitter.MinFadeInTime, emitter.MaxFadeInTime,
                    emitter.MinFadeOutTime, emitter.MaxFadeOutTime,
                    emitter.MinLifeSpan, emitter.MaxFadeOutTime,
                    new RectangleF(
                    emissionAreaTransformed.X, emissionAreaTransformed.Y,
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
                    var newPt = TransformPoint(
                        new Vector2(em.Information.EmissionArea.X, em.Information.EmissionArea.Y));
                    var emissionAreaNew = new RectangleF(
                        newPt.X,
                        newPt.Y,
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
            {
                if (emitter.Information.ActivationIsTriggered &&
                    emitter.Information.TriggerName.Equals(trigger, StringComparison.InvariantCultureIgnoreCase))
                {
                    var pos = TransformPoint(
                        new Vector2(emitter.Information.EmissionArea.X, emitter.Information.EmissionArea.Y));
                    emitter.AttachedEmitter.Paused = false;
                    emitter.AttachedEmitter.EmissionArea = new RectangleF(
                       pos.X, pos.Y,
                       emitter.AttachedEmitter.EmissionArea.Width,
                       emitter.AttachedEmitter.EmissionArea.Height);
                }
            }
        }
        private void ActivateAtTimeEmitters()
        {
            foreach (var emitter in _emittersWithData)
                if (emitter.Information.ActivatesAtTime)
                    if (emitter.Information.TimeToSpawnAt < TimeAlive)
                        emitter.AttachedEmitter.Paused = false;
                    else emitter.AttachedEmitter.Paused = true;
        }

        #endregion

        #region Animations
        private void LoadAnimations(GameObjectAnimationJSON[] animations)
        {
            foreach (var anim in animations)
            {
                var sprite = anim.SpriteOptions[Game.SharedRandom.Next(0, anim.SpriteOptions.Length)];
                var animation = new Animation(
                   new SpriteInfo(sprite.Frame,
                    ResolveJSONSheet(sprite.Sheet),
                    true, anim.LoopCount),
                    TransformPoint(anim.Position),
                    anim.Size,
                    anim.Mask ?? Color.White,
                    anim.RotationOrigin ?? ((Vector2)anim.Size) / 2,
                    null,
                    anim.DrawLayer
                    );

                _animations.Add(anim.Name, animation);
                _animationsWithData.Add(new AnimationData
                {
                    Animation = animation,
                    Information = anim
                });
            }
        }
        private void UpdateAnimations(GameTime gameTime)
        {
            foreach (var anim in _animationsWithData)
            {
                if (anim.Information.ActivatesAtTime && anim.Information.TimeToSpawnAt < TimeAlive &&
                    !Game.AnimationEngine.Animations.Contains(anim.Animation))
                {
                    anim.Animation.Position = TransformPoint(anim.Information.Position);
                    anim.Animation.Size = anim.Information.Size * Scale;
                    Game.AnimationEngine.AddAnimation(anim.Animation);
                }
                if (anim.Information.TracksObject)
                {
                    anim.Animation.Position = TransformPoint(anim.Information.Position);
                    anim.Animation.Size = anim.Information.Size * Scale;
                }
            }

        }

        private void TriggerAnimations(string triggerName)
        {
            foreach (var anim in _animationsWithData)
                if (anim.Information.ActivationIsTriggered &&
                    anim.Information.TriggerName.Equals(triggerName, StringComparison.InvariantCultureIgnoreCase))
                {
                    anim.Animation.Position = TransformPoint(anim.Information.Position);
                    anim.Animation.Size = anim.Information.Size * Scale;
                    Game.AnimationEngine.AddAnimation(anim.Animation);
                }
        }
        #endregion

        #region Lights
        private void LoadLights(GameObjectLightJSON[] lights)
        {
            foreach (var light in lights)
            {
                var l = new Light()
                {
                    SpriteInfo = new SpriteInfo(light?.Image?.Frame, ResolveJSONSheet(light?.Image?.Sheet)),
                    Color = light.Color,
                    //Intensity is nothing when not activated yet
                    Intensity = (light.ActivatesAtTime || light.ActivationIsTriggered) ? 0 : light.Intensity,
                    Position = light.Position,
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

        private void UpdateLights(GameTime gameTime)
        {
            foreach (var light in _lightsWithData)
                if (light.Information.ActivatesAtTime &&
                    light.Information.TimeToSpawnAt < TimeAlive &&
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
            TriggerSounds(triggerName);
        }

        private void UpdateComponents(GameTime gameTime)
        {
            UpdateLights(gameTime);
            UpdateAnimations(gameTime);
            UpdateEmitters(gameTime);
            UpdateRenderableComponentRotations(gameTime);
            UpdateSounds(gameTime);
        }
        #endregion
    }
}
