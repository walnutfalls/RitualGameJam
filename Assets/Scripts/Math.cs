using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class Math
    {
        public const float TWO_PI = Mathf.PI * 2;
        public const float EPS = 0.0005f;

        public const float e = 2.71828182846f;
        


        public static float Orient2D(Vector2 a, Vector2 b, Vector2 c)
        {
            //orient a: (ax, ay)  b: (bx, by), c: (cx, cy):
            // 
            // |ax - cx     ay - cy|
            // |bx - cx     by - cy|
            return ((a.x - c.x) * (b.y - c.y)) - ((a.y - c.y) * (b.x - c.x));
        }
    }


}