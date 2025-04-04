using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private Animator animator;
    private Vector2 moveInput;
    private bool isRunning;

    public UnityEvent OnStartWalking;
    public UnityEvent OnStopWalking;
    public UnityEvent OnStartRunning;
    public UnityEvent OnStopRunning;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        bool shiftHeld = Keyboard.current.leftShiftKey.isPressed;

        bool wasRunning = isRunning;

        isRunning = shiftHeld && moveInput.magnitude > 0.1f;

        animator.SetBool("IsWalking", moveInput.magnitude > 0);
        animator.SetBool("IsRunning", isRunning);

        if (moveInput.magnitude > 0)
        {
            if (!wasRunning && isRunning)
            {
                OnStartRunning.Invoke();
            }
            else if (wasRunning && !isRunning)
            {
                OnStopRunning.Invoke();
            }

            if (wasRunning && !isRunning)
            {
                OnStopWalking.Invoke();
            }
            if (!wasRunning && !isRunning)
            {
                OnStartWalking.Invoke();
            }
        }
        else
        {
            if (wasRunning)
            {
                OnStopRunning.Invoke();
            }
            if (!wasRunning)
            {
                OnStopWalking.Invoke();
            }
        }
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    void MoveCharacter()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        if (moveDirection.magnitude > 0)
        {
            float speed = isRunning ? runSpeed : walkSpeed;

            rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
