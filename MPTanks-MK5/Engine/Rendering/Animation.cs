using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering
{
    /// <summary>
    /// An abstract description of an Animation / specific
    /// set of images that should be rendered on screen at a 
    /// specific time. 
    /// </summary>
    public class Animation
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public float Rotation { get; set; }
        public string AnimationName { get; private set; }
        public string SpriteSheetName { get; private set; }
        public float PositionInAnimationMs { get; set; }
        public Action<Animation> CompletionCallback { get; private set; }
        public float LoopCount { get; private set; }

        public Animation(string name, Vector2 center, Vector2 size, string sheetName = "", Action<Animation> callback = null, float loopCount = 1)
        {
            Position = center;
            AnimationName = name;
            SpriteSheetName = sheetName;
            Size = size;
            CompletionCallback = callback;
            LoopCount = loopCount;
        }

        /// <summary>
        /// Converts animation info to a string so it can be stored in the 
        /// "assetname" section of a renderable component.
        /// </summary>
        /// <param name="animationName"></param>
        /// <param name="positionInAnimation"></param>
        /// <returns></returns>
        public static string AnimationAsString(string animationName, string spriteSheetName, float positionInAnimationMs = 0, bool loop = false)
        {
            return "[animation]" + positionInAnimationMs + "," + spriteSheetName + "," + animationName + "," + loop;
        }
    }
}
