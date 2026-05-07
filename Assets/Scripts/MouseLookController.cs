using System;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using Vectors;

[Serializable]
public class MouseLookController : MonoBehaviour, IPauseable
{
    [SerializeField]
    private CustomEvents events;
    
    [SerializeField]
    private Transform objectToRotateAround;
    
    [SerializeField]
    private float mouseSpeed = 1.0f;

    [SerializeField] private bool allowLimitLessPitch = false;
    [SerializeField] [Range(0f, 90f)] private float maxPitch = 80f;
    [SerializeField] private bool allowLimitLessHeading = false;
    [SerializeField] [Range(0f, 180f)] private float maxHeading = 90f;

    [Header("Additional options")] 
    [SerializeField] private bool rotateObject = true;
    
    [SerializeField]
    private Vector3 startingPosition;
    
    
    private Vector3 currentMouseLookingDirection;
    private Vector3 objectLookAroundPosition;
    private VectorRenderer vectorRenderer;
    private const float tolerance = 1e-6f;
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
        objectLookAroundPosition = objectToRotateAround != null ? objectToRotateAround.position : new Vector3(0f, 0f, 0f);
        startingPosition = transform.position;
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
        Vector2 look = lookAction.ReadValue<Vector2>() * (Time.deltaTime * mouseSpeed * 10f);
        if (look != Vector2.zero)
        {
            UpdateRotationAngles(look);
            Vector2 normalizedLook = UtilityFunctions.NormalizeVector(look);
            currentMouseLookingDirection = new Vector3(normalizedLook.y, -normalizedLook.x, 0f);
            Quaternion rotation = UtilityFunctions.ConvertEulerToQuaternion(new Vector3(heading, pitch, 0f));
            Vector3 newPosition = UtilityFunctions.RotatePosition(rotation, startingPosition - objectLookAroundPosition);
            gameObject.transform.position = newPosition + objectLookAroundPosition;
            // jag tänker inte lista ut det här själv ok! 
            if (rotateObject) gameObject.transform.rotation = rotation;
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
        if (!allowLimitLessHeading) heading = Mathf.Clamp(heading, -0.5f * maxHeading + tolerance, 0.5f * maxHeading - tolerance);
        if (!allowLimitLessPitch) pitch = Mathf.Clamp(pitch, -0.5f * maxPitch + tolerance, 0.5f * maxPitch - tolerance);
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
        Debug.Log($"validate!");
    }
}
