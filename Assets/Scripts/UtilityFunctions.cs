using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Utility
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
        /// Get the dot product of two Vector2 instances.
        /// see https://gamemath.com/book/vectors.html#cross_product#dot_product for equation
        /// </summary>
        /// <param name="v1">First Vector2</param>
        /// <param name="v2">Second Vector2</param>
        /// <returns>Scalar representing the dot product</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DotProduct(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.x + v1.y * v2.y;     
        }
        
        /// <summary>
        /// Get the dot product of two Vector3 instances
        /// see https://gamemath.com/book/vectors.html#cross_product#dot_product for equation
        /// </summary>
        /// <param name="v1">First Vector3</param>
        /// <param name="v2">Second Vector3</param>
        /// <returns>Scalar representing the dot product</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DotProduct(Vector3 v1, Vector3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;    
        }

        /// <summary>
        /// Get the cross product of 2 Vector3 instances
        /// see https://gamemath.com/book/vectors.html#cross_product#cross_product for equation
        /// </summary>
        /// <param name="v1">First Vector3</param>
        /// <param name="v2">Second Vector3</param>
        /// <returns>Vector3 that represents the cross product</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 CrossProduct(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.y*v2.z - v2.y*v1.z, v1.z*v2.x - v2.z*v1.x, v1.x*v2.y - v2.x*v1.y);
        }
        
        /// <summary>
        /// Modifies a Vector2 with new X and Y values. Takes a reference to an existing Vector2
        /// </summary>
        /// <param name="x">New X value corresponding with the vectors old X value</param>
        /// <param name="y">New Y value corresponding with the vectors old Y value</param>
        /// <param name="vector">Vector2 instance to be modified</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ModifyVector2(float x, float y, ref Vector2 vector)
        {
            vector.x = x;
            vector.y = y;
        }
        
        /// <summary>
        /// Returns the magnitude of a Vector2. Magnitude can substitute lenght of a vector if measured from origin.
        /// see https://gamemath.com/book/vectors.html#vector_magnitude for details
        /// </summary>
        /// <param name="vector">Vector2 instance to get the magnitude of</param>
        /// <returns>returns the magnitude</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMagnitudeOfVector(Vector2 vector)
        {
            if (vector == Vector2.zero) return 1f;
            return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
        }
        
        /// <summary>
        /// Returns the magnitude of a Vector3. Magnitude can substitute lenght of a vector if measured from origin.
        /// see https://gamemath.com/book/vectors.html#vector_magnitude for details
        /// </summary>
        /// <param name="vector">Vector3 instance to get the magnitude of</param>
        /// <returns>returns the magnitude</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMagnitudeOfVector(Vector3 vector)
        {
            // lite av en bandaid fix men så här gör vi!
            if (vector == Vector3.zero) return 1f;
            return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
        }

        /// <summary>
        /// Calculates a normalized Vector2 of a given Vector2. Takes a reference to an existing Vector2
        /// see https://gamemath.com/book/vectors.html#normalized_vectors for details
        /// </summary>
        /// <param name="vector">Vector2 instance to turn into a normalized Vector2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NormalizeVector(ref Vector2 vector)
        {
            float magnitude = GetMagnitudeOfVector(vector);
            vector.x /= magnitude;
            vector.y /= magnitude;
        }

        /// <summary>
        /// Calculates a normalized Vector3 of a given Vector3. Takes a reference to an existing Vector3
        /// see https://gamemath.com/book/vectors.html#normalized_vectors for details
        /// </summary>
        /// <param name="vector">Vector3 instance to turn into a normalized Vector2</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NormalizeVector(ref Vector3 vector)
        {
            float magnitude = GetMagnitudeOfVector(vector);
            vector.x /= magnitude;
            vector.y /= magnitude;
            vector.z /= magnitude;
        }
        
        /// <summary>
        /// Calculates a normalized Vector2 of a given Vector2.
        /// see https://gamemath.com/book/vectors.html#normalized_vectors for details
        /// </summary>
        /// <param name="vector">Vector2 instance to turn into a normalized Vector2</param>
        /// <returns>Returns a normalized Vector2</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 NormalizeVector(Vector2 vector)
        {
            float magnitude = GetMagnitudeOfVector(vector);
            return new Vector2(vector.x / magnitude, vector.y / magnitude);
        }

        /// <summary>
        /// Calculates a normalized Vector3 of a given Vector3.
        /// see https://gamemath.com/book/vectors.html#normalized_vectors for details
        /// </summary>
        /// <param name="vector">Vector3 instance to turn into a normalized Vector3</param>
        /// <returns>Returns a normalized Vector3</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        public static void ConvertMouseVectorToQuaternionValue(float angle, Vector3 unitVector, ref Quaternion quat)
        {
            if (unitVector == Vector3.zero) return;
            float sinusValue = Mathf.Sin((angle /2f));
            quat.x = sinusValue * unitVector.x;
            quat.y = sinusValue * unitVector.y;
            quat.z = sinusValue * unitVector.z;
            quat.w = Mathf.Cos((angle / 2f));
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
            float sinusValue = Mathf.Sin(angle / 2f * Mathf.Deg2Rad);
            return new Quaternion(
                sinusValue * unitVector.x, 
                sinusValue * unitVector.y, 
                sinusValue * unitVector.z,
                Mathf.Cos(angle / 2f * Mathf.Deg2Rad));
        }
        
        public static Quaternion AxisAngleQuaternion(Vector3 axis, float angleDegrees)
        {
            float angleRad = angleDegrees * Mathf.Deg2Rad * 0.5f;
            axis = axis.normalized;
            float s = Mathf.Sin(angleRad);
            float c = Mathf.Cos(angleRad);
            return new Quaternion(axis.x * s, axis.y * s, axis.z * s, c);
        }
        
        public static Vector3 RotatePosition(Quaternion rotationQuaternion, Vector3 position)
        {
            Quaternion quatPosition = new Quaternion(position.x, position.y, position.z, 0f);
            Quaternion inverseQuaternion = InverseQuaternion(rotationQuaternion);
            Quaternion rotatedPosition = MultiplyQuaternion(MultiplyQuaternion(rotationQuaternion, quatPosition), inverseQuaternion);

            return new Vector3(rotatedPosition.x, rotatedPosition.y, rotatedPosition.z);
        }

        /// <summary>
        /// Calculate the Conjugate of a given quaternion. Returns a new conjugate quaternion.
        /// See https://gamemath.com/book/orient.html#quaternion_conjugate equation 8.6
        /// </summary>
        /// <param name="quat">quaternion to use to get conjugate</param>
        /// <returns>Conjugate quaternion</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion ConjugateQuaternion(Quaternion quat)
        {   
            return new Quaternion(-quat.x, -quat.y, -quat.z, quat.w);
        }
        

        /// <summary>
        /// Calculate the Magnitude of a quaternion. see https://gamemath.com/book/orient.html#quaternion_magnitude
        /// equation 8.4
        /// </summary>
        /// <param name="quat">Quaternion to get the magnitude from</param>
        /// <returns>Magnitude of quaternion</returns>
        /// 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetMagnitudeQuaternion(Quaternion quat)
        {
            return Mathf.Sqrt(quat.x * quat.x + quat.y * quat.y + quat.z * quat.z + quat.w * quat.w);
        }
        /// <summary>
        /// Calculate the inverse of a given quaternion. returns a new quaternion. If quaternion is unit quaternion
        /// the conjugate is returned
        /// see https://gamemath.com/book/orient.html#quaternion_conjugate equation 8.6 for details
        /// </summary>
        /// <param name="quat"></param>
        /// <returns>The </returns>
        
        public static Quaternion InverseQuaternion(Quaternion quat)
        {
            float magnitude = GetMagnitudeQuaternion(quat);
            Quaternion conjugateQuaternion = ConjugateQuaternion(quat);
            if (magnitude == 0f || Mathf.Approximately(magnitude, 1f)) return conjugateQuaternion;
            return new Quaternion(
                conjugateQuaternion.x / magnitude,
                conjugateQuaternion.y / magnitude,
                conjugateQuaternion.z / magnitude,
                conjugateQuaternion.w / magnitude);
        }
        
        
        /// <summary>
        /// Multiply a quaternion with another. Takes two quaternions as input and returns the product of said quaternions
        /// see https://gamemath.com/book/orient.html#quaternion_cross_product for details
        /// </summary>
        /// <param name="q1">First Quaternion</param>
        /// <param name="q2">Second Quaternion</param>
        /// <returns>The product of two Quaternions</returns>
        
        // Hamilton product: result = q1 * q2
        public static Quaternion MultiplyQuaternion(Quaternion q1, Quaternion q2)
        {
            Vector3 v1 = new Vector3(q1.x, q1.y, q1.z);
            Vector3 v2 = new Vector3(q2.x, q2.y, q2.z);
            Vector3 newVector = q1.w * v2 + q2.w * v1 + CrossProduct(v1, v2);
            float w = q1.w * q2.w - DotProduct(v1, v2);
            return new Quaternion(newVector.x, newVector.y, newVector.z, w);
        }

        public static Vector3 ConvertQuaternionToEulerAngles(quaternion quat)
        {
            // lättare att läsa anser jag
            float x = quat.value.x, y = quat.value.y, z = quat.value.z, w = quat.value.w;
            float pitch = Mathf.Asin(-2 * (y * z - w * x));
            float heading = pitch != 0f ? Mathf.Atan2(x * z + w * y, (-(x * x) - (y * y)) / 2f) : Mathf.Atan2(-x * z + w * y, (-(y*y) - (z*z)) / 2f);
            // om man behöver pitch så läggs den till här!
            float bank = pitch != 0f ? 0f : 0f;
            return new Vector3(heading, pitch, bank) * Mathf.Rad2Deg;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OverlapAngleValues(ref Vector3 eulerAngles)
        {
            eulerAngles = new Vector3((eulerAngles.x + 360f) % 360f, (eulerAngles.y + 360f) % 360f, (eulerAngles.z + 360f) % 360f);
        }
    }
}