using UnityEngine;

public class PlayerMovementClass : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float strafeSpeed = 5f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float jumpForce = 5f;

    public int maxJumps = 2;

    private float verticalInput;
    private float horizontalInput;
    private float verticalVelocity;
    private Vector3 moveDirection;
    private CharacterController playerCharacterController;
    private int jumpsUsed;

    private void Start()
    {
        playerCharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical"); //Unity built in W & S detection
        horizontalInput = Input.GetAxis("Horizontal"); //Unity built in A & D detection

        moveDirection = this.transform.forward * verticalInput * moveSpeed * Time.deltaTime + this.transform.right * horizontalInput * strafeSpeed * Time.deltaTime;

        if (playerCharacterController.isGrounded == true)
        {
            verticalVelocity = -2f;
            jumpsUsed = 0;
        }
        if (playerCharacterController.isGrounded == false)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space) && jumpsUsed < maxJumps)
        {
            jumpsUsed ++;
            verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        moveDirection.y = verticalVelocity;

        playerCharacterController.Move(moveDirection);

        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);
    }
}


