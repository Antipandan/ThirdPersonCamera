using System;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

[Serializable]
public class MouseLookController : MonoBehaviour, IPauseable
{
    [SerializeField]
    private Transform objectToFollow;
    
    [SerializeField]
    private float mouseSpeed = 1.0f;

    [SerializeField, Range(0, 180)]
    private float verticalAngle = 160;

    [SerializeField] 
    private CustomEvents customEvents;

    [SerializeField]
    private Vector2 defaultLookingDirection = new Vector2(1f, 1f);
    
    private float rotationAngle;
    private Vector2 currentLookingDirection;
    private InputAction lookAction;
    private quaternion rotationQuaternion;
    private bool isPaused;
    
    
    public bool IsPaused => isPaused;
    
    private void Awake()
    {
        rotationQuaternion = new quaternion(0f, 0f, 0f, 0f);
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
    }

    private void MouseLook()
    {
        Vector2 look = lookAction.ReadValue<Vector2>() * 0.1f;
        float hor = look.x;
        float ver = look.y;
        UtilityFunctions.ModifyVector2(hor, ver, ref currentLookingDirection);
        rotationAngle = UtilityFunctions.GetMagnitudeOfVector(currentLookingDirection);
        //Debug.Log($"current looking direction is: {currentLookingDirection}");
        
        if (Mathf.Abs(hor) > float.Epsilon) 
        {
        }

        if (Mathf.Abs(ver) > float.Epsilon) 
        {
        }
    }
    
}
