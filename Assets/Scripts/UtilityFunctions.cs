using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public static class UtilityFunctions
    {
        /// <summary>
        /// Faster way of getting the sign of a float. Difference is slim compared to mathf implementation
        /// </summary>
        /// <param name="number">number to get sign of</param>
        /// <returns>the sign as +- 1</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSign(float number)
        {
            int integer = BitConverter.SingleToInt32Bits(number);
            return ((integer >> 31) * 2) + 1;
        }
        
        // tagen från https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/sizeof
        // används för att kolla storleken på en datatyp om det skulle behövas.
        /// <summary>
        /// Use when you want to write to the console the size of a type e.g int, float, Vector2, custom structs & classes
        /// </summary>
        /// <typeparam name="T">the type to get the sizeof</typeparam>
        public static unsafe void DisplaySizeOf<T>() where T : unmanaged
        {
            Console.WriteLine($"Size of {typeof(T)} is {sizeof(T)} bytes");
        }
        
        /// <summary>
        /// Modifies a quaternion struct with new x, y, z and w values. Takes a reference to an existing quaternion
        /// </summary>
        /// <param name="x">First factor corresponding with the first imaginary part of the quaternion's vector</param>
        /// <param name="y">Second factor corresponding with the Second imaginary part of the quaternion's vector</param>
        /// <param name="z">Third factor corresponding with the third imaginary part of the quaternion's vector</param>
        /// <param name="w">First factor corresponding with the real number of the quaternion</param>
        /// <param name="quat">quaternion instance to be modified</param>
        public static void ModifyQuaternion(float x, float y, float z, float w, ref quaternion quat)
        {
            // det kanske är bättre att skapa en ny quaternion eftersom den är liten men testar att göra så här istället
            quat.value.x = x;
            quat.value.y = y;
            quat.value.z = z;
            quat.value.w = w;
        }
        
        /// <summary>
        /// Modifies a Vector2 with new X and Y values. Takes a reference to an existing Vector2
        /// </summary>
        /// <param name="x">New X value corresponding with the vectors old X value</param>
        /// <param name="y">New Y value corresponding with the vectors old Y value</param>
        /// <param name="vector">Vector2 instance to be modified</param>
        public static void ModifyVector2(float x, float y, ref Vector2 vector)
        {
            vector.x = x;
            vector.y = y;
        }
        
        /// <summary>
        /// Returns the magnitude of a Vector2. Magnitude can substitute lenght of a vector if measured from origin
        /// </summary>
        /// <param name="vector">Vector2 instance to get the magnitude of</param>
        /// <returns>returns the magnitude</returns>
        public static float GetMagnitudeOfVector(Vector2 vector)
        {
            if (vector == Vector2.zero) return 0f;
            return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
        }
        
        /// <summary>
        /// Returns the magnitude of a Vector3. Magnitude can substitute lenght of a vector if measured from origin
        /// </summary>
        /// <param name="vector">Vector3 instance to get the magnitude of</param>
        /// <returns>returns the magnitude</returns>
        public static float GetMagnitudeOfVector(Vector3 vector)
        {
            if (vector == Vector3.zero) return 0f;
            return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
        }

        /// <summary>
        /// Calculates a normalized Vector2 of a given Vector2. Takes a reference to an existing Vector2
        /// </summary>
        /// <param name="vector">Vector2 instance to turn into a normalized Vector2</param>
        public static void NormalizeVector(ref Vector2 vector)
        {
            float magnitude = GetMagnitudeOfVector(vector);
            vector.x /= magnitude;
            vector.y /= magnitude;
        }

        /// <summary>
        /// Calculates a normalized Vector3 of a given Vector3. Takes a reference to an existing Vector3
        /// </summary>
        /// <param name="vector">Vector3 instance to turn into a normalized Vector2</param>
        public static void NormalizeVector(ref Vector3 vector)
        {
            float magnitude = GetMagnitudeOfVector(vector);
            vector.x /= magnitude;
            vector.y /= magnitude;
            vector.z /= magnitude;
        }
        
        /// <summary>
        /// Calculates a normalized Vector2 of a given Vector2.
        /// </summary>
        /// <param name="vector">Vector2 instance to turn into a normalized Vector2</param>
        /// <returns>Returns a normalized Vector2</returns>
        public static Vector2 NormalizeVector(Vector2 vector)
        {
            float magnitude = GetMagnitudeOfVector(vector);
            return new Vector2(vector.x / magnitude, vector.y / magnitude);
        }

        /// <summary>
        /// Calculates a normalized Vector3 of a given Vector3.
        /// </summary>
        /// <param name="vector">Vector3 instance to turn into a normalized Vector3</param>
        /// <returns>Returns a normalized Vector3</returns>
        public static Vector3 NormalizeVector(Vector3 vector)
        {
            float magnitude = GetMagnitudeOfVector(vector);
            return new Vector3(vector.x /  magnitude, vector.y / magnitude, vector.z / magnitude);
        }
        
        /// <summary>
        /// Calculates a quaternion which is responsible for rotation an object about the Y and Z axis
        /// (heading and pitch). Calculate said quaternion based on an input vector.Takes a reference to an existing quaternion
        /// </summary>
        /// <param name="angle">the angle to rotate in degrees</param>
        /// <param name="unitVector">unit vector to rotate around</param>
        /// <param name="quat">quaternion to be modified</param>
        public static void ConvertMouseVectorToQuaternionValue(float angle, Vector3 unitVector, ref quaternion quat)
        {
            
            float sinusValue = Mathf.Sin(angle * Mathf.Deg2Rad);
            quat.value.x = sinusValue * unitVector.x;
            quat.value.y = sinusValue * unitVector.y;
            quat.value.z = sinusValue * unitVector.z;
            quat.value.w = Mathf.Cos(angle * Mathf.Deg2Rad);
        }
        /// <summary>
        /// Calculates a quaternion which is responsible for rotation an object about the Y and Z axis
        /// (heading and pitch). Calculate said quaternion based on an input vector.Takes a reference to an existing quaternion
        /// </summary>
        /// <param name="angle">the angle to rotate in degrees</param>
        /// <param name="unitVector">unit vector to rotate around</param>
        /// <returns>returns a new quaternion</returns>>
        public static Quaternion ConvertMouseVectorToQuaternionValue(float angle, Vector3 unitVector)
        {
            float sinusValue = Mathf.Sin(angle * Mathf.Deg2Rad);
            return new Quaternion(
                Mathf.Cos(angle * Mathf.Deg2Rad), 
                sinusValue * unitVector.x, 
                sinusValue * unitVector.y, 
                sinusValue * unitVector.z);
        }

        public static quaternion ConjugateOfQuaternion(quaternion quat)
        {
            return new quaternion(quat.value.x * -1,  quat.value.y * -1, quat.value.z * -1, quat.value.w);
        }

        public static float GetMagnitudeOfQuaternion(quaternion quat)
        {
            return Mathf.Sqrt(
                quat.value.x * quat.value.x + quat.value.y * quat.value.y +
                quat.value.z * quat.value.z + quat.value.w * quat.value.w);
        }

        public static quaternion InverseQuaternion(quaternion quat)
        {
            quaternion conjugate = ConjugateOfQuaternion(quat);
            float magnitude = GetMagnitudeOfQuaternion(conjugate);
            return new quaternion(
                conjugate.value.x / magnitude, conjugate.value.y / magnitude,
                conjugate.value.z / magnitude, conjugate.value.w / magnitude);
        }

        public static void RotateAboutQuaternion(Transform position, ref quaternion quat)
        {
            Vector3 newPosition;
            quaternion rotatedQuaternion = 
        }
    }
}