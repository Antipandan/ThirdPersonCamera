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
    private Vector2 currentLookingDirection;
    private InputAction lookAction;
    private quaternion rotationQuaternion;
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

        using (vectorRenderer.Begin())
        {
            vectorRenderer.Draw(objectToRotateAround.position, objectToRotateAround.position + currentMouseLookingDirection, Color.yellow);
        }
    }

    private void MouseLook()
    {
        Vector2 look = lookAction.ReadValue<Vector2>();
        if (look == Vector2.zero) look = Vector2.zero;
        float hor = (look.x) * Time.deltaTime * 100f;
        float ver = (look.y) * Time.deltaTime * 100f;
        
        UtilityFunctions.ModifyVector2(hor, ver, ref currentLookingDirection);
        rotationAngle = UtilityFunctions.GetMagnitudeOfVector(currentLookingDirection);
        currentLookingDirection = UtilityFunctions.NormalizeVector(currentLookingDirection);
        currentMouseLookingDirection= new Vector3(0f, currentLookingDirection.x, currentLookingDirection.y);
        UtilityFunctions.ConvertMouseVectorToQuaternionValue(rotationAngle, currentMouseLookingDirection, ref rotationQuaternion);
        quaternion rotatedQuaternion = UtilityFunctions.RotateAroundQuaternion(rotationQuaternion, gameObject.transform.position - objectToRotateAround.position);
        // gameObject.transform.position = new Vector3(rotatedQuaternion.value.x, rotatedQuaternion.value.y, rotatedQuaternion.value.z) + objectToRotateAround.position;
    }
    
    
}
