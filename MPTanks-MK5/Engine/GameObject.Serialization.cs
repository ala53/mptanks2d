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
        public byte[] FullState
        {
            get
            {
                var data = GetFullState();
                return data.ReleaseAndReturnData();
            }
            set { var reader = ByteArrayReader.Get(value); SetFullState(reader); reader.Release(); }
        }
        private enum __SerializationGameObjectType : byte
        {
            GameObject,
            Tank,
            Projectile,
            MapObject
        }
        public static GameObject CreateAndAddFromSerializationInformation(GameCore game, ByteArrayReader reader, bool authorized = true)
        {
            var objectId = reader.ReadUShort();
            var reflectionName = reader.ReadString();
            var type = (__SerializationGameObjectType)reader.ReadByte();
            var playerUid = reader.ReadUShort();

            GameObject obj = null;
            if (type == __SerializationGameObjectType.Tank)
            {
                if (game.PlayersById.ContainsKey(playerUid))
                    obj = game.AddTank(reflectionName, game.PlayersById[playerUid], authorized, objectId);
            }
            else if (type == __SerializationGameObjectType.Projectile)
            {
                if (game.PlayersById.ContainsKey(playerUid))
                    obj = game.AddProjectile(reflectionName, game.PlayersById[playerUid].Tank, authorized, objectId);
            }
            else if (type == __SerializationGameObjectType.MapObject)
                obj = game.AddMapObject(reflectionName, authorized, objectId);
            else
                obj = game.AddGameObject(reflectionName, authorized, objectId);

            if (obj == null) return null;

            obj.UnsafeDisableEvents();
            obj.SetFullState(reader);
            obj.UnsafeEnableEvents();
            return obj;
        }
        /// <summary>
        /// Gets the full state of the object, ergo.
        /// </summary>
        /// <returns></returns>
        public ByteArrayWriter GetFullState()
        {
            //And figure out which guid to print
            ushort playerUid = 0;

            if (GetType().IsSubclassOf(typeof(Tanks.Tank)))
                playerUid = ((Tanks.Tank)this).Player.Id;
            else if (GetType().IsSubclassOf(typeof(Projectiles.Projectile)))
                playerUid = ((Projectiles.Projectile)this).Owner.Player.Id;

            var writer = ByteArrayWriter.Get();
            writer.Write(ObjectId);
            writer.Write(ReflectionName);
            writer.Write((byte)GetSerializationType());
            writer.Write(playerUid);
            writer.Write(IsSensor);
            writer.Write(IsStatic);
            writer.Write(ColorMask);
            writer.Write(TimeAlive);
            writer.Write(Size);
            writer.Write(Position);
            writer.Write(LinearVelocity);
            writer.Write(Rotation);
            writer.Write(AngularVelocity);
            writer.Write(Restitution);
            writer.Write(Health);

            GetTypeStateHeader(writer);

            GetFullStateInternal(writer);
            return writer;
        }

        protected virtual void GetTypeStateHeader(ByteArrayWriter writer) { }

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
        protected virtual void GetFullStateInternal(ByteArrayWriter writer) { }

        public void SetFullState(ByteArrayReader reader)
        {
            reader.Offset = 0;

            var objectId = reader.ReadUShort();
            var reflectionName = reader.ReadString();
            var type = (__SerializationGameObjectType)reader.ReadByte();
            var playerUid = reader.ReadUShort();

            SetStateHeader(reader);

            if (GlobalSettings.Debug)
                SetFullStateInternal(reader);
            else
                try
                {
                    SetFullStateInternal(reader);
                }
                catch (Exception ex)
                {
                    Game.Logger.Error($"GameObject full state parsing failed! {ReflectionName}[ID {ObjectId}]", ex);
                    SetFullStateInternal(reader);
                }
        }

        private void SetStateHeader(ByteArrayReader reader)
        {
            IsSensor = reader.ReadBool();
            IsStatic = reader.ReadBool();
            ColorMask = reader.ReadColor();
            TimeAlive = reader.ReadTimeSpan();
            Size = reader.ReadVector();
            Position = reader.ReadVector();
            LinearVelocity = reader.ReadVector();
            Rotation = reader.ReadFloat();
            AngularVelocity = reader.ReadFloat();
            Restitution = reader.ReadFloat();
            Health = reader.ReadFloat();

            SetTypeStateHeader(reader);
        }

        /// <summary>
        /// Called so a class of objects (Tanks, MapObjects, or Projectiles) can set it's own type information.
        /// This is usually sealed.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="offset"></param>
        protected virtual void SetTypeStateHeader(ByteArrayReader reader)
        {

        }

        protected virtual void SetFullStateInternal(ByteArrayReader reader) { }
        /// <summary>
        /// Allows you to do deferred state initialization, as <see cref="SetFullStateInternal"/>
        /// is called before the world is fully initialized. This is usually only necessary when
        /// interacting with other objects.
        /// </summary>
        public virtual void SetPostInitStateData() { }

    }
}
