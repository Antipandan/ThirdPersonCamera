using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

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
        
        // tagen från https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/sizeof
        // används för att kolla storleken på en datatyp om det skulle behövas.
        public static unsafe void DisplaySizeOf<T>() where T : unmanaged
        {
            Console.WriteLine($"Size of {typeof(T)} is {sizeof(T)} bytes");
        }
        public static void ModifyQuaternion(float x, float y, float z, float w, ref quaternion quat)
        {
            // det kanske är bättre att skapa en ny quaternion eftersom den är liten men testar att göra så här istället
            quat.value.x = x;
            quat.value.y = y;
            quat.value.z = z;
            quat.value.w = w;
        }

        public static void ModifyVector2(float x, float y, ref Vector2 vector)
        {
            vector.x = x;
            vector.y = y;
        }
        
    }
}