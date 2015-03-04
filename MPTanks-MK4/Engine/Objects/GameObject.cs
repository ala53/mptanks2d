using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Objects
{
    abstract class GameObject
    {
        protected Game Game { get; private set; }
        public GameObject(Game game)
        {
            Game = game;
        }

        public GameObject(Game game, byte[] state)
        {
            Game = game;
        }

        public abstract byte[] GetFullState(); 
    }
}
