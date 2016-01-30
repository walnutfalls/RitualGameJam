using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class TransformExtensions
    {
        public static void MatchUpVector(this Transform transform, Vector2 targetUp)
        {
            //get angle from up to target's up
            float angle = Vector3.Angle(transform.up, targetUp);

            float orient = Math.Orient2D(Vector2.zero, transform.up.normalized, targetUp.normalized);

            if (orient < 0)
                angle = -angle;

            //rotate to match target's up
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward) * transform.rotation;
        }

        public static IEnumerable<Transform> GetChildrenRecursive(this Transform transform)
        {
            List<Transform> children = new List<Transform>();
            FillRecursiveChildList(transform, children);
            return children;         
        }

        private static void FillRecursiveChildList(Transform transform, List<Transform> children)
        {
            children.Add(transform);

            foreach(Transform t in transform)
                FillRecursiveChildList(t, children);
        }
    }
}
