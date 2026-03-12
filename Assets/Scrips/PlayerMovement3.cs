using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement3 : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float strafeSpeed = 10f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 1.3f;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 90f;

    [Header("Jump + Gravity")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private int maxJumps = 2;

    [Header("Forgiveness Timers")]
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    [Header("Variable Jump Height")]
    [SerializeField] private float jumpCutMultiplier = 1f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 50f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.6f;
    [SerializeField] private int maxDashes = 1;

    [Header("Ground Check (Custom)")]
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float groundCheckDistance = 0.08f;
    [SerializeField] private float maxGroundAngle = 55f;

    [Header("Anti-Sticky Grounding")]
    [SerializeField] private float groundedLockTimeAfterJump = 0.08f;

    private Rigidbody rb; // not used for movement with CharacterController (safe to remove Rigidbody component)
    private int count;
    public TextMeshProUGUI countText;

    private CharacterController controller;

    // Input Layer
    private Vector3 moveInput;
    private float mouseX;
    private float mouseY;
    private bool jumpPressedThisFrame;
    private bool jumpReleasedThisFrame;
    private bool jumpHeld;
    private bool dashPressedThisFrame;

    // State Layer
    private float pitch;
    private int jumpsUsed;
    private float coyoteTimer;
    private float jumpBufferTimer;

    // Dash state
    private float dashTimeRemaining;
    private float dashCooldownRemaining;
    private Vector3 dashDirection;
    private int dashesRemaining;

    // Ground state
    private bool isGroundedReal;
    private float groundedLockTimer;

    // Force Layer
    private float verticalVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();

        controller = GetComponent<CharacterController>();
        dashesRemaining = maxDashes;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ReadInput();
        UpdateLook();

        // lockout timer ticks down
        if (groundedLockTimer > 0f)
            groundedLockTimer -= Time.deltaTime;

        HandleDash();
        HandleJumpRequests();
        ApplyGravity();
        ApplyMovement();

        // IMPORTANT: ground check after moving so it reflects current position
        UpdateGroundedReal();
        UpdateTimersAndGround();
    }

    // Input Layer
    private void ReadInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        jumpPressedThisFrame = Input.GetButtonDown("Jump");
        jumpReleasedThisFrame = Input.GetButtonUp("Jump");
        jumpHeld = Input.GetButton("Jump");

        dashPressedThisFrame = Input.GetMouseButtonDown(1);
    }

    // Custom grounded check: spherecast downward and reject steep surfaces
    private void UpdateGroundedReal()
    {
        // If we just jumped, or we're moving upward, don't allow "grounded"
        if (groundedLockTimer > 0f || verticalVelocity > 0.05f)
        {
            isGroundedReal = false;
            return;
        }

        Bounds b = controller.bounds;

        // Slightly smaller than controller radius to avoid side hits
        float radius = controller.radius * 0.95f;

        // Start a bit above the bottom of the capsule
        Vector3 origin = new Vector3(b.center.x, b.min.y + radius + 0.02f, b.center.z);

        // Cast down a small distance
        float castDist = groundCheckDistance + 0.02f;

        bool hitGround = Physics.SphereCast(
            origin,
            radius,
            Vector3.down,
            out RaycastHit hit,
            castDist,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        if (!hitGround)
        {
            isGroundedReal = false;
            return;
        }

        float angle = Vector3.Angle(hit.normal, Vector3.up);
        isGroundedReal = angle <= maxGroundAngle;
    }

    // State Layer (ground + timers + jump count reset)
    private void UpdateTimersAndGround()
    {
        bool grounded = isGroundedReal;

        // Only treat as landed if we're not moving upward (prevents mid-air resets)
        if (grounded && verticalVelocity <= 0.05f)
        {
            // Keeps controller stuck to ground when actually grounded
            if (verticalVelocity < 0f)
                verticalVelocity = -2f;

            jumpsUsed = 0;
            coyoteTimer = coyoteTime;

            dashesRemaining = maxDashes;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (jumpPressedThisFrame)
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;
    }

    // Dash (timers + direction)
    private void HandleDash()
    {
        if (dashCooldownRemaining > 0f)
            dashCooldownRemaining -= Time.deltaTime;

        if (dashTimeRemaining > 0f)
            dashTimeRemaining -= Time.deltaTime;

        if (!dashPressedThisFrame)
            return;

        if (dashTimeRemaining > 0f || dashCooldownRemaining > 0f)
            return;

        if (dashesRemaining <= 0)
            return;

        Vector3 wishDir = (transform.forward * moveInput.y) + (transform.right * moveInput.x);
        if (wishDir.sqrMagnitude < 0.001f)
            wishDir = transform.forward;

        dashDirection = wishDir.normalized;

        dashesRemaining--;
        dashTimeRemaining = dashDuration;
        dashCooldownRemaining = dashCooldown;
    }

    // Jump logic
    private void HandleJumpRequests()
    {
        bool hasBufferedJump = jumpBufferTimer > 0f;

        if (hasBufferedJump && CanJump())
        {
            DoJump();
            jumpBufferTimer = 0f;
        }

        // Variable jump height (cut jump short if releasing jump)
        if (jumpReleasedThisFrame && verticalVelocity > 0f)
            verticalVelocity *= jumpCutMultiplier;
    }

    private bool CanJump()
    {
        bool groundedLike = isGroundedReal || coyoteTimer > 0f;
        return groundedLike ? (jumpsUsed < maxJumps) : (jumpsUsed < maxJumps);
    }

    private void DoJump()
    {
        jumpsUsed++;
        verticalVelocity = Mathf.Sqrt(2f * jumpHeight * -gravity);

        coyoteTimer = 0f;

        // Prevent ground check from re-grounding right after jump
        groundedLockTimer = groundedLockTimeAfterJump;
    }

    // Gravity
    private void ApplyGravity()
    {
        if (dashTimeRemaining > 0f)
        {
            verticalVelocity = 0f;
            return;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    // Movement
    private void ApplyMovement()
    {
        Vector3 horizontal =
            transform.forward * (moveInput.y * moveSpeed) +
            transform.right * (moveInput.x * strafeSpeed);

        if (dashTimeRemaining > 0f)
        {
            Vector3 dashVelocity = dashDirection * dashSpeed;
            controller.Move(dashVelocity * Time.deltaTime);
            return;
        }

        Vector3 velocity = horizontal + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    // UI
    void SetCountText()
    {
        if (!countText) return;
        countText.text = "Count: " + count.ToString();
    }

    // Camera Look
    private void UpdateLook()
    {
        transform.Rotate(0f, mouseX, 0f);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        if (playerCamera != null)
            playerCamera.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }

    public void Launch(float force)
    {
        verticalVelocity = force;

        // reset fall speed and allow jumps after launch
        jumpsUsed = 0;
        coyoteTimer = 0f;

        // also lock ground briefly so launch doesn't instantly "land"
        groundedLockTimer = groundedLockTimeAfterJump;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }
}