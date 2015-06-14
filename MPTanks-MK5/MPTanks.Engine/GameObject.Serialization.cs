using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public partial class GameObject
    {
        private enum __SerializationGameObjectType : byte
        {
            GameObject,
            Tank,
            Projectile,
            MapObject
        }
        public static GameObject CreateFromSerializationInformation(GameCore game, byte[] serializationData, bool authorized = true)
        {
            var nameLength = Helpers.GetInt(serializationData, 0);
            var name = Encoding.UTF8.GetString(serializationData, 4, nameLength);
            var type = (__SerializationGameObjectType)serializationData[nameLength + 4];
            var guid = new Guid(serializationData.Slice(nameLength + 4 + 1, 16));

            GameObject obj;
            if (type == __SerializationGameObjectType.Tank)
                obj = game.AddTank(name, game.PlayersById[guid], authorized);
            else if (type == __SerializationGameObjectType.Projectile)
                obj = game.AddProjectile(name, game.PlayersById[guid].Tank, authorized);
            else if (type == __SerializationGameObjectType.MapObject)
                obj = game.AddMapObject(name, authorized);
            else
                obj = game.AddGameObject(name, authorized);

            obj.SetFullState(serializationData);

            return obj;
        }

        /// <summary>
        /// Gets the full state of the object, ergo.
        /// </summary>
        /// <returns></returns>
        public byte[] GetFullState()
        {
            //Layout:
            //4 byte reflection name length bytes
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
            /////Then, there's the data from the instance
            //4 byte byte count
            //variable Private state data bytes
            var privateState = GetPrivateStateData();
            var reflectionNameBytes = Encoding.UTF8.GetBytes(ReflectionName);
            var coreData = new byte[
                2 + reflectionNameBytes.Length + 1 + 1 + 1 + 4 + 4 + 4 + 8 + 8 + 8 + 4 + 4 + 4 + privateState.Length];
            int offset = 0;

            coreData.CopyFrom(reflectionNameBytes.Length, offset); offset += 4;
            coreData.CopyFrom(reflectionNameBytes, offset); offset += reflectionNameBytes.Length;

            coreData.CopyFrom(new[] { (byte)GetSerializationType() }, offset++);

            if (GetSerializationType() == __SerializationGameObjectType.Tank)
                coreData.CopyFrom(((Tanks.Tank)this).Player.Id.ToByteArray(), offset);
            else coreData.CopyFrom(new byte[16], offset);
            offset += 16;

            coreData.CopyFrom(new[] { (byte)(IsSensor ? 1 : 0) }, offset++);
            coreData.CopyFrom(new[] { (byte)(IsStatic ? 1 : 0) }, offset++);

            coreData.CopyFrom(ObjectId, offset); offset += 4;
            coreData.CopyFrom(ColorMask.PackedValue, offset); offset += 4;
            coreData.CopyFrom(TimeAliveMs, offset); offset += 4;

            coreData.CopyFrom(Size, offset); offset += 8;
            coreData.CopyFrom(Position, offset); offset += 8;
            coreData.CopyFrom(LinearVelocity, offset); offset += 8;

            coreData.CopyFrom(Rotation, offset); offset += 4;
            coreData.CopyFrom(AngularVelocity, offset); offset += 4;

            coreData.CopyFrom(privateState.Length, offset); offset += 4;
            coreData.CopyFrom(privateState, offset);

            return coreData;
        }

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

        protected virtual byte[] GetPrivateStateData() => new byte[] { };

        public void SetFullState(byte[] state)
        {
            var reflectionNameLength = Helpers.GetInt(state, 0);
            SetStateHeader(state.Slice(0, 2 + reflectionNameLength + 1 + 1 + 1 + 4 + 4 + 4 + 8 + 8 + 8 + 4 + 4));
            var contentsLength =
                Helpers.GetInt(state, 2 + reflectionNameLength + 1 + 1 + 1 + 4 + 4 + 4 + 8 + 8 + 8 + 4 + 4);
            SetFullStateInternal(state.Slice(
                2 + reflectionNameLength + 1 + 1 + 1 + 4 + 4 + 4 + 8 + 8 + 8 + 4 + 4 + 4, contentsLength));
        }


        private void SetStateHeader(byte[] header)
        {
            var nameLength = Helpers.GetInt(header, 0);
            var name = Encoding.UTF8.GetString(header, 4, nameLength);
            var type = (__SerializationGameObjectType)header[nameLength + 4];
            var guid = new Guid(header.Slice(nameLength + 4 + 1, 16));
            var isSensor = header[nameLength + 4 + 1 + 16] == 1;
            var isStatic = header[nameLength + 4 + 1 + 1 + 16] == 1;
            var id = Helpers.GetInt(header, nameLength + 4 + 1 + 16 + 1 + 1);
            var color = Helpers.GetColor(header, nameLength + 4 + 1 + 16 + 1 + 1 + 4);
            var timeAlive = Helpers.GetFloat(header, nameLength + 4 + 1 + 16 + 1 + 1 + 4 + 4);
            var size = Helpers.GetVector(header, nameLength + 4 + 1 + 16 + 1 + 1 + 4 + 4 + 4 + 4);
            var position = Helpers.GetVector(header, nameLength + 4 + 1 + 16 + 1 + 1 + 4 + 4 + 4 + 4 + 4 + 8);
            var linVel = Helpers.GetVector(header, nameLength + 4 + 1 + 16 + 1 + 1 + 4 + 4 + 4 + 8 + 8);
            var rot = Helpers.GetFloat(header, nameLength + 4 + 1 + 16 + 1 + 1 + 4 + 4 + 4 + 8 + 8 + 8);
            var rotVel = Helpers.GetFloat(header, nameLength + 4 + 1 + 16 + 1 + 1 + 4 + 4 + 4 + 8 + 8 + 8 + 4);

            IsSensor = isSensor;
            IsStatic = isStatic;
            ObjectId = id;
            ColorMask = color;
            TimeAliveMs = timeAlive;
            Size = size;
            Position = position;
            LinearVelocity = linVel;
            Rotation = rot;
            AngularVelocity = rotVel;
        }

        protected virtual void SetFullStateInternal(byte[] state)
        {

        }

    }
}
