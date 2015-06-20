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
            SpawnPoint,
            HasCustomTankStyle,
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

        public void ApplyTankStyles()
        {
            if (Tank != null)
            {

            }
        }
    }
}
