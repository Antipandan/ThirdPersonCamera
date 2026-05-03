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
            // MouseLook();
        }

        using (vectorRenderer.Begin())
        {
            vectorRenderer.Draw(objectToRotateAround.position, objectToRotateAround.position + currentMouseLookingDirection, Color.yellow);
        }
    }

    private void MouseLook()
    {
        Vector2 look = lookAction.ReadValue<Vector2>() * (Time.deltaTime * 100f);
        if (look == Vector2.zero) return;
        // float hor = (look.x) * Time.deltaTime * 100f;
        // float ver = (look.y) * Time.deltaTime * 100f;
        rotationAngle = UtilityFunctions.GetMagnitudeOfVector(look);
        look = UtilityFunctions.NormalizeVector(look);
        Debug.Log($"look: {look}");
        currentMouseLookingDirection = new Vector3(look.x, look.y,0f);
        UtilityFunctions.ConvertMouseVectorToQuaternionValue(15f * Time.deltaTime, currentMouseLookingDirection, ref rotationQuaternion);
        // Debug.Log(relativeEulerAngles);
        quaternion rotatedQuaternion = UtilityFunctions.RotateAroundQuaternion(rotationQuaternion, gameObject.transform.position - objectToRotateAround.position);
        // Debug.Log($"rotated quaternion: {rotatedQuaternion}");
        gameObject.transform.position = new Vector3(rotatedQuaternion.value.x, rotatedQuaternion.value.y, rotatedQuaternion.value.z) + objectToRotateAround.position;

    }
    
    
}
