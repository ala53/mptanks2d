using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Tanks
{
    public struct InputState
    {
        public float RotationSpeed;
        public float MovementSpeed;
        public float LookDirection;
        public bool FirePressed;
        public int WeaponNumber;

        public static bool operator ==(InputState i1, InputState i2)
        {
            return (i1.RotationSpeed == i2.RotationSpeed) &&
                (i1.MovementSpeed == i2.MovementSpeed) &&
                (i1.FirePressed == i2.FirePressed) &&
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
