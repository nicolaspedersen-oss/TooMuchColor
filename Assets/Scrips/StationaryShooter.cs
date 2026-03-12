using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class StationaryShooter3D : MonoBehaviour
{
    [Header("Gravity")]
    [SerializeField] private float gravity = -50f;

    public Transform player;
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float shootingRange = 20f;
    public float fireRate = 1f;
    public float bulletSpeed = 20f;

    private float nextFireTime = 0f;

    private float verticalVelocity;
    //private bool isGrounded = false;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        ApplyGravity();

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= shootingRange && HasLineOfSight())
        {
            // Rotate to face the player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);

            // Shoot
            if (Time.time >= nextFireTime)
            {
                Shoot(direction);
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime);
    }

    bool HasLineOfSight()
    {
        Vector3 direction = (player.position - firePoint.position).normalized;

        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, shootingRange))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }

    void Shoot(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * bulletSpeed;
    }
}
