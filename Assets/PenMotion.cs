using UnityEngine;

public class PenMotion : MonoBehaviour
{
    [Header("Bobbing")]
    public CharacterController controller;
    public float bobSpeed = 6f;
    public float bobAmount = 0.05f;
    public float bobSmooth = 8f;

    [Header("Attack Hold-Out")]
    public float extendDistance = 0.15f;
    public float extendSpeed = 10f;
    public float returnSpeed = 8f;

    [Header("Jump Reaction")]
    public float jumpDipAmount = 0.1f;      // how much the pen dips when jumping
    public float jumpDipSpeed = 6f;         // how fast it dips
    public float jumpReturnSpeed = 4f;      // how fast it returns

    private Vector3 startLocalPos;
    private float bobTimer;
    private float attackLerp;
    private float jumpLerp;
    private bool wasGrounded;

    private void Start()
    {
        startLocalPos = transform.localPosition;
        wasGrounded = controller.isGrounded;
    }

    private void Update()
    {
        UpdateAttack();
        UpdateBobbing();
        UpdateJumpDip();

        // Combine all motion
        Vector3 finalPos = startLocalPos;

        // Bobbing
        finalPos += new Vector3(0, Mathf.Sin(bobTimer) * bobAmount, 0);

        // Attack extension
        finalPos += Vector3.forward * extendDistance * attackLerp;

        // Jump dip
        finalPos += Vector3.down * jumpDipAmount * jumpLerp;

        // Apply final position
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            finalPos,
            Time.deltaTime * 12f
        );
    }

    private void UpdateAttack()
    {
        if (Input.GetMouseButton(0))
            attackLerp = Mathf.MoveTowards(attackLerp, 1f, Time.deltaTime * extendSpeed);
        else
            attackLerp = Mathf.MoveTowards(attackLerp, 0f, Time.deltaTime * returnSpeed);
    }

    private void UpdateBobbing()
    {
        bool moving = controller.velocity.magnitude > 0.1f && controller.isGrounded;

        if (moving)
            bobTimer += Time.deltaTime * bobSpeed;
        else
            bobTimer = Mathf.Lerp(bobTimer, 0f, Time.deltaTime * bobSmooth);
    }

    private void UpdateJumpDip()
    {
        // Detect jump start
        if (!controller.isGrounded && wasGrounded)
        {
            // Begin dip
            jumpLerp = 1f;
        }

        // Smoothly return to normal
        if (controller.isGrounded)
        {
            jumpLerp = Mathf.MoveTowards(jumpLerp, 0f, Time.deltaTime * jumpReturnSpeed);
        }
        else
        {
            jumpLerp = Mathf.MoveTowards(jumpLerp, 0f, Time.deltaTime * jumpDipSpeed);
        }

        wasGrounded = controller.isGrounded;
    }
}
