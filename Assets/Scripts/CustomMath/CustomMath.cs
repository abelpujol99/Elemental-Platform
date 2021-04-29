using UnityEngine;

namespace CustomMath
{
    public static class CustomMath
    {
        public static float Map (float value, float originalMin, float originalMax, float newMin, float newMax)
        {
            return newMin + (value-originalMin) * (newMax-newMin) / (originalMax-originalMin);
        }
    }
}