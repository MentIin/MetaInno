using UnityEngine;

namespace CodeBase.Infrastructure.Data
{
    public static class DataExtensions
    {
        public static Vector2Data AsVector2Data(this Vector2 vector2) =>
            new Vector2Data(vector2.x, vector2.y);

        public static Vector2 AsUnityVector2(this Vector2Data vector2Data) =>
            new Vector2(vector2Data.X, vector2Data.Y);

        public static Vector3 Absolute(this Vector3 vector) => new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y),
            Mathf.Abs(vector.z));
        

        public static Quaternion ToQuaternion(this Direction direction)
        {
            float z = (float)direction;
            return Quaternion.Euler(0, 0, z);
        }

        public static string AsJson(this object obj) => JsonUtility.ToJson(obj);
        
        public static T ToDeserialized<T>(this string json) => JsonUtility.FromJson<T>(json);
        

        public static float FromMillisecondsToSeconds(this int mil) => (float) mil / 100;
    }
}