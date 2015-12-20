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
        public IEnumerable<GameObject> GetNearbyGameObjects(Vector2 position, float radius, Func<GameObject, bool> match = null)
        {
            if (match == null)
                return GameObjects
                    .Where(x => Vector2.DistanceSquared(position, x.Position) < radius * radius);
            else
                return GameObjects
                    .Where(x => Vector2.DistanceSquared(position, x.Position) < radius * radius)
                    .Where(match);
        }
        public GameObject GetIntersectingGameObject(Vector2 position)
        {
            var fixture = World.TestPoint(position * Settings.PhysicsScale);
            return fixture?.Body?.UserData as GameObject;
        }
    }
}
