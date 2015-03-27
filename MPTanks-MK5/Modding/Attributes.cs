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
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ProjectileAttribute : Attribute
    {
        /// <summary>
        /// The name of the tank type that owns this projectile type
        /// </summary>
        public string OwnerReflectionName { get; set; }
        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class GamemodeAttribute : Attribute
    {
        /// <summary>
        /// The minimum number of players required to start a game.
        /// </summary>
        public int MinPlayersCount { get; set; }
        /// <summary>
        /// Whether to whitelist or blacklist allowed tank types.
        /// </summary>
        public bool WhitelistPlayerTankTypes { get; set; }
        /// <summary>
        /// Whether to allow supertanks
        /// </summary>
        public bool AllowSuperTanks { get; set; }
        /// <summary>
        /// If whitelisted: the allowed player tank types (reflection names)
        /// </summary>
        public IEnumerable<string> AllowedPlayerTankTypes { get; set; }
        /// <summary>
        /// If blacklisted: the disallowed player tank types (reflection names)
        /// </summary>
        public IEnumerable<string> DisallowedPlayerTankTypes { get; set; }
        /// <summary>
        /// The name of the gamemode
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The description of the gamemode
        /// </summary>
        public string Description { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ModuleDeclarationAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Author { get; private set; }
        public string Version { get; private set; }

        // This is a positional argument
        public ModuleDeclarationAttribute(string name, string description, string author, string version)
        {
            Name = name;
            Description = description;
            Author = author;
            Version = version;
        }
    }
}
