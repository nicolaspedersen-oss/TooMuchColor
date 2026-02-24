using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float muzzleVelocity = 60f;
    [SerializeField] private GameObject bulletBallPrefab;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private float maxAimDistance = 200f;
    [SerializeField] private LayerMask aimMask = ~0;
    [SerializeField] private LayerMask obstructionMask = ~0;

    [SerializeField] private bool bulletUsesGravity = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Fire();
    }

    void Fire()
    {
        // Where is the crosshair aiming? (camera center ray)
        Ray camRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;
        if (Physics.Raycast(camRay, out RaycastHit camHit, maxAimDistance, aimMask, QueryTriggerInteraction.Ignore))
            aimPoint = camHit.point;
        else
            aimPoint = camRay.GetPoint(maxAimDistance);

        // From the muzzle, is there something blocking the barrel toward that aim point?
        Vector3 toAim = aimPoint - muzzle.position;
        float distToAim = toAim.magnitude;
        Vector3 dir = toAim / Mathf.Max(distToAim, 0.0001f);

        if (Physics.Raycast(muzzle.position, dir, out RaycastHit muzzleHit, distToAim, obstructionMask, QueryTriggerInteraction.Ignore))
        {
            // Something is in the way of the barrel, so shoot at the obstruction instead.
            aimPoint = muzzleHit.point;
            dir = (aimPoint - muzzle.position).normalized;
        }

        GameObject bullet = Instantiate(bulletBallPrefab, muzzle.position, Quaternion.LookRotation(dir));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.useGravity = bulletUsesGravity;
        rb.linearVelocity = dir * muzzleVelocity;
    }
}