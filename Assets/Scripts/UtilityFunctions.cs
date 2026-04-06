using System;
using System.Runtime.CompilerServices;

namespace DefaultNamespace
{
    public static class UtilityFunctions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSign(float number)
        {
            int integer = BitConverter.SingleToInt32Bits(number);
            return ((integer >> 31) * 2) + 1;
        }
        
    }
}