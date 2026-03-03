using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private Camera playerCamera;

    [Header("Aiming")]
    [SerializeField] private float maxAimDistance = 200f;
    [SerializeField] private LayerMask aimMask = ~0;
    [SerializeField] private LayerMask obstructionMask = ~0;

    [Header("Projectiles")]
    [SerializeField] private GameObject fireProjectilePrefab;
    [SerializeField] private GameObject waterProjectilePrefab;
    [SerializeField] private GameObject grassProjectilePrefab;
    [SerializeField] private float muzzleVelocity = 200f;
    [SerializeField] private bool usesGravity = false;

    [Header("Lightning Beam")]
    [SerializeField] private float beamRange = 40f;
    [SerializeField] private float beamDamage = 10f;
    [SerializeField] private float beamTickRate = 12f; // damage per second

    private ElementType current = ElementType.Fire;
    private float beamTickTimer;

    void Update()
    {
        // quick element switching example (replace with your UI)
        if (Input.GetKeyDown(KeyCode.Alpha1)) current = ElementType.Fire;
        if (Input.GetKeyDown(KeyCode.Alpha2)) current = ElementType.Water;
        if (Input.GetKeyDown(KeyCode.Alpha3)) current = ElementType.Lightning;
        if (Input.GetKeyDown(KeyCode.Alpha4)) current = ElementType.Grass;

        if (current == ElementType.Lightning)
        {
            if (Input.GetMouseButton(0)) FireBeam();
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) FireProjectile();
        }
    }

    void FireProjectile()
    {
        Vector3 dir = GetAimDirection(out _);

        GameObject prefab = current switch
        {
            ElementType.Fire => fireProjectilePrefab,
            ElementType.Water => waterProjectilePrefab,
            ElementType.Grass => grassProjectilePrefab,
            _ => fireProjectilePrefab
        };

        var go = Instantiate(prefab, muzzle.position, Quaternion.LookRotation(dir));

        // set hit payload
        var proj = go.GetComponent<ProjectileHit>();
        if (proj != null)
        {
            proj.hit = BuildHitForCurrentElement();
        }

        // launch
        var rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = usesGravity;
            rb.linearVelocity = dir * muzzleVelocity;
        }
    }

    void FireBeam()
    {
        // tick-based so holding mouse does steady damage
        beamTickTimer -= Time.deltaTime;
        if (beamTickTimer > 0f) return;
        beamTickTimer = 1f / beamTickRate;

        Vector3 dir = GetAimDirection(out RaycastHit hitInfo);

        // Lightning beam uses raycast
        if (Physics.Raycast(muzzle.position, dir, out RaycastHit hit, beamRange, aimMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.TryGetComponent(out EnemyHealth enemy))
                enemy.TakeDamage(beamDamage);

            if (hit.collider.TryGetComponent(out EnemyStatus status))
            {
                var h = new AttackHit
                {
                    damage = 0f,
                    element = ElementType.Lightning
                    // stun effect, chain lightning
                };
                status.ApplyHit(h);
            }

            // spawn beam impact VFX at hit.point
        }

        // draw line renderer from muzzle to hit point
    }

    Vector3 GetAimDirection(out RaycastHit camHit)
    {
        Ray camRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;
        if (Physics.Raycast(camRay, out camHit, maxAimDistance, aimMask, QueryTriggerInteraction.Ignore))
            aimPoint = camHit.point;
        else
            aimPoint = camRay.GetPoint(maxAimDistance);

        Vector3 toAim = aimPoint - muzzle.position;
        float distToAim = toAim.magnitude;
        Vector3 dir = toAim / Mathf.Max(distToAim, 0.0001f);

        if (Physics.Raycast(muzzle.position, dir, out RaycastHit muzzleHit, distToAim, obstructionMask, QueryTriggerInteraction.Ignore))
        {
            aimPoint = muzzleHit.point;
            dir = (aimPoint - muzzle.position).normalized;
        }

        return dir;
    }

    AttackHit BuildHitForCurrentElement()
    {
        // Element effects
        switch (current)
        {
            case ElementType.Fire:
                return new AttackHit
                {
                    damage = 20f,
                    element = ElementType.Fire,
                    dotDps = 6f,
                    dotDuration = 3f
                };

            case ElementType.Water:
                return new AttackHit
                {
                    damage = 15f,
                    element = ElementType.Water,
                    slowPercent = 0.35f,
                    slowDuration = 2.5f
                    // Knockback effect + Slow
                };

            case ElementType.Grass:
                return new AttackHit
                {
                    damage = 12f,
                    element = ElementType.Grass,
                    rootDuration = 2f
                    // poison, root, lifesteal
                };
        }

        return new AttackHit { damage = 10f, element = current };
    }
}