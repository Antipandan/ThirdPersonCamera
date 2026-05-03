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

    
    void Update()
    {
        Vector3 rotationVector = new Vector3(1f, 0f, 0f).normalized;
        Quaternion rotationQuaternion = UtilityFunctions.AxisAngleQuaternion(rotationVector, angle);
        Vector3 rotated = UtilityFunctions.RotatePosition(rotationQuaternion, startingPosition);
        startingPosition = rotated;
        gameObject.transform.position = startingPosition;
    }
}


