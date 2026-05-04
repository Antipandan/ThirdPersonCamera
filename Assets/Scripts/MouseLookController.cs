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
    
    private float rotationAngle;
    private InputAction lookAction;
    private bool isPaused;
    private VectorRenderer vectorRenderer;
    private Vector3 currentMouseLookingDirection;
    private Vector3 objectLookAroundPosition;
    
    
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
        Vector2 look = lookAction.ReadValue<Vector2>() * (Time.deltaTime * 100f);
        look = UtilityFunctions.NormalizeVector(look);
        currentMouseLookingDirection = new Vector3(-look.y * mouseSpeed, 0f, 0f);
        float rotationAmount = UtilityFunctions.GetMagnitudeOfVector(look);
        Quaternion yRotationQuaternion = UtilityFunctions.AxisAngleQuaternion(currentMouseLookingDirection, rotationAmount);
        currentMouseLookingDirection = new Vector3(0f, -look.x * mouseSpeed, 0f);
        Quaternion xRotationQuaternion = UtilityFunctions.AxisAngleQuaternion(currentMouseLookingDirection, rotationAmount);
        // Quaternion combinedRotationQuaternion = UtilityFunctions.MultiplyQuaternion(yRotationQuaternion, xRotationQuaternion);
        Vector3 newPosition = UtilityFunctions.RotatePosition(xRotationQuaternion, gameObject.transform.position);
        gameObject.transform.position = newPosition;
    }
    
    
}
