using UnityEngine;

public class PlayerShoot2 : MonoBehaviour
{
    [SerializeField] private float muzzleVelocity = 20f;
    [SerializeField] private GameObject bulletBallPrefab;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float maxAimDistance = 200f;
    [SerializeField] private LayerMask aimMask = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            FireCannon();
    }

    private void FireCannon()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimMask))
            aimPoint = hit.point;
        else
            aimPoint = ray.GetPoint(maxAimDistance);

        Vector3 dir = (aimPoint - muzzle.position).normalized;

        GameObject bullet = Instantiate(bulletBallPrefab, muzzle.position, Quaternion.LookRotation(dir));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = dir * muzzleVelocity;

        //var bullet = Instantiate(bulletBallPrefab, muzzle.position, muzzle.rotation);

        //var rb = bullet.GetComponent<Rigidbody>();
        //rb.linearVelocity = muzzle.forward * muzzleVelocity;
    }
}