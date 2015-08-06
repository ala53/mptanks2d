using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine.Tanks
{
    public static class TankHelper
    {
        public static float ConstrainTurretRotation(float? minRotation, float? maxRotation, float currentRotation, float targetRotation, float maxSpeed)
        {
            if (minRotation != null && maxRotation != null)
            {
                if (targetRotation < minRotation) targetRotation = minRotation.Value;
                if (targetRotation > maxRotation) targetRotation = maxRotation.Value;
            }

            currentRotation = NormalizeAngle(currentRotation);
            targetRotation = NormalizeAngle(targetRotation);

            var a = NormalizeAngle(targetRotation - currentRotation + MathHelper.Pi) - MathHelper.Pi;

            var shortestDistance = a;
            if (shortestDistance < 0)
                return currentRotation + Math.Max(shortestDistance, -maxSpeed);

            return currentRotation + Math.Min(shortestDistance, maxSpeed);
        }

        private static float NormalizeAngle(float angle)
        {
            while (angle < 0) angle += MathHelper.TwoPi;

            while (angle > MathHelper.TwoPi) angle -= MathHelper.TwoPi;

            return angle;
        }
    }
}
