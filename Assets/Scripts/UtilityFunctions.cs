using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using UnityEngine;

namespace Utility
{
    public static class UtilityFunctions
    {
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
            return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
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
        /// Rotate a position around a given Quaternion. Does not rotate around a custom pivot point.
        /// Pivot point is origo in worldspace. In order to rotate around a pivot point subtract the position of the object to be rotated,
        /// with the position of the pivot point. Add the position of the pivot point to the return value of this function
        /// </summary>
        /// <param name="rotationQuaternion"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector3 RotatePosition(Quaternion rotationQuaternion, Vector3 position)
        {
            Quaternion quatPosition = new Quaternion(position.x, position.y, position.z, 0f);
            Quaternion inverseQuaternion = InverseQuaternion(rotationQuaternion);
            Quaternion rotatedPosition = MultiplyQuaternion(FastMultiplyQuaternion(rotationQuaternion, quatPosition), inverseQuaternion);

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
        /// <param name="quat">Quaternion to turn into the inverse of itself</param>
        /// <returns>The Inverse of a given Quaternion</returns>
        
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
        
        public static Quaternion RelativeQuaternion(Quaternion q1, Quaternion q2)
        {
            return MultiplyQuaternion(q2, inverse(q1));
        }
        
        /// <summary>
        /// Multiply a quaternion with another. Takes two quaternions as input and returns the product of said quaternions
        /// see https://gamemath.com/book/orient.html#quaternion_cross_product for details
        /// </summary>
        /// <param name="q1">First Quaternion</param>
        /// <param name="q2">Second Quaternion</param>
        /// <returns>The Hamilton product of two Quaternions</returns>
        public static Quaternion MultiplyQuaternion(Quaternion q1, Quaternion q2)
        {
            Vector3 v1 = new Vector3(q1.x, q1.y, q1.z);
            Vector3 v2 = new Vector3(q2.x, q2.y, q2.z);
            Vector3 newVector = q1.w * v2 + q2.w * v1 + CrossProduct(v1, v2);
            float w = q1.w * q2.w - DotProduct(v1, v2);
            return new Quaternion(newVector.x, newVector.y, newVector.z, w);
        }

        /// <summary>
        /// Multiply a quaternion with another. Takes two quaternions as input and returns the product of said quaternions
        /// see https://gamemath.com/book/orient.html#quaternion_cross_product for details. Assumes either
        /// one of the w component is equal to zero. Thus, Operation requires fewer instructions
        /// </summary>
        /// <param name="q1">First Quaternion</param>
        /// <param name="q2">Second Quaternion</param>
        /// <returns>The Hamilton product of two Quaternions</returns>
        public static Quaternion FastMultiplyQuaternion(Quaternion q1, Quaternion q2)
        {
            Vector3 v1 = new Vector3(q1.x, q1.y, q1.z);
            Vector3 v2 = new Vector3(q2.x, q2.y, q2.z);
            Vector3 newVector = q1.w * v2 + q2.w * v1 + CrossProduct(v1, v2);
            return new Quaternion(newVector.x, newVector.y, newVector.z, -DotProduct(v1, v2));
        }
        
        /// <summary>
        /// Converts EulerAngles to Quaternion. Takes a Vector3 containing
        /// the euler angles expressed as follows: Vector3(Heading, Pitch, Bank)
        /// </summary>
        /// <param name="euler">Vector3(Heading, Pitch, Bank)</param>
        /// <returns>Quaternion converted from provided Euler angle</returns>
        public static Quaternion ConvertEulerToQuaternion(Vector3 euler)
        {
            float heading = euler.x * Mathf.Deg2Rad;
            float pitch = euler.y *  Mathf.Deg2Rad;
            float bank = euler.z * Mathf.Deg2Rad;
            return new Quaternion(
                cos(heading / 2f) * sin(pitch / 2f) * cos(bank / 2f) + sin(heading / 2f) * cos(pitch / 2f) * sin(bank / 2f),
                sin(heading / 2f) * cos(pitch / 2f) * cos(bank / 2f) - cos(heading / 2f) * sin(pitch / 2f) * sin(bank / 2f),
                cos(heading / 2f) * cos(pitch / 2f) * sin(bank / 2f) - sin(heading / 2f) * sin(pitch / 2f) * cos(bank / 2f), 
                cos(heading / 2f) * cos(pitch / 2f) * cos(bank / 2f) + sin(heading / 2f) * sin(bank / 2f) * sin(bank / 2f));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PreventFunctionsRunningInEditor(params Action[] functionsToRun)
        {
            foreach (Action function in functionsToRun)
            {
                if (UnityEditor.EditorApplication.isPlaying) function();
            }
        }

    }
}