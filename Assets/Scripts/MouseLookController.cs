using System;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using Utility;
using Vectors;

[Serializable]
public class MouseLookController : MonoBehaviour, IPauseable
{
    [SerializeField]
    private Transform objectToRotateAround;
    
    [SerializeField]
    private float mouseSpeed = 1.0f;
    
    [SerializeField]
    private Vector2 defaultLookingDirection = new Vector2(1f, 1f);

    [SerializeField] [Range(0f, 90f)] private float maxPitch = 80f;
    [SerializeField] [Range(0f, 180f)] private float maxHeading = 90f;
    
    private float rotationAngle;
    private InputAction lookAction;
    private bool isPaused;
    private VectorRenderer vectorRenderer;
    private Vector3 currentMouseLookingDirection;
    private Vector3 objectLookAroundPosition;
    private float pitch;
    private float heading;
    private Vector3 startingPosition;
    
    
    public bool IsPaused => isPaused;

    private void OnEnable()
    {
        vectorRenderer = GetComponent<VectorRenderer>();
    }
    
    private void Awake()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        if (objectToRotateAround != null) objectLookAroundPosition = objectToRotateAround.position;
        else objectLookAroundPosition = new Vector3(0f, 0f, 0f);
        startingPosition = transform.position;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Pause()
    {
        isPaused ^= true;
    }

    private void Update()
    {
        if (!isPaused)
        {
            MouseLook();
        }

        using (vectorRenderer.Begin())
        {
            vectorRenderer.Draw(objectLookAroundPosition, objectLookAroundPosition + currentMouseLookingDirection, Color.yellow);
        }
    }

    private void MouseLook()
    {
        Vector2 look = lookAction.ReadValue<Vector2>() * (Time.deltaTime * mouseSpeed * 10f);
        float rotationAmount = UtilityFunctions.GetMagnitudeOfVector(look);
        UpdateRotationAngles(look);
        Vector2 normalizedLook = UtilityFunctions.NormalizeVector(look);
        currentMouseLookingDirection = new Vector3(0f, -normalizedLook.x, normalizedLook.y);
        // Quaternion xRotationQuaternion = UtilityFunctions.AxisAngleQuaternion(currentMouseLookingDirection, rotationAmount);
        Quaternion rotation = Quaternion.Euler(heading, pitch, 0f);
        Vector3 newPosition = UtilityFunctions.RotatePosition(rotation, startingPosition - objectLookAroundPosition);
        // gameObject.transform.rotation = rotation;
        gameObject.transform.position = newPosition + objectLookAroundPosition;
    }

    private void UpdateRotationAngles()
    {
        pitch = Mathf.Clamp(pitch, -0.5f * maxPitch, 0.5f * maxPitch);
        heading = Mathf.Clamp(heading, -0.5f * maxHeading, 0.5f * maxHeading);
        Debug.Log($"heading: {heading}, pitch: {pitch}");
    }

    private void UpdateRotationAngles(Vector2 mouseVector)
    {
        pitch -= mouseVector.x;
        heading += mouseVector.y;
        UpdateRotationAngles();
    }
    
}
