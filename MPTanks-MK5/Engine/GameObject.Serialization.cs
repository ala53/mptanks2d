using Microsoft.Xna.Framework;
using MPTanks.Engine.Helpers;
using MPTanks.Engine.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public partial class GameObject
    {
        public byte[] FullState { get { return GetFullState(); } set { SetFullState(value); } }
        private enum __SerializationGameObjectType : byte
        {
            GameObject,
            Tank,
            Projectile,
            MapObject
        }
        public static GameObject CreateAndAddFromSerializationInformation(GameCore game, byte[] serializationData, bool authorized = true)
        {
            int offset = 0;
            var id = serializationData.GetUShort(offset); offset += 2;
            var name = serializationData.GetString(offset); offset += serializationData.GetUShort(offset); offset += 2;
            var type = (__SerializationGameObjectType)serializationData[offset]; offset++;
            var guid = serializationData.GetGuid(offset); offset += 16;

            GameObject obj;
            if (type == __SerializationGameObjectType.Tank)
                obj = game.AddTank(name, game.PlayersById[guid], authorized);
            else if (type == __SerializationGameObjectType.Projectile)
                obj = game.AddProjectile(name, game.PlayersById[guid].Tank, authorized);
            else if (type == __SerializationGameObjectType.MapObject)
                obj = game.AddMapObject(name, authorized);
            else
                obj = game.AddGameObject(name, authorized);

            obj.UnsafeDisableEvents();
            obj.SetFullState(serializationData);
            obj.UnsafeEnableEvents();
            return obj;
        }
        /// <summary>
        /// Gets the full state of the object, ergo.
        /// </summary>
        /// <returns></returns>
        public byte[] GetFullState()
        {
            //Layout:
            //variable string reflection name
            //1 byte GameObjectType
            //[used in all, only useful for tanks and projectiles]: player GUID
            //      If projectile, its the tank that spawned it
            //      If tank, the player that ids it
            //1 byte is sensor
            //1 byte is static
            //4 byte object id
            //4 byte color mask
            //4 byte time alive in ms
            //8 byte size
            //8 byte pos
            //8 byte lin velocity
            //4 byte rotation
            //4 byte rot velocity
            //4 byte restitution
            /////Then, there's the data from the instance
            //variable Private state data bytes
            byte[] privateState = SerializationHelpers.Serialize(GetPrivateStateData());
            
            //And figure out which guid to print
            var guidToWrite = new Guid();

            if (GetType().IsSubclassOf(typeof(Tanks.Tank)))
                guidToWrite = ((Tanks.Tank)this).Player.Id;
            else if (GetType().IsSubclassOf(typeof(Projectiles.Projectile)))
                guidToWrite = ((Projectiles.Projectile)this).Owner.Player.Id;

            return SerializationHelpers.AllocateArray(true,
            ObjectId,
            ReflectionName,
            (byte)GetSerializationType(),
            guidToWrite,
            IsSensor,
            IsStatic,
            ColorMask,
            TimeAlive.TotalMilliseconds,
            Size,
            Position,
            LinearVelocity,
            Rotation,
            AngularVelocity,
            Restitution,
            Health,
            GetTypeStateHeader(),
            privateState
            );
        }

        protected virtual byte[] GetTypeStateHeader() => SerializationHelpers.EmptyByteArray;

        private __SerializationGameObjectType GetSerializationType()
        {
            if (GetType().IsSubclassOf(typeof(Tanks.Tank)))
                return __SerializationGameObjectType.Tank;
            if (GetType().IsSubclassOf(typeof(Maps.MapObjects.MapObject)))
                return __SerializationGameObjectType.MapObject;
            if (GetType().IsSubclassOf(typeof(Projectiles.Projectile)))
                return __SerializationGameObjectType.Projectile;

            return __SerializationGameObjectType.GameObject;
        }

        /// <summary>
        /// Return a byte array for optimal performance, or either a string or other random object for ease of use.
        /// </summary>
        /// <returns></returns>
        protected virtual object GetPrivateStateData() => SerializationHelpers.EmptyByteArray;

        public void SetFullState(byte[] state)
        {
            var reflectionNameLength = state.GetValue<ushort>(0);
            int offset = 0;
            SetStateHeader(state, ref offset);


            var privateState = state.GetByteArray(offset);

            if (GlobalSettings.Debug)
                SerializationHelpers.ResolveDeserialize(privateState,
                    SetFullStateInternal, SetFullStateInternal, SetFullStateInternal);
            else
                try
                {
                    SerializationHelpers.ResolveDeserialize(privateState,
                        SetFullStateInternal, SetFullStateInternal, SetFullStateInternal);
                }
                catch (Exception ex)
                {
                    Game.Logger.Error($"GameObject full state parsing failed! {ReflectionName}[ID {ObjectId}]", ex);
                    SetFullStateInternal(privateState);
                }
        }

        private void SetStateHeader(byte[] header, ref int offset)
        {
            var id = header.GetUShort(offset); offset += 2;
            var name = header.GetString(offset); offset += header.GetUShort(offset); offset += 2;
            var type = (__SerializationGameObjectType)header[offset++];
            var guid = header.GetGuid(offset); offset += 16;
            var isSensor = header[offset++] == 1;
            var isStatic = header[offset++] == 1;
            var color = header.GetColor(offset); offset += 4;
            var timeAlive = header.GetDouble(offset); offset += 8;
            var size = header.GetVector(offset); offset += 8;
            var position = header.GetVector(offset); offset += 8;
            var linVel = header.GetVector(offset); offset += 8;
            var rot = header.GetFloat(offset); offset += 4;
            var rotVel = header.GetFloat(offset); offset += 4;
            var restitution = header.GetFloat(offset); offset += 4;
            var health = header.GetInt(offset); offset += 4;

            SetTypeStateHeader(header, ref offset); 

            IsSensor = isSensor;
            IsStatic = isStatic;
            ObjectId = id;
            ColorMask = color;
            TimeAlive = TimeSpan.FromMilliseconds(timeAlive);
            Size = size;
            Position = position;
            LinearVelocity = linVel;
            Rotation = rot;
            AngularVelocity = rotVel;
            Health = health;
        }

        /// <summary>
        /// Called so a class of objects (Tanks, MapObjects, or Projectiles) can set it's own type information.
        /// This is usually sealed.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="offset"></param>
        protected virtual void SetTypeStateHeader(byte[] header, ref int offset)
        {
            
        }

        protected virtual void SetFullStateInternal(byte[] stateData) { }

        protected virtual void SetFullStateInternal(string state) { }

        protected virtual void SetFullStateInternal(dynamic state)
        {
        }
        /// <summary>
        /// Allows you to do deferred state initialization, as <see cref="SetFullStateInternal"/>
        /// is called before the world is fully initialized. This is usually only necessary when
        /// interacting with other objects.
        /// </summary>
        public virtual void SetPostInitStateData() { }

    }
}
