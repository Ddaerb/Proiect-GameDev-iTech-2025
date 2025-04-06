using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Unity.Cinemachine; // Using the new Cinemachine namespace

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;

    [Header("Jump Settings")]
    public float jumpForce = 120f;
    public float groundCheckDistance = 0.3f;
    public float fallMultiplier = 4f;

    [Header("Events")]
    public UnityEvent OnStartWalking;
    public UnityEvent OnStopWalking;
    public UnityEvent OnStartRunning;
    public UnityEvent OnStopRunning;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private bool autoFindCamera = true;

    [SerializeField] Rigidbody rb;
    private PlayerInput playerInput;
    private Animator animator;
    private Vector2 moveInput;
    private bool isRunning;
    private bool isGrounded;

    private InputAction jumpAction;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        // Set up Cinemachine camera
        SetupCinemachineCamera();

        jumpAction = playerInput.actions["Jump"];
        jumpAction.performed += ctx => TryJump();
    }

    void SetupCinemachineCamera()
    {
        // If no camera is assigned and auto-find is enabled, try to find one
        if (_cinemachineCamera == null && autoFindCamera)
        {
            _cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();

            if (_cinemachineCamera == null)
            {
                Debug.LogWarning("No CinemachineCamera found in the scene");
                return;
            }
        }

        // Set the camera's follow target
        if (_cinemachineCamera != null)
        {
            _cinemachineCamera.Follow = transform;
            _cinemachineCamera.LookAt = transform;
        }
    }

    void Update()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        bool shiftHeld = Keyboard.current.leftShiftKey.isPressed;
        bool wasRunning = isRunning;
        isRunning = shiftHeld && moveInput.magnitude > 0.1f;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance);
        animator.SetBool("isGrounded", isGrounded);

        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);

        if (isGrounded && rb.linearVelocity.y < 0.1f && !animator.GetCurrentAnimatorStateInfo(0).IsName("JumpLanding"))
        {
            animator.SetTrigger("Land");
        }

        animator.SetBool("IsWalking", moveInput.magnitude > 0);
        animator.SetBool("IsRunning", isRunning);

        if (moveInput.magnitude > 0)
        {
            if (!wasRunning && isRunning)
                OnStartRunning.Invoke();
            else if (wasRunning && !isRunning)
                OnStopRunning.Invoke();

            if (wasRunning && !isRunning)
                OnStopWalking.Invoke();
            if (!wasRunning && !isRunning)
                OnStartWalking.Invoke();
        }
        else
        {
            if (wasRunning)
                OnStopRunning.Invoke();
            if (!wasRunning)
                OnStopWalking.Invoke();
        }
    }

    void FixedUpdate()
    {
        MoveCharacter();

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
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

    void TryJump()
    {
        if (rb == null)
            return;

        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            SoundManager.Instance.PlaySFX(SoundManager.Instance.jumpSound);
            animator.SetTrigger("Jump");
        }
    }

    // Clean up input action when destroyed
    void OnDestroy()
    {
        if (jumpAction != null)
            jumpAction.performed -= ctx => TryJump();
    }
}
