using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Extensions
{
    public static class Collision2DExtensions
    {
        /// <summary>
        /// A measure of how hard objects collided. 
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="thisMass"></param>
        /// <returns>A </returns>
        public static float GetCollisionForceIndex(this Collision2D coll, float thisMass)
        {
            Rigidbody2D colObjRb = coll.rigidbody;
            return thisMass * Vector2.Dot(coll.relativeVelocity, coll.contacts[0].normal);
        }

        public static float GetCollisionForceIndex(this Collision2D coll)
        {
            if (coll.relativeVelocity.magnitude == 0)
                return 0;

            Vector2 avgNormal = Vector2.zero;

            foreach (var c in coll.contacts) avgNormal += c.normal;
            avgNormal /= coll.contacts.Length;

            return Vector2.Dot(-coll.relativeVelocity, avgNormal.normalized);
        }
    }
}
