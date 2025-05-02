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
        
    }
}