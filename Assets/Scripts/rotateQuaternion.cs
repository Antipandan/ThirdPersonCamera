using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using Utility;

public class rotateQuaternion : MonoBehaviour
{

    private readonly float angle = 5f; // degrees
    private Vector3 startingPosition = new Vector3(0f, 0f, 0f);

    private void Awake()
    {
        startingPosition = gameObject.transform.position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotationVector = new Vector3(1f, 0f, 0f).normalized;
        Quaternion rotationQuaternion = QuaternionUtils.AxisAngleQuaternion(rotationVector, angle);
        Vector3 rotated = QuaternionUtils.RotateVector(rotationQuaternion, startingPosition);
        Debug.Log($"rotated: {rotated}");
        startingPosition = rotated;
        gameObject.transform.position = startingPosition;
    }
    
}


public static class QuaternionUtils
{
    // Build a unit quaternion from axis (assumed non-zero) and angle in degrees
    public static Quaternion AxisAngleQuaternion(Vector3 axis, float angleDegrees)
    {
        float angleRad = angleDegrees * Mathf.Deg2Rad * 0.5f;
        axis = axis.normalized;
        float s = Mathf.Sin(angleRad);
        float c = Mathf.Cos(angleRad);
        // Quaternion(x, y, z, w)
        return new Quaternion(axis.x * s, axis.y * s, axis.z * s, c);
    }

    // Hamilton product: result = a * b
    public static Quaternion MultiplyQuaternion(Quaternion q1, Quaternion q2)
    {
        Vector3 v1 = new Vector3(q1.x, q1.y, q1.z);
        Vector3 v2 = new Vector3(q2.x, q2.y, q2.z);
        Vector3 newVector = q1.w * v2 + q2.w * v1 + UtilityFunctions.CrossProduct(v1, v2);
        float w = q1.w * q2.w - UtilityFunctions.DotProduct(v1, v2);
        return new Quaternion(newVector.x, newVector.y, newVector.z, w);
    }

    // Conjugate (for unit quaternions inverse = conjugate)
    public static Quaternion ConjugateQuaternion(Quaternion quat)
    {
        return new Quaternion(-quat.x, -quat.y, -quat.z, quat.w);
    }

    // Rotate a Vector3 v by quaternion q using p' = q * p * q_conj
    public static Vector3 RotateVector(Quaternion q, Vector3 v)
    {
        // Ensure q is normalized (to avoid scaling)
        q = Normalize(q);

        // p as pure quaternion
        Quaternion p = new Quaternion(v.x, v.y, v.z, 0f);

        // q * p
        Quaternion qp = MultiplyQuaternion(q, p);
        // qp * q_conj
        Quaternion qConj = ConjugateQuaternion(q);
        Quaternion res = MultiplyQuaternion(qp, qConj);

        return new Vector3(res.x, res.y, res.z);
    }

    // Normalize quaternion
    public static Quaternion Normalize(Quaternion q)
    {
        float mag = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
        if (mag > 1e-6f)
        {
            float inv = 1f / mag;
            return new Quaternion(q.x * inv, q.y * inv, q.z * inv, q.w * inv);
        }
        return new Quaternion(0f, 0f, 0f, 1f);
    }
}
