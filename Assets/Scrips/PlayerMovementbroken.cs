using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementNroken : MonoBehaviour
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
    private int maxJumps = 2;

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

    private Rigidbody rb;
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
    public int jumpsUsed;
    private float coyoteTimer;
    private float jumpBufferTimer;

    // Dash state
    private float dashTimeRemaining;
    private float dashCooldownRemaining;
    private Vector3 dashDirection;
    private int dashesRemaining;

    // Force Layer
    private float verticalVelocity;

    private bool isGrounded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        controller = GetComponent<CharacterController>();

        dashesRemaining = maxDashes;

        jumpsUsed = 0;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 rayOrigin = transform.position + Vector3.down;

        Ray ray = new Ray(rayOrigin, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.5f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        ReadInput();
        UpdateLook();
        UpdateTimersAndGround();
        HandleDash();
        HandleJump();
        ApplyGravity();
        ApplyMovement();
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

    // State Layer (ground + timers + jump count reset)
    private void UpdateTimersAndGround()
    {
        //bool grounded = controller.isGrounded;

        if (isGrounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -7f;

            if (jumpsUsed > 1)
            {
                jumpsUsed = 0;
                coyoteTimer = coyoteTime;
            }

            dashesRemaining = maxDashes;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
        if (jumpPressedThisFrame)
        {
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }
    }

    // Dash (timers + direction)
    private void HandleDash()
    {
        if (dashCooldownRemaining > 0f)
        {
            dashCooldownRemaining -= Time.deltaTime;
        }
        if (dashTimeRemaining > 0f)
        {
            dashTimeRemaining -= Time.deltaTime;
        }
        if (!dashPressedThisFrame)
        {
            return;
        }
        if (dashTimeRemaining > 0f || dashCooldownRemaining > 0f)
        {
            return;
        }
        if (dashesRemaining <= 0f)
        {
            return;
        }

        Vector3 wishDir = (transform.forward * moveInput.y) + (transform.right * moveInput.x);
        if (wishDir.sqrMagnitude < 0.001f)
        {
            wishDir = transform.forward;
        }

        dashDirection = wishDir.normalized;

        dashesRemaining--;
        dashTimeRemaining = dashDuration;
        dashCooldownRemaining = dashCooldown;
    }

    // Constraint Layer + Jump Impulse
    private void HandleJump()
    {
        bool hasBufferedJump = jumpBufferTimer > 0f;

        if (hasBufferedJump && jumpsUsed < 2)
        {
            DoJump();
            jumpBufferTimer = 0f;
        }

        if (jumpReleasedThisFrame && verticalVelocity > 0f)
        {
            verticalVelocity *= jumpCutMultiplier;
        }
    }



    private void DoJump()
    {
        if (jumpsUsed == 1)
        {
            verticalVelocity = Mathf.Sqrt(2f * jumpHeight * -gravity);
            jumpsUsed++;
            return;
        }
        else if (jumpsUsed == 0)
        {

            verticalVelocity = Mathf.Sqrt(2f * jumpHeight * -gravity);

            jumpsUsed++;
        }

        coyoteTimer = 0f;
    }

    // Force Layer (gravity)
    private void ApplyGravity()
    {
        if (dashTimeRemaining > 0f)
        {
            verticalVelocity = 0f;
            return;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }

    // Application Layer (ONE Move per frame)
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
            //horizontal = dashDirection * dashSpeed;
        }

        Vector3 velocity = horizontal + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }
    void SetCountText()
    {
        if (!countText) return; // Checks if countText is assigned. if not, return
        countText.text = "Count: " + count.ToString();
    }
    // Camera Look
    private void UpdateLook()
    {
        transform.Rotate(0f, mouseX, 0f);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        if (playerCamera != null)
        {
            playerCamera.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }
    }
    public void Launch(float force)
    {
        verticalVelocity = force;

        // reset fall speed and allow jumps after launch
        jumpsUsed = 0;
        coyoteTimer = 0f;
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