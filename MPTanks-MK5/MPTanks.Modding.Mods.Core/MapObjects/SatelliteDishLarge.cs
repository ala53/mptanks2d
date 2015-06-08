using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Engine.Maps.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding.Mods.Core.MapObjects
{
    [Modding.MapObject("SatelliteDishLarge", "satellitedish.json", IsStatic = true, MinHeightBlocks = 2, MinWidthBlocks = 2, 
        DisplayName = "Satellite Dish (large)")]
    public class SatelliteDishLarge : MapObject
    {
        public SatelliteDishLarge(GameCore game, bool authorized, Vector2 position = default(Vector2), float rotation = 0)
            : base(game, authorized, position, rotation)
        {
            Size = new Vector2(4);
        }

        protected override void AddComponents()
        {
            Components.Add("base", new MPTanks.Engine.Rendering.RenderableComponent()
            {
                Size = new Vector2(4),
                FrameName = Assets.SatelliteDish.Base.SpriteName,
                SheetName = Assets.SatelliteDish.Base.SheetName
            });
            Components.Add("dish", new MPTanks.Engine.Rendering.RenderableComponent()
            {
                Size = new Vector2(6, 3.5f),
                RotationOrigin = new Vector2(3, 1f),
                Offset = new Vector2(-1.5f, 0.75f),
                FrameName = Assets.SatelliteDish.DishAndRevolver.SpriteName,
                SheetName = Assets.SatelliteDish.DishAndRevolver.SheetName,
                DrawLayer = 5 //Dish should always be on top of tanks
            });
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            Components["dish"].Rotation += 0.05f * ((float)time.ElapsedGameTime.TotalMilliseconds / 16.66666f);
        }
    }
}
