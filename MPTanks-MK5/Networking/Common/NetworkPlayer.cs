using MPTanks.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPTanks.Engine.Tanks;
using Microsoft.Xna.Framework;
using MPTanks.Engine.Gamemodes;

namespace MPTanks.Networking.Common
{
    public class NetworkPlayer : GamePlayer
    {
        public enum NetworkPlayerPropertyChanged
        {
            SelectedTankReflectionName,
            Id,
            UniqueId,
            AllowedTankTypes,
            Tank,
            Team,
            Game,
            Username,
            SpawnPoint,
            HasCustomTankStyle,
            ClanName,
            IsAdmin,
            IsPremium,
            IsSpectator,
            IsReady
        }
        public event EventHandler<NetworkPlayerPropertyChanged> OnPropertyChanged = delegate { };

        private bool _isReady;
        public bool IsReady
        {
            get { return _isReady; }
            set
            {
                _isReady = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.IsReady);
            }
        }

        private bool _hasCustomTankStyle;

        public bool HasCustomTankStyle
        {
            get { return _hasCustomTankStyle; }

            set
            {
                _hasCustomTankStyle = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.HasCustomTankStyle);
            }
        }
        public override string[] AllowedTankTypes
        {
            get
            {
                return base.AllowedTankTypes;
            }

            set
            {
                base.AllowedTankTypes = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.AllowedTankTypes);
            }
        }
        public override GameCore Game
        {
            get
            {
                return base.Game;
            }

            set
            {
                base.Game = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.Game);
            }
        }
        public override ushort Id
        {
            get
            {
                return base.Id;
            }

            set
            {
                base.Id = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.Id);
            }
        }
        private Guid _uid;
        public Guid UniqueId
        {
            get
            {
                return _uid;
            }
            set
            {
                _uid = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.UniqueId);
            }
        }
        public override string Username
        {
            get
            {
                return base.Username;
            }

            set
            {
                base.Username = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.Username);
            }
        }
        public override string SelectedTankReflectionName
        {
            get
            {
                return base.SelectedTankReflectionName;
            }

            set
            {
                base.SelectedTankReflectionName = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.SelectedTankReflectionName);
            }
        }
        public override Vector2 SpawnPoint
        {
            get
            {
                return base.SpawnPoint;
            }

            set
            {
                base.SpawnPoint = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.SpawnPoint);
            }
        }
        public override Team Team
        {
            get
            {
                return base.Team;
            }

            set
            {
                base.Team = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.Team);
            }
        }
        public override Tank Tank
        {
            get
            {
                return base.Tank;
            }

            set
            {
                base.Tank = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.Tank);
                ApplyTankStyles();
            }
        }

        private string _clanName;
        public string ClanName
        {
            get { return _clanName; }
            set
            {
                _clanName = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.ClanName);
            }
        }

        private bool _admin;
        public bool IsAdmin
        {
            get { return _admin; }
            set
            {
                _admin = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.IsAdmin);
            }
        }
        private bool _premium;
        public bool IsPremium
        {
            get { return _premium; }
            set
            {
                _premium = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.IsPremium);
            }
        }

        public override bool IsSpectator
        {
            get { return base.IsSpectator; }
            set
            {
                base.IsSpectator = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.IsSpectator);
            }
        }
        public void ApplyTankStyles()
        {
            if (Tank != null)
            {

            }
        }
    }
}
