using UnityEngine;
namespace CodeBase.Infrastructure.Data
{
    public static class DataExtensions
    {
        public static Vector3 AddY(this Vector3 vector3, float y)
        {
            return new Vector3(vector3.x, vector3.y + y, vector3.z);
        }

        public static Vector3 ChangeY(this Vector3 vector3, float y)
        {
            return new Vector3(vector3.x, y, vector3.z);
        }
    }
}
