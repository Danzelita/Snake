using Project.Scripts.Multiplayer.Generated;
using UnityEngine;

namespace Project.Scripts.Data
{
    public static class DataExtensions
    {
        public static Vector3 ToVector3(this Vector2Float vector2Float) => 
            new(vector2Float.x, 0f, vector2Float.z);

        public static Vector2Data ToVector2Data(this in Vector3 vector3) =>
            new(vector3.x, vector3.z);
    }
}