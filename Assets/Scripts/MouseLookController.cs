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
    
    private InputAction lookAction;
    private bool isPaused;
    private VectorRenderer vectorRenderer;
    private Vector3 currentMouseLookingDirection;
    private Vector3 objectLookAroundPosition;
    private float totalPitchRotation = 0f;
    private float totalHeadingRotation = 0f;
    private Vector3 startingPosition;
    private Vector3 lastCameraRotation;
    
    
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
        startingPosition = gameObject.transform.position;
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
            // vectorRenderer.Draw(objectLookAroundPosition, objectLookAroundPosition + currentMouseLookingDirection, Color.yellow);
        }
    }

    private void MouseLook()
    {
        Vector2 look = lookAction.ReadValue<Vector2>() * (Time.deltaTime * 100f);
        float rotationAmount = UtilityFunctions.GetMagnitudeOfVector(look);
        float headingRotation = look.x;
        float pitchRotation = look.y;
        look = UtilityFunctions.NormalizeVector(look);
        currentMouseLookingDirection = new Vector3(-look.y * mouseSpeed, 0f, 0f);
        Quaternion yRotationQuaternion = UtilityFunctions.AxisAngleQuaternion(currentMouseLookingDirection, pitchRotation);
        currentMouseLookingDirection = new Vector3(0f, -look.x * mouseSpeed, 0f);
        Debug.Log($"heading rotation: {headingRotation}");
        Quaternion xRotationQuaternion = UtilityFunctions.AxisAngleQuaternion(currentMouseLookingDirection, headingRotation);
        Quaternion combinedRotationQuaternion = UtilityFunctions.MultiplyQuaternion(xRotationQuaternion, yRotationQuaternion);
        Vector3 newPosition = UtilityFunctions.RotatePosition(xRotationQuaternion, gameObject.transform.position - objectLookAroundPosition);
        gameObject.transform.position = newPosition + objectLookAroundPosition;
    }
    
    
}
