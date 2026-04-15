using System;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

[Serializable]
public class MouseLookController : MonoBehaviour, IPauseable
{
    [SerializeField]
    private Transform head;
    
    [SerializeField]
    private Transform body;
    
    [SerializeField]
    private float mouseSpeed = 1.0f;

    [SerializeField, Range(0, 180)]
    private float verticalAngle = 160;

    [SerializeField] 
    private CustomEvents customEvents;
    
    private InputAction lookAction;
    private bool isPaused;
    
    
    public bool IsPaused => isPaused;

    private void Awake()
    {
        return;
    }

    private void Start()
    {
        if (head == null) head = gameObject.GetComponentInChildren<Camera>().transform;
        if (body == null) body = gameObject.transform;
        lookAction = InputSystem.actions.FindAction("Look");
    }
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Pause()
    {
        // xor
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

        if (Mathf.Abs(hor) > float.Epsilon) 
        {
            body.Rotate(Vector3.up, hor * mouseSpeed);
        }

        if (Mathf.Abs(ver) > float.Epsilon) 
        {
            Quaternion rot = head.localRotation;
            Quaternion aim = Quaternion.AngleAxis(-0.5f * verticalAngle * Mathf.Sign(ver), Vector3.right);
            Quaternion delta = Quaternion.RotateTowards(rot, aim, Mathf.Sign(ver) * ver * mouseSpeed);
            head.localRotation = delta;
        }
    }
    
}
