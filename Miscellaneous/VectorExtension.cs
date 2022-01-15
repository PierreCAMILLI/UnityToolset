using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolset
{
    public static class VectorExtension
    {
        public static Vector3 XZ(this Vector2 vector)
        {
            return new Vector3(vector.x, 0f, vector.y);
        }

        public static Vector2 XZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        public static Vector3 X0Z(this Vector3 vector)
        {
            return new Vector3(vector.x, 0f, vector.z);
        }

        public static Vector3 XZ(this Vector2Int vector)
        {
            return new Vector3(vector.x, 0f, vector.y);
        }

        public static Vector2 Rotate(this Vector2 vector, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = vector.x;
            float ty = vector.y;
            vector.x = (cos * tx) - (sin * ty);
            vector.y = (sin * tx) + (cos * ty);
            return vector;
        }
    }
}
