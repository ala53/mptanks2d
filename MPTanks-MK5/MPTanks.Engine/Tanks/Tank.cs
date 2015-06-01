using MPTanks.Engine.Gamemodes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Tanks
{

    public abstract class Tank : GameObject
    {
        public GamePlayer Player { get; private set; }
        public Team Team { get; internal set; }
        public InputState InputState { get; private set; }

        public int Health { get; protected set; }

        protected abstract float RotationSpeed { get; }
        protected abstract float MovementSpeed { get; }

        public Tank(GamePlayer player, Team team, GameCore game, bool authorized)
            : base(game, authorized, game.Settings.TankDensity, 0, default(Vector2), 0)
        {
            Player = player;
            Team = team;
        }

        private bool _killed = false;
        protected override bool CollideInternal(GameObject other, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (other.GetType().IsSubclassOf(typeof(Projectiles.Projectile)) && other.Alive)
            {
                var o = (Projectiles.Projectile)other;

                if (!IsDamageAllowed(o.Owner))
                    return true;

                Health -= o.DamageAmount;

                o.CollidedWithTank(this);

                if (Health <= 0 && !_killed)
                {
                    Game.RemoveGameObject(this, o);
                    _killed = true;
                }
                return true;
            }

            return base.CollideInternal(other, contact);
        }

        private bool IsDamageAllowed(Tank tank)
        {
            if (Game.FriendlyFireEnabled)
                return true;
            if (tank.Team != Team)
                return true;
            return false;
        }

        public void Input(InputState state)
        {
            InputState = state;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            var velocity = InputState.MovementSpeed *
                MovementSpeed;

            var rotationAmount = InputState.RotationSpeed *
                RotationSpeed * (time.ElapsedGameTime.TotalMilliseconds / 16.66666);

            //calculate the actual movement by axis
            var x = velocity * Math.Sin(Rotation);
            var y = velocity * -Math.Cos(Rotation);

            LinearVelocity = new Vector2((float)x, (float)y);
            AngularVelocity = 0;
            Rotation += (float)rotationAmount;
        }



        public override string ToString()
        {
            return "Player ID: " + Player.Id.ToString();
        }

        #region Static initialization
        private static Dictionary<string, Type> _tankTypes =
            new Dictionary<string, Type>();

        public static Tank ReflectiveInitialize(string tankName, GamePlayer player, Team team, GameCore game, bool authorized, byte[] state = null)
        {
            if (!_tankTypes.ContainsKey(tankName.ToLower())) throw new Exception("Tank type does not exist.");

            var inst = (Tank)Activator.CreateInstance(_tankTypes[tankName.ToLower()], player, team, game, authorized);
            if (state != null) inst.ReceiveStateData(state);

            return inst;
        }

        public static T ReflectiveInitialize<T>(string tankName, GamePlayer player, Team team, GameCore game, bool authorized, byte[] state = null)
            where T : Tank
        {
            return (T)ReflectiveInitialize(tankName, player, team, game, authorized, state);
        }

        private static void RegisterType<T>() where T : Tank
        {
            //get the name
            var name = ((MPTanks.Modding.GameObjectAttribute)(typeof(T).
                GetCustomAttributes(typeof(MPTanks.Modding.GameObjectAttribute), true))[0]).ReflectionTypeName;
            if (_tankTypes.ContainsKey(name)) throw new Exception("Already registered!");

            _tankTypes.Add(name.ToLower(), typeof(T));
        }

        public static ICollection<string> GetAllTankTypes()
        {
            return _tankTypes.Keys;
        }
        #endregion

        static Tank()
        {
        }
    }
}
