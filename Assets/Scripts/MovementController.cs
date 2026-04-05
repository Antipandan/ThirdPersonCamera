using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private CharacterController body;
    private InputAction moveAction;
    private InputAction jumpAction;
    private float verticalSpeed;
    private bool grounded;

    [SerializeField]
    private float movementSpeed = 4.0f;

    [SerializeField]
    private float jumpHeight = 1.0f;

    [SerializeField]
    private float gravityConstant = 9.0f;

    [SerializeField]
    [Tooltip("If 0, then all momentum will be lost if the player jumps and hits the ceiling.")]
    [Range(0.0f, 1.0f)]
    private float ceilingBounce = 0.5f;

    void Start()
    {
        body = transform.GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        Transform t = transform;
        Vector2 move = moveAction.ReadValue<Vector2>();
        float hor = move.x;
        float ver = move.y;
        float amount = Mathf.Max(Mathf.Abs(hor), Mathf.Abs(ver));
        grounded = body.isGrounded;

        if (grounded && verticalSpeed > 0.0f) {
            verticalSpeed = 0.0f;
        }
    
        if (amount > float.Epsilon) {
            Vector3 dir = t.right * hor + t.forward * ver;
            dir.y = 0.0f;
            dir.Normalize();
            dir *= amount * movementSpeed * Time.deltaTime;
            body.Move(dir);
        }
    
        if (grounded && jumpAction.IsPressed()) {
            verticalSpeed = -Mathf.Sqrt(jumpHeight * 3.0f * gravityConstant);
        }
    
        verticalSpeed += gravityConstant * Time.deltaTime;

        CollisionFlags collisionFlags = body.Move(Vector3.down * (verticalSpeed * Time.deltaTime));
        if ((collisionFlags & CollisionFlags.Above) != 0) {
            verticalSpeed *= -ceilingBounce; // Bounce against the ceiling
        }
    }
}
