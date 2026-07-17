using System;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utility;
using Vectors;

[Serializable]
public class MouseLookController : MonoBehaviour, IPauseable
{
    [SerializeField]
    private CustomEvents events;
    
    [Header("Settings")] [SerializeField]
    private Transform objectToRotateAround;
    
    [SerializeField] private float mouseSpeed = 1.0f;

    [SerializeField] private bool useStaticMouseMovement = false;
    [SerializeField] private Vector2 staticMouseMovement;
    
    [SerializeField] private bool allowLimitLessPitch = false;
    [SerializeField] [Range(0f, 90f)] private float maxPitch = 80f;
    
    [SerializeField] private bool allowLimitLessHeading = false;
    [SerializeField] [Range(0f, 180f)] private float maxHeading = 90f;
    
    [SerializeField] private bool lookTowardsRotationPoint = true;
    
    [SerializeField] private Vector3 startingPosition;
    
    private Vector3 currentMouseLookingDirection;
    private Vector3 objectLookAroundPosition;
    private VectorRenderer vectorRenderer;
    private const float Tolerance = 1e-6f;
    private InputAction lookAction;
    private bool isPaused;
    private float heading;
    private float pitch;

    
    public bool IsPaused => isPaused;

    private void OnEnable()
    {
        vectorRenderer = GetComponent<VectorRenderer>();
    }
    
    private void Awake()
    {
        if (events == null)
        {
            Debug.LogError($"Reference to {nameof(CustomEvents)} in GameObject '{gameObject.name}'" +
                           $" is missing in inspector. A reference is required for the game to start", gameObject);
            throw new ArgumentNullException(nameof(events));
        }
        lookAction = InputSystem.actions.FindAction("Look");
        SetupValues();
    }

    private void SetupValues()
    {
        objectLookAroundPosition = objectToRotateAround != null ? objectToRotateAround.position : new Vector3(0f, 0f, 0f);
        startingPosition = gameObject.transform.position;
        SetRelativeAngles();
    }

    private void SetRelativeAngles()
    {
        heading = 0f;
        pitch = 0f;
    }

    private void SubscribeToEvents()
    {
        events.OnMouseSpeedChanged += ChangeMouseSpeed;
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
        Vector2 look = staticMouseMovement;
        if (!useStaticMouseMovement) look = lookAction.ReadValue<Vector2>();
        look *= Time.deltaTime * mouseSpeed * 10f;
        if (look != Vector2.zero)
        {
            UpdateRotationAngles(look);
            Vector2 normalizedLook = UtilityFunctions.NormalizeVector(look);
            currentMouseLookingDirection = new Vector3(normalizedLook.x, normalizedLook.y, 0f);
            Quaternion rotationX = UtilityFunctions.AngleAxisQuaternion(heading, Vector3.up);
            Quaternion rotationY = UtilityFunctions.AngleAxisQuaternion(pitch, gameObject.transform.right);
            Quaternion rotation = rotationY * rotationX;
            Vector3 newPosition = UtilityFunctions.RotatePosition(rotation, startingPosition - objectLookAroundPosition);
            gameObject.transform.position = newPosition + objectLookAroundPosition;
            // jag tänker inte lista ut det här själv ok?
            if (lookTowardsRotationPoint)
            {
                transform.LookAt(objectLookAroundPosition);
            }
        }
    }
    
    
    private void UpdateRotationAngles(Vector2 mouseVector)
    {
        heading += mouseVector.x;
        pitch -= mouseVector.y;
        UpdateRotationAngles();
    }

    private void UpdateRotationAngles()
    {
        if (!allowLimitLessHeading) heading = Mathf.Clamp(heading, -0.5f * maxHeading + Tolerance, 0.5f * maxHeading - Tolerance);
        if (!allowLimitLessPitch) pitch = Mathf.Clamp(pitch, -0.5f * maxPitch + Tolerance, 0.5f * maxPitch - Tolerance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ChangeMouseSpeed(string newMouseSpeed)
    {
        float.TryParse(newMouseSpeed, out mouseSpeed);
    }

    public void ChangeCameraPosition()
    {
        startingPosition = gameObject.transform.position;
    }

    private void OnValidate()
    {
        gameObject.transform.position = startingPosition;
        UtilityFunctions.PreventFunctionsRunningInEditor(SetupValues);
    }
}
