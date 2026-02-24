using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float strafeSpeed = 5f;

    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float minPich = -80f;
    [SerializeField] private float maxPich = 90f;

    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private int maxJumps = 2;

    private CharacterController playerCharacterController;
    private float pitch;
    private float verticalVelocity;
    private int jumpsUsed;

    private void Start()
    {
        playerCharacterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Character Movement
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 horizontalVelocity =
            transform.forward * (vertical * moveSpeed) +
            transform.right * (horizontal * strafeSpeed);

        // Character Jump
        if (playerCharacterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
            jumpsUsed = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpsUsed < maxJumps)
        {
            jumpsUsed++;
            verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = horizontalVelocity + Vector3.up * verticalVelocity;
        playerCharacterController.Move(velocity * Time.deltaTime);

        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0f, mouseX, 0f);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPich, maxPich);
        playerCamera.localEulerAngles = new Vector3(pitch, 0, 0);
    }
}