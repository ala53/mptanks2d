using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding
{

    /// <summary>
    /// An attribute that marks an object as a tank
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class TankAttribute : Attribute
    {
        public TankAttribute()
        {

        }

        /// <summary>
        /// The name for use in reflective initialization
        /// </summary>
        public string ReflectionName {get; set;}
        /// <summary>
        /// The display name of the tank: E.g. "Basic Tank"
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// The description to display for the tank. 
        /// E.g. "A simple tank for simple people"
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Whether this tank types requires a matching tank on the enemy team.
        /// </summary>
        public bool RequiresMatchingOnOtherTeam { get; set; }
        /// <summary>
        /// The general category this tank fits under, for matchmaking reasons.
        /// </summary>
        public TankCategory Category { get; set; }

        public enum TankCategory
        {
            Basic,
            Artillery,
            Supression,
            Stealth,
            SuperTank,
            StealthSuperTank,
            Engineer,
            Melee,

        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class MapObjectAttribute : Attribute
    {
        /// <summary>
        /// Whether the body for the object is static or dynamic
        /// </summary>
        public bool IsStatic { get; set; }
        public float MinWidthBlocks { get; set; }
        public float MinHeightBlocks { get; set; }
        public string Name { get; set; }
        public string ReflectionName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ProjectileAttribute : Attribute
    {
        /// <summary>
        /// The name of the tank type (it's reflection name) this projectile type applies to.
        /// </summary>
        public string TankReflectionName { get; set; }
        public string ReflectionName { get; set; }
        public string Name { get; set; }
    }
}
