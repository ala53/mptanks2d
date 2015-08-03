using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Assets
{
    public interface IHasSpriteInfo
    {
        SpriteInfo SpriteInfo { get; set; }
    }

    public struct SpriteInfo
    {
        public string FrameName { get; set; }
        public string SheetName { get; set; }
        public int LoopCount { get; set; }
        public TimeSpan PositionInAnimation { get; set; }
        public bool IsAnimation { get; set; }
        public SpriteInfo(string spriteName, string sheetName, bool animation = false, int loopCount = 1, TimeSpan position = default(TimeSpan))
        {
            FrameName = spriteName;
            SheetName = sheetName;
            LoopCount = loopCount;
            IsAnimation = animation;
            PositionInAnimation = position;
        }
        public SpriteInfo(SpriteInfo info)
        {
            FrameName = info.FrameName;
            SheetName = info.SheetName;
            LoopCount = info.LoopCount;
            IsAnimation = info.IsAnimation;
            PositionInAnimation = info.PositionInAnimation;
        }
    }
}