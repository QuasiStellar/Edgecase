using UnityEngine;

namespace Utils
{
    public static class Vectors
    {
        public static Vector3 FLeft => new Vector3(-0.5f, 0, Numbers.Sqrt3By2);
        public static Vector3 FRight => new Vector3(0.5f, 0, Numbers.Sqrt3By2);
        public static Vector3 BLeft => new Vector3(-0.5f, 0, -Numbers.Sqrt3By2);
        public static Vector3 BRight => new Vector3(0.5f, 0, -Numbers.Sqrt3By2);
    }
}