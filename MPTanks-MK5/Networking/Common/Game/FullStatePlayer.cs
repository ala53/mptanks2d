using Lidgren.Network;
using Microsoft.Xna.Framework;
using MPTanks.Engine;
using MPTanks.Engine.Gamemodes;
using MPTanks.Engine.Tanks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Common.Game
{
    public class FullStatePlayer
    {
        public bool PlayerWantsToBeSpectator { get; set; }
        public bool GameSpectatorFlagSet { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPremium { get; set; }
        public bool IsReady { get; set; }
        public bool TankHasCustomStyle { get; set; }
        public bool HasTank { get; set; }
        public bool HasSelectedTank { get; set; }
        public ushort TankObjectId { get; set; }
        public string Username { get; set; }
        public ushort Id { get; set; }
        public Guid UniqueId { get; set; }
        public short TeamId { get; set; }
        public Vector2 SpawnPoint { get; set; }
        public string TankReflectionName { get; set; }
        public string[] AllowedTankTypes { get; set; }
        public NetworkPlayer PlayerObject { get; set; }
        public InputState Input { get; set; }

        public FullStatePlayer()
        {

        }

        public FullStatePlayer(NetworkPlayer plr)
        {
            AllowedTankTypes = plr.AllowedTankTypes;
            HasSelectedTank = plr.HasSelectedTankYet;
            HasTank = plr.HasTank;
            Id = plr.Id;
            IsAdmin = plr.IsAdmin;
            IsPremium = plr.IsPremium;
            GameSpectatorFlagSet = plr.IsSpectatorFlagSet;
            PlayerWantsToBeSpectator = plr.PlayerWantsToBeSpectator;
            SpawnPoint = plr.SpawnPoint;
            TankHasCustomStyle = plr.HasCustomTankStyle;
            TankObjectId = (HasTank) ? plr.Tank.ObjectId : (ushort)0;
            TankReflectionName = (plr.Tank != null) ? plr.Tank.ReflectionName : "";
            TeamId = (plr.Team != null) ? plr.Team.TeamId : (short)-3;
            Input = (plr.Tank != null ? plr.Tank.InputState : default(InputState));
            Username = plr.Username;
            IsReady = plr.IsReady;
            PlayerObject = plr;
            UniqueId = plr.UniqueId;
        }

        public static FullStatePlayer Read(NetIncomingMessage msg)
        {
            var fs = new FullStatePlayer();
            fs.GameSpectatorFlagSet = msg.ReadBoolean();
            fs.PlayerWantsToBeSpectator = msg.ReadBoolean();
            fs.IsAdmin = msg.ReadBoolean();
            fs.IsPremium = msg.ReadBoolean();
            fs.IsReady = msg.ReadBoolean();
            fs.TankHasCustomStyle = msg.ReadBoolean();
            fs.HasTank = msg.ReadBoolean();
            fs.HasSelectedTank = msg.ReadBoolean();
            if (fs.HasTank) fs.TankObjectId = msg.ReadUInt16();
            fs.Username = msg.ReadString();
            fs.Id = (ushort)msg.ReadUInt32(GameCore.PlayerIdNumberOfBits);
            fs.UniqueId = new Guid(msg.ReadBytes(16));
            fs.TeamId = msg.ReadInt16();
            fs.SpawnPoint = new Vector2(msg.ReadFloat(), msg.ReadFloat());
            if (fs.HasSelectedTank)
                fs.TankReflectionName = msg.ReadString();
            var allowedTankTypes = new string[msg.ReadInt32()];
            for (var i = 0; i < allowedTankTypes.Length; i++)
                allowedTankTypes[i] = msg.ReadString();
            var input = new InputState();
            input.FirePressed = msg.ReadBoolean();
            input.LookDirection = msg.ReadFloat();
            input.MovementSpeed = msg.ReadFloat();
            input.RotationSpeed = msg.ReadFloat();
            input.WeaponNumber = msg.ReadByte();

            fs.Input = input;
            return fs;
        }

        public void Apply(NetworkPlayer player)
        {
            player.IsSpectatorFlagSet = GameSpectatorFlagSet;
            player.PlayerWantsToBeSpectator = PlayerWantsToBeSpectator;
            player.IsAdmin = IsAdmin;
            player.IsPremium = IsPremium;
            player.IsReady = IsReady;
            player.HasCustomTankStyle = TankHasCustomStyle;
            player.Username = Username;
            player.Id = Id;
            player.UniqueId = UniqueId;
            player.SpawnPoint = SpawnPoint;
            player.SelectedTankReflectionName = TankReflectionName;
            player.AllowedTankTypes = AllowedTankTypes;
            PlayerObject = player;
        }

        public void ApplySecondPass(NetworkPlayer player, GameCore game)
        {
            if (game.GameObjectsById.ContainsKey(TankObjectId))
            {
                player.Tank = (HasTank ? (Tank)game.GameObjectsById[TankObjectId] : null);
                if (HasTank) player.Tank.InputState = Input;
            }
            player.Team = (TeamId != -3 ? FindTeam(game.Gamemode.Teams, TeamId) : null);
        }

        private Team FindTeam(Team[] teams, int id)
        {
            foreach (var t in teams)
                if (t.TeamId == id) return t;

            return Team.Null;
        }

        public NetworkPlayer ToNetworkPlayer()
        {
            var np = new NetworkPlayer();
            Apply(np);
            return np;
        }

        public void Write(NetOutgoingMessage msg)
        {
            msg.Write(GameSpectatorFlagSet);
            msg.Write(PlayerWantsToBeSpectator);
            msg.Write(IsAdmin);
            msg.Write(IsPremium);
            msg.Write(IsReady);
            msg.Write(TankHasCustomStyle);
            msg.Write(HasTank);
            msg.Write(HasSelectedTank);
            if (HasTank)
                msg.Write(TankObjectId);
            msg.Write(Username);
            msg.Write(Id, GameCore.PlayerIdNumberOfBits);
            msg.Write(UniqueId.ToByteArray());
            msg.Write(TeamId);
            msg.Write(SpawnPoint.X);
            msg.Write(SpawnPoint.Y);
            if (HasSelectedTank)
                msg.Write(TankReflectionName);
            msg.Write(AllowedTankTypes?.Length ?? 0);
            if (AllowedTankTypes != null)
                foreach (var type in AllowedTankTypes) msg.Write(type);
            msg.Write(Input.FirePressed);
            msg.Write(Input.LookDirection);
            msg.Write(Input.MovementSpeed);
            msg.Write(Input.RotationSpeed);
            msg.Write((byte)Input.WeaponNumber);
        }
    }
}
