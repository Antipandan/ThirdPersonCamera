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
    
    
    public bool IsPaused => isPaused;

    private void OnEnable()
    {
        vectorRenderer = GetComponent<VectorRenderer>();
    }
    
    private void Awake()
    {
        lookAction = InputSystem.actions.FindAction("Look");
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
            vectorRenderer.Draw(objectToRotateAround.position, objectToRotateAround.position + currentMouseLookingDirection, Color.yellow);
        }
    }

    private void MouseLook()
    {
        Vector2 look = lookAction.ReadValue<Vector2>() * (Time.deltaTime * 100f);
        look = UtilityFunctions.NormalizeVector(look);
        currentMouseLookingDirection = new Vector3(look.x, look.y, 0f) * mouseSpeed;
        float rotationAmount = UtilityFunctions.GetMagnitudeOfVector(look);
        Quaternion rotationQuaternion = UtilityFunctions.AxisAngleQuaternion(currentMouseLookingDirection, rotationAmount);
        Vector3 newPosition = UtilityFunctions.RotatePosition(rotationQuaternion, gameObject.transform.position);
        gameObject.transform.position = newPosition;
    }
    
    
}
