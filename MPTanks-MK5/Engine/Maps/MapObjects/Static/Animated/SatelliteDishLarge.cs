using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Maps.MapObjects.Static.Animated
{
    public class SatelliteDishLarge : MapObject
    {
        public SatelliteDishLarge(GameCore game, bool authorized, Vector2 position = default(Vector2), float rotation = 0)
            : base(game, authorized, position, rotation)
        {
            AddComponents();
        }

        private void AddComponents()
        {
            Components.Add("base", new Rendering.RenderableComponent()
            {
                Size = new Vector2(4),
                AssetName = Assets.MapObjects.SatelliteDish.Base.SpriteName,
                SpriteSheetName = Assets.MapObjects.SatelliteDish.Base.SheetName
            });
            Components.Add("dish", new Rendering.RenderableComponent()
            {
                Size = new Vector2(6, 3.5f),
                RotationOrigin = new Vector2(3, 1f),
                Offset = new Vector2 (-1.5f, 0.75f),
                AssetName = Assets.MapObjects.SatelliteDish.DishAndRevolver.SpriteName,
                SpriteSheetName = Assets.MapObjects.SatelliteDish.DishAndRevolver.SheetName,
                DrawLayer = 5 //Dish should always be on top of tanks
            });
        }
        public override Microsoft.Xna.Framework.Vector2 Size
        {
            get { return new Vector2(4); }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            Components["dish"].Rotation += 0.05f * ((float)time.ElapsedGameTime.TotalMilliseconds / 16.66666f);
        }
    }
}
