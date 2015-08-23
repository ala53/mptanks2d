using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public partial class GameCore
    {
        public IEnumerable GetNearbyGameObjects(Vector2 position, float radius, Func<GameObject, bool> match = null)
        {
            if (match == null)
                return GameObjects
                    .Where(x => Vector2.DistanceSquared(position, x.Position) < radius * radius);
            else
                return GameObjects
                    .Where(x => Vector2.DistanceSquared(position, x.Position) < radius * radius)
                    .Where(match);
        }
    }
}
