using UnityEngine;

namespace CMath
{
    public static class CMath
    {
        public static float Map (float value, float originalMin, float originalMax, float newMin, float newMax)
        {
            return newMin + (value-originalMin) * (newMax-newMin) / (originalMax-originalMin);
        }
    }
}