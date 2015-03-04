using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Tanks
{
    public struct InputState
    {
        public float XMovementState;
        public float YMovementState;
        public float LookDirection;
        public bool FireState;
        public int WeaponNumber;

        public static bool operator ==(InputState i1, InputState i2)
        {
            return (i1.XMovementState == i2.XMovementState) &&
                (i1.YMovementState == i2.YMovementState) &&
                (i1.FireState == i2.FireState) &&
                (i1.WeaponNumber == i2.WeaponNumber)&&
                (i1.LookDirection == i2.LookDirection);
        }

        public static bool operator !=(InputState i1, InputState i2)
        {
            return !(i1 == i2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is InputState))
                return false;
            else
                return (((InputState)obj) == this);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
