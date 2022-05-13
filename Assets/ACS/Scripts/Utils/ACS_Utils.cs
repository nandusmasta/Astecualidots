/* 
 * Project: Cualit DOTS Challenge.
 * Author: Fernando Rey. May 2022.
*/

using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.ACS.Scripts.Utils
{
    public static class ACS_Utils
    {

        public static float GetRandomFloat()
        {
            return UnityEngine.Random.Range(float.MinValue, float.MaxValue);
        }

        public static float GetRandomFloat(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static int GetRandomInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float3 ClampFloat3(float3 value, float normalizeLimit)
        {
            if (value.z > normalizeLimit)
                value.z = normalizeLimit;
            if (value.z < -normalizeLimit)
                value.z = -normalizeLimit;
            if (value.x > normalizeLimit)
                value.x = normalizeLimit;
            if (value.x < -normalizeLimit)
                value.x = -normalizeLimit;
            if (value.y > normalizeLimit)
                value.y = normalizeLimit;
            if (value.y < -normalizeLimit)
                value.y = -normalizeLimit;
            return value;
        }

    }
}
