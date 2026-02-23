using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce = 5f;


    private float verticalInput;
    private float horizontalInput;
    private float verticalVelocity;
    private Vector3 moveDirection;
    private CharacterController playerCharController;
   

    void Start()
    {
        playerCharController = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        // Get vertical input for movement
        verticalInput = Input.GetAxis("Vertical");
        // Get horizontal input for rotation
        horizontalInput = Input.GetAxis("Horizontal");

      
        // Move the player forward/backward based on vertical input
        //transform.Translate(Vector3.forward * verticalInput * moveSpeed * Time.deltaTime);
        moveDirection = this .transform.forward * verticalInput * moveSpeed * Time.deltaTime;

        if (playerCharController.isGrounded == true)
        {
            verticalVelocity = -2f; // Small negative value to keep the player grounded
        }
        if (playerCharController.isGrounded == false)
        { 
            //
          verticalVelocity += gravity * Time.deltaTime;
        }
        if (true)
        {

        } 
        if(Input.GetButtonDown("Jump") && playerCharController.isGrounded == true)
        {
            verticalVelocity = jumpForce;
        }
            moveDirection.y = verticalVelocity * Time.deltaTime;
        // Rotate the player based on horizontal input
        playerCharController.Move(moveDirection);

        transform.Rotate(Vector3.up * horizontalInput * rotationSpeed * Time.deltaTime);

    }
}
