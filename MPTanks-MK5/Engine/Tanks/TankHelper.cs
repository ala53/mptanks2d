using Microsoft.Xna.Framework;
using MPTanks.Engine.Helpers;
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

            currentRotation = BasicHelpers.NormalizeAngle(currentRotation);
            targetRotation = BasicHelpers.NormalizeAngle(targetRotation);
            
            var shortestDistance = BasicHelpers.NormalizeAngle(
                targetRotation - currentRotation + MathHelper.Pi) - MathHelper.Pi;
            if (shortestDistance < 0)
                return currentRotation + Math.Max(shortestDistance, -maxSpeed);

            return currentRotation + Math.Min(shortestDistance, maxSpeed);
        }
    }
}
