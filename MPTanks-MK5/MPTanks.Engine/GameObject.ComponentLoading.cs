using Microsoft.Xna.Framework;
using MPTanks.Engine.Assets;
using MPTanks.Engine.Core;
using MPTanks.Engine.Rendering;
using MPTanks.Engine.Rendering.Particles;
using MPTanks.Engine.Serialization;
using MPTanks.Modding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public partial class GameObject
    {
        #region Associated Properties

        protected Dictionary<string, RenderableComponent> _components;
        public IReadOnlyDictionary<string, RenderableComponent> Components
        {
            get
            {
                return _components;
            }
        }
        protected Dictionary<string, string> _assets;
        public IReadOnlyDictionary<string, string> Assets
        {
            get
            {
                return _assets;
            }
        }
        private List<Tuple<ParticleEngine.Emitter, GameObjectEmitterJSON>> _emittersWithData =
            new List<Tuple<ParticleEngine.Emitter, GameObjectEmitterJSON>>();
        protected Dictionary<string, ParticleEngine.Emitter> _emitters;
        public IReadOnlyDictionary<string, ParticleEngine.Emitter> Emitters
        {
            get
            {
                return _emitters;
            }
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

        /// <summary>
        /// Loads the components from the specified asset and adds them to the internal dictionary.
        /// </summary>
        /// <param name="assetName"></param>
        protected void LoadComponentsFromFile(string assetName)
        {
            Game.Logger.Trace("Loading Components: " + assetName);
            var deserialized = GameObjectComponentsJSON.Create(File.ReadAllText(assetName));

            Game.Logger.Trace("Begin load: " + deserialized.Name);

            if (deserialized.ReflectionName != ReflectionName)
                Game.Logger.Warning(
                    $"GameObject-{ObjectId}.LoadComponentsFromFile():" +
                    $"{deserialized.ReflectionName} does not match {ReflectionName}");

            DefaultSize = deserialized.DefaultSize;

            foreach (var cmp in deserialized.Components)
            {
                string sheet;
                if (cmp.Sheet.FromOtherMod)
                    sheet = ResolveAsset(cmp.Sheet.ModName, cmp.Sheet.File);
                else
                    sheet = ResolveAsset(cmp.Sheet.File);

                SpriteInfo asset = new SpriteInfo();
                if (cmp.Frame != null)
                {
                    if (cmp.Frame.StartsWith("[animation]"))
                        asset = new SpriteAnimationInfo(cmp.Frame.Substring("[animation]".Length), sheet);
                    else
                        asset = new SpriteInfo(cmp.Frame, sheet);
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
            foreach (var asset in deserialized.OtherAssets)
            {
                if (asset.FromOtherMod && !_assets.ContainsKey(asset.Key))
                    _assets.Add(asset.Key, ResolveAsset(asset.ModName, asset.File));
                else
                    _assets.Add(asset.Key, ResolveAsset(asset.File));
            }

            foreach (var cmp in deserialized.Components)
            {
                if (cmp.Sheet.Key != null && !_assets.ContainsKey(cmp.Sheet.Key))
                {
                    if (cmp.Sheet.FromOtherMod)
                        _assets.Add(cmp.Sheet.Key, ResolveAsset(cmp.Sheet.ModName, cmp.Sheet.File));
                    else
                        _assets.Add(cmp.Sheet.Key, ResolveAsset(cmp.Sheet.File));
                }
            }

            foreach (var emitter in deserialized.Emitters)
            {
                foreach (var sp in emitter.Sprites)
                {
                    if (sp.Sheet.Key != null && !_assets.ContainsKey(sp.Sheet.Key))
                    {
                        if (sp.Sheet.FromOtherMod)
                            _assets.Add(sp.Sheet.Key, ResolveAsset(sp.Sheet.ModName, sp.Sheet.File));
                        else
                            _assets.Add(sp.Sheet.Key, ResolveAsset(sp.Sheet.File));
                    }
                }
            }

            CreateEmitters(deserialized.Emitters);
        }

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

            LoadComponentsFromFile(ResolveAsset(componentFile));
            AddComponents(_components);
        }


        private void CreateEmitters(GameObjectEmitterJSON[] emitters)
        {
            foreach (var emitter in emitters)
            {
                var infos = new List<SpriteInfo>();
                foreach (var sprite in emitter.Sprites)
                {
                    if (sprite.Frame.StartsWith("[animation]"))
                    {
                        if (sprite.Sheet.FromOtherMod)
                            infos.Add(
                                new SpriteAnimationInfo(sprite.Frame.Substring("[animation]".Length),
                                ResolveAsset(sprite.Sheet.ModName, sprite.Sheet.File)));
                        else
                            infos.Add(
                                new SpriteAnimationInfo(sprite.Frame.Substring("[animation]".Length),
                                ResolveAsset(sprite.Sheet.File)));
                    }
                    else
                    {
                        if (sprite.Sheet.FromOtherMod)
                            infos.Add(new SpriteInfo(sprite.Frame,
                                ResolveAsset(sprite.Sheet.ModName, sprite.Sheet.File)));
                        else
                            infos.Add(new SpriteInfo(sprite.Frame, ResolveAsset(sprite.Sheet.File)));
                    }
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
                    emitter.ShrinkInsteadOfFade,
                    emitter.SizeScalingUniform,
                    emitter.RenderBelowObjects,
                    Vector2.Zero);

                em.Paused = true;

                _emitters.Add(emitter.Name, em);
                _emittersWithData.Add(Tuple.Create(em, emitter));
            }
        }

        private void UpdateEmitters(GameTime gameTime)
        {
            foreach (var em in _emittersWithData)
            {
                if (em.Item2.ColorChangedByObjectMask)
                {
                    em.Item1.MinColorMask = new Color(((Color)em.Item2.MinColorMask).ToVector4() * ColorMask.ToVector4());
                    em.Item1.MaxColorMask = new Color(((Color)em.Item2.MaxColorMask).ToVector4() * ColorMask.ToVector4());
                }
                if (em.Item2.EmissionArea.TracksObject)
                {
                    var emissionAreaNew = new RectangleF(
                        em.Item2.EmissionArea.X + Position.X,
                        em.Item2.EmissionArea.Y + Position.Y,
                        em.Item2.EmissionArea.W,
                        em.Item2.EmissionArea.H);

                    em.Item1.EmissionArea = emissionAreaNew;
                }
            }
        }

        private void DestroyEmitters()
        {
            foreach (var emitter in _emittersWithData)
                if (!emitter.Item2.KeepAliveAfterDeath)
                    emitter.Item1.Kill();
        }

        private void ActivateOnCreateEmitters()
        {
            foreach (var emitter in _emittersWithData)
                if (emitter.Item2.SpawnOnCreate)
                    emitter.Item1.Paused = false;
        }
        private void ActivateOnDestroyEmitters()
        {
            foreach (var emitter in _emittersWithData)
                if (emitter.Item2.SpawnOnDestroy)
                    emitter.Item1.Paused = false;
        }
        private void ActivateOnDestroyEndedEmitters()
        {
            foreach (var emitter in _emittersWithData)
                if (emitter.Item2.SpawnOnDestroyEnded)
                    emitter.Item1.Paused = false;
        }
        private void ActivateAtTimeEmitters()
        {
            foreach (var emitter in _emittersWithData)
                if (emitter.Item2.SpawnAtTime)
                    if (emitter.Item2.TimeMsToSpawnAt < TimeAliveMs)
                        emitter.Item1.Paused = false;
                    else emitter.Item1.Paused = true;
        }
    }
}
