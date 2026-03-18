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

    private Vector3 startLocalPos;
    private float bobTimer;
    private float attackLerp; // 0 = normal, 1 = fully extended

    private void Start()
    {
        startLocalPos = transform.localPosition;
    }

    private void Update()
    {
        UpdateAttack();
        UpdateBobbing();

        // Combine bobbing + attack
        Vector3 finalPos = startLocalPos;

        // Add bobbing
        finalPos += new Vector3(0, Mathf.Sin(bobTimer) * bobAmount, 0);

        // Add attack extension
        finalPos += Vector3.forward * extendDistance * attackLerp;

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
        {
            attackLerp = Mathf.MoveTowards(attackLerp, 1f, Time.deltaTime * extendSpeed);
        }
        else
        {
            attackLerp = Mathf.MoveTowards(attackLerp, 0f, Time.deltaTime * returnSpeed);
        }
    }

    private void UpdateBobbing()
    {
        bool moving = controller.velocity.magnitude > 0.1f && controller.isGrounded;

        if (moving)
        {
            bobTimer += Time.deltaTime * bobSpeed;
        }
        else
        {
            bobTimer = Mathf.Lerp(bobTimer, 0f, Time.deltaTime * bobSmooth);
        }
    }
}
