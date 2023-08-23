using System.Collections;
using UnityEngine;

namespace Assets._SDK.Utils
{
    public static class MathExtensions
    {

        public static int OfValue(this int percent, int value)
        {
            return Mathf.FloorToInt(((float)percent / 100) * value);
        }

        public static float OfValue(this int percent, float value)
        {
            return ((float)percent / 100) * value;
        }

        public static float DistanceFrom(this Vector2 vectorA, Vector2 vectorB)
        {
            return GetDistance(vectorA.x, vectorA.y, vectorB.x, vectorB.y);
        }

        public static float GetDistance(float x1, float y1, float x2, float y2)
        {
            return GetDistance(x1, y1, 0, x2, y2, 0);
        }

        public static float GetDistance(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2) + Mathf.Pow(z1 - z2, 2));
        }
    }
}