using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkit.SpriteSheets
{
    class JSONSpriteSheet
    {
        public static JSONSpriteSheet Load(string JSON) =>
            Newtonsoft.Json.JsonConvert.DeserializeObject<JSONSpriteSheet>(JSON);
        public string Serialize() =>
            Newtonsoft.Json.JsonConvert.SerializeObject(this);

        public string Name = null;

        public JSONSprite[] Sprites = null;
        public class JSONSprite
        {
            public string Name = null;
            public int X = 0;
            public int Y = 0;
            public int Width = 0;
            public int Height = 0;

        }

        public JSONAnimation[] Animations = null;
        public class JSONAnimation
        {
            public string Name = null;
            public float FrameRate = 0;
            public string[] Frames = null;
        }
    }
}
