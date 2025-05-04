using UnityEngine;

namespace CodeBase.Tools
{
    public static class MyExtensions
    {
        public static float SetAbsoluteValue(this float num, float value)
        {
            if (num > 0)
            {
                return value;
            }else if (num < 0)
            {
                return -value;
            }

            return 0;
        }

        public static void ImportData(this CharacterController controller, CharacterData from)
        {
            controller.slopeLimit = from.slopeLimit;
            controller.height = from.height;
            controller.stepOffset = from.stepOffset;
            controller.skinWidth = from.skinWidth;
            controller.center = from.center;
            controller.radius = from.radius;
        }
        
    }
}