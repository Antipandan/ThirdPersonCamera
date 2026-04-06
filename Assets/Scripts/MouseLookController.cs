using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class MouseLookController : MonoBehaviour, IPauseable
{
    private Transform head;
    private Transform body;
    private InputAction lookAction;
    private bool isPaused;

    [SerializeField]
    private float mouseSpeed = 1.0f;

    [SerializeField, Range(0, 180)]
    private float verticalAngle = 160;
    
    public bool IsPaused => isPaused;

    private void Start()
    {
        head = GetComponentInChildren<Camera>().transform;
        body = transform;
        lookAction = InputSystem.actions.FindAction("Look");
        
    }

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
            int rotationSign = UtilityFunctions.GetSign(hor);
            Quaternion currentHeadRotation = head.localRotation;
            Quaternion maxRotation = Quaternion.AngleAxis(0.5f * verticalAngle * rotationSign, Vector3.up);
            Quaternion angleToRotate = Quaternion.RotateTowards(currentHeadRotation, maxRotation, rotationSign * hor * mouseSpeed);
            head.localRotation = angleToRotate;
        }
        
        /*
         *         if (Mathf.Abs(ver) > float.Epsilon) 
        {
            Quaternion rot = head.localRotation;
            Quaternion aim = Quaternion.AngleAxis(-0.5f * verticalAngle * Mathf.Sign(ver), Vector3.right);
            Quaternion delta = Quaternion.RotateTowards(rot, aim, Mathf.Sign(ver) * ver * mouseSpeed);
            delta.eulerAngles = new Vector3(delta.eulerAngles.x, delta.eulerAngles.y, 0);
            head.localRotation = delta;
        }
         */
    }
    
}
