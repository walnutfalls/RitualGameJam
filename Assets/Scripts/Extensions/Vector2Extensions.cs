using UnityEngine;

namespace Assets.Scripts
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Normalized -pi to pi
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Radians</returns>
        public static float SignedAngleNormalizedPi(Vector2 from, Vector2 to)
        {
            float angle = (Mathf.Atan2(to.y, to.x) - Mathf.Atan2(from.y, from.x)) % (Mathf.PI*2);            
            
            //normalize
            return angle - Math.TWO_PI * Mathf.Floor((angle + Mathf.PI) / Math.TWO_PI);
        }

        /// <summary>
        /// Normalized 0 - 2pi
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Radians</returns>
        public static float AngleNormalized2Pi(Vector2 from, Vector2 to)
        {
            float angle = Mathf.Atan2(to.y, to.x) - Mathf.Atan2(from.y, from.x);

            if (angle < 0)
                angle += Math.TWO_PI;

            return angle;
        }

        public static Vector2 FromVec3(Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }

        public static Vector2 RandomUnitVector()
        {
            float x = UnityEngine.Random.Range(-1.0f, 1.0f); //RandomRange(-1.0f, 1.0f);
            float y = UnityEngine.Random.Range(-1.0f, 1.0f); 

            return new Vector2(x, y).normalized;
        }
    }
}
