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
            HasSelectedTankYet,
            Id,
            AllowedTankTypes,
            Tank,
            Team,
            Game,
            DisplayName,
            DisplayNameColor,
            SpawnPoint,
            HasCustomTankStyle,
            ClanName,
            IsAdmin,
            IsPremium,
            IsSpectator,
        }
        public event EventHandler<NetworkPlayerPropertyChanged> OnPropertyChanged = delegate { };

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
        public override bool HasSelectedTankYet
        {
            get
            {
                return base.HasSelectedTankYet;
            }

            set
            {
                base.HasSelectedTankYet = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.HasSelectedTankYet);
            }
        }
        public override Guid Id
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
        public override string DisplayName
        {
            get
            {
                return base.DisplayName;
            }

            set
            {
                base.DisplayName = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.DisplayName);
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

        private Color _dispNameColor;
        public Color DisplayNameDrawColor
        {
            get { return _dispNameColor; }
            set
            {
                _dispNameColor = value;
                OnPropertyChanged(this, NetworkPlayerPropertyChanged.DisplayNameColor);
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
