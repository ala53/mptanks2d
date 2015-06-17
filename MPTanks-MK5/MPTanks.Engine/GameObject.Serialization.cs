using Microsoft.Xna.Framework;
using MPTanks.Engine.Helpers;
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
            var nameLength = SerializationHelpers.GetValue<ushort>(serializationData, 0);
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

        const int _headerSizeExcludingString =
              2 //id
            + 2 // length of reflection name
            + 1 //gameobject type
            + 16 //guid of player or projectile's creator
            + 1 //is sensor
            + 1 // is static
            + 4 //color mask
            + 4 //time object was alive
            + 8 // size
            + 8 //position
            + 8 //linear velocity
            + 4 //rotation
            + 4 //rotation velocity
            + 2 //private data size
            ;

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
            var privateStateObject = GetPrivateStateData();
            byte[] privateState;

            //Figure out what the final output should be (encoded object, string, or plain byte array)
            if (privateStateObject.GetType() == typeof(byte[]))
                privateState = (byte[])privateStateObject;
            else if (privateStateObject.GetType() == typeof(string))
            {
                privateState = new byte[stringSerializedMagicBytes.Length +
                    Encoding.UTF8.GetByteCount((string)privateStateObject)];

                privateState.SetContents(stringSerializedMagicBytes, 0);
            }
            else
            {
                var serialized = SerializeStateChangeObject(privateStateObject);
                privateState = new byte[JSONSerializedMagicBytes.Length +
                    Encoding.UTF8.GetByteCount(serialized)];

                privateState.SetContents(JSONSerializedMagicBytes, 0);
            }

            //Write the byte array
            var reflectionNameBytes = Encoding.UTF8.GetBytes(ReflectionName);
            var coreData = new byte[
                _headerSizeExcludingString + reflectionNameBytes.Length + privateState.Length];
            int offset = 0;

            coreData.SetContents(ObjectId, offset); offset += 2;

            coreData.SetContents((ushort)reflectionNameBytes.Length, offset); offset += 4;
            coreData.SetContents(reflectionNameBytes, offset); offset += reflectionNameBytes.Length;

            coreData.SetContents(new[] { (byte)GetSerializationType() }, offset++);

            if (GetSerializationType() == __SerializationGameObjectType.Tank)
                coreData.SetContents(((Tanks.Tank)this).Player.Id.ToByteArray(), offset);
            else coreData.SetContents(new byte[16], offset);
            offset += 16;

            coreData.SetContents(new[] { (byte)(IsSensor ? 1 : 0) }, offset++);
            coreData.SetContents(new[] { (byte)(IsStatic ? 1 : 0) }, offset++);

            coreData.SetContents(ColorMask.PackedValue, offset); offset += 4;
            coreData.SetContents(TimeAliveMs, offset); offset += 4;

            coreData.SetContents(Size, offset); offset += 8;
            coreData.SetContents(Position, offset); offset += 8;
            coreData.SetContents(LinearVelocity, offset); offset += 8;

            coreData.SetContents(Rotation, offset); offset += 4;
            coreData.SetContents(AngularVelocity, offset); offset += 4;

            coreData.SetContents((ushort)privateState.Length, offset); offset += 4;
            coreData.SetContents(privateState, offset);

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

        /// <summary>
        /// Return a byte array for optimal performance, or either a string or other random object for ease of use.
        /// </summary>
        /// <returns></returns>
        protected virtual object GetPrivateStateData() => SerializationHelpers.EmptyByteArray;

        public void SetFullState(byte[] state)
        {
            var reflectionNameLength = SerializationHelpers.GetValue<ushort>(state, 0);
            SetStateHeader(state);
            var contentsLength =
                SerializationHelpers.GetInt(state, _headerSizeExcludingString + reflectionNameLength - 2);

            var privateState = state.Slice(_headerSizeExcludingString + reflectionNameLength, contentsLength);

            if (privateState.SequenceBegins(JSONSerializedMagicBytes))
                SetFullStateInternal(DeserializeStateChangeObject(
                    privateState.GetString(JSONSerializedMagicBytes.Length)));
            else if (privateState.SequenceBegins(stringSerializedMagicBytes))
                SetFullStateInternal(privateState.GetString(stringSerializedMagicBytes.Length));
            else SetFullStateInternal(privateState);
        }


        private void SetStateHeader(byte[] header)
        {
            var offset = 0;
            var id = SerializationHelpers.GetValue<ushort>(header, offset); offset += 2;
            var nameLength = SerializationHelpers.GetValue<ushort>(header, offset); offset += 2;
            var name = Encoding.UTF8.GetString(header, offset, nameLength); offset += nameLength;
            var type = (__SerializationGameObjectType)header[offset++];
            var guid = new Guid(header.Slice(offset, 16)); offset += 16;
            var isSensor = header[offset++] == 1;
            var isStatic = header[offset++] == 1;
            var color = SerializationHelpers.GetColor(header, offset); offset += 4;
            var timeAlive = SerializationHelpers.GetFloat(header, offset); offset += 4;
            var size = SerializationHelpers.GetVector(header, offset); offset += 8;
            var position = SerializationHelpers.GetVector(header, offset); offset += 8;
            var linVel = SerializationHelpers.GetVector(header, offset); offset += 8;
            var rot = SerializationHelpers.GetFloat(header, offset); offset += 4;
            var rotVel = SerializationHelpers.GetFloat(header, offset); offset += 4;

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

        protected virtual void SetFullStateInternal(byte[] stateData)
        {

        }

        protected virtual void SetFullStateInternal(string state)
        {

        }

        protected virtual void SetFullStateInternal(dynamic state)
        {

        }

    }
}
