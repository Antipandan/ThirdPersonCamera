using System;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using Utility;

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
        Vector2 look = lookAction.ReadValue<Vector2>();
        if (look == Vector2.zero) look = Vector2.zero;
        float hor = look.x;
        float ver = look.y;
        
        UtilityFunctions.ModifyVector2(hor, ver, ref currentLookingDirection);
        rotationAngle = UtilityFunctions.GetMagnitudeOfVector(currentLookingDirection);
        currentLookingDirection = UtilityFunctions.NormalizeVector(currentLookingDirection);
        Vector3 mouseLookingDirection = new Vector3(0f, currentLookingDirection.x, currentLookingDirection.y);
        UtilityFunctions.ConvertMouseVectorToQuaternionValue(1f, mouseLookingDirection, ref rotationQuaternion);
        //quaternion deltaQuaternion = new quaternion(deltaPosition.x, deltaPosition.y, deltaPosition.z, 0f);
        RotateAroundQuaternion();
    }

    private void RotateAroundQuaternion()
    {
        Vector3 deltaPosition = gameObject.transform.position - objectToRotateAround.position;
        quaternion positionQuaternion = new quaternion(deltaPosition.x, deltaPosition.y, deltaPosition.z, 0f);
        quaternion rotatedQuaternion = UtilityFunctions.MultiplyQuaternion(
            UtilityFunctions.MultiplyQuaternion(rotationQuaternion, positionQuaternion),
            UtilityFunctions.InverseQuaternion(rotationQuaternion));
        gameObject.transform.position = new Vector3(rotatedQuaternion.value.x, rotatedQuaternion.value.y, rotatedQuaternion.value.z) + objectToRotateAround.position;
    }
    
}
