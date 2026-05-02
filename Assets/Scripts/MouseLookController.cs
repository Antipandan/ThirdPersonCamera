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
            vectorRenderer.Draw(objectToRotateAround.position, objectToRotateAround.position + new Vector3(0f, 1f, 0f).normalized, Color.yellow);
        }
    }

    private void MouseLook()
    {
        Vector2 look = lookAction.ReadValue<Vector2>() * (Time.deltaTime * 100f);
        if (look == Vector2.zero) look = Vector2.zero;
        // float hor = (look.x) * Time.deltaTime * 100f;
        // float ver = (look.y) * Time.deltaTime * 100f;
        Vector3 relativeEulerAngles = new Vector3();
        
        rotationAngle = UtilityFunctions.GetMagnitudeOfVector(look);
        look = UtilityFunctions.NormalizeVector(look);
        Debug.Log($"look: {look}");
        currentMouseLookingDirection = new Vector3(look.y, look.x,0f);
        UtilityFunctions.ConvertMouseVectorToQuaternionValue(180f, new Vector3(0f, 0f, 0f), ref rotationQuaternion);
        Debug.Log($"rotationQuaternion: {rotationQuaternion}");
        relativeEulerAngles = UtilityFunctions.ConvertQuaternionToEulerAngles(rotationQuaternion);
        UtilityFunctions.OverlapAngleValues(ref relativeEulerAngles);
        // Debug.Log(relativeEulerAngles);
        quaternion rotatedQuaternion = UtilityFunctions.RotateAroundQuaternion(rotationQuaternion, gameObject.transform.position - objectToRotateAround.position);
        gameObject.transform.position = new Vector3(rotatedQuaternion.value.x, rotatedQuaternion.value.y, rotatedQuaternion.value.z) + objectToRotateAround.position;
    }
    
    
}
