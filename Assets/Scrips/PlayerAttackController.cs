using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("Referances")]
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
    [SerializeField] private float shootCooldown = 0.2f;

    [Header("Lightning Beam")]
    [SerializeField] private float beamRange = 40f;
    [SerializeField] private float beamDamage = 15f;
    [SerializeField] private float beamTickRate = 12f; // damage per second
    [SerializeField] private ParticleSystem beamEffectPrefab;

    [Header("Secondary Slash")]
    [SerializeField] private float slashDamage = 18f;
    [SerializeField] private float slashRadius = 1.0f;
    [SerializeField] private float slashCooldown = 0.35f;
    [SerializeField] private LayerMask enemyMask = ~0;
    [SerializeField] private Transform slashOrigin;
    [SerializeField] private float slashForwardOffset = 1.2f;
    [SerializeField] private bool slashUsesCurrentElement = false;

    [Header("Brush Prefabs")]
    public GameObject[] brushes;

    private ElementType current = ElementType.Fire;
    private float beamTickTimer;
    private float slashCooldownTimer;
    private float shootCooldownTimer;

    public AudioSource audioSource;
    public AudioClip shootSound;

    private void Awake()
    {
    }

    private void Start()
    {
        SwitchBrush(0);
    }

    void Update()
    {
        // cooldown timers
        slashCooldownTimer -= Time.deltaTime;
        shootCooldownTimer -= Time.deltaTime;

        // Elemental selection
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            current = ElementType.Fire;
            SwitchBrush(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            current = ElementType.Water;
            SwitchBrush(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            current = ElementType.Lightning;
            SwitchBrush(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            current = ElementType.Grass;
            SwitchBrush(3);
        }

        if (current == ElementType.Lightning)
        {
            if (Input.GetMouseButton(0)) FireBeam();
        }
        else
        {
            if (Input.GetMouseButton(0)) FireProjectile();
        }

        // Secondary slash
        if (Input.GetKeyDown(KeyCode.F))
        {
            TrySlash();
        }
    }

    void SwitchBrush(int index)
    {
        for (int i = 0; i < brushes.Length; i++)
        {
            brushes[i].SetActive(i == index);
        }
    }

    void TrySlash()
    {
        if (slashCooldownTimer > 0f) return;
        slashCooldownTimer = slashCooldown;

        Transform originT = slashOrigin != null ? slashOrigin : transform;

        // Center of melee volume in front of the player
        Vector3 center = originT.position + originT.forward * slashForwardOffset;

        var triggerMode = QueryTriggerInteraction.Collide;

        Collider[] hits = Physics.OverlapSphere(center, slashRadius, enemyMask, triggerMode);

        // Build payload
        AttackHit hitPayload;
        if (slashUsesCurrentElement)
        {
            hitPayload = BuildHitForCurrentElement();
            hitPayload.damage = slashDamage;
        }
        else
        {
            hitPayload = new AttackHit { damage = slashDamage, element = ElementType.Fire };
        }

        for (int i = 0; i < hits.Length; i++)
        {
            Collider collider = hits[i];

            Vector3 p = collider.ClosestPoint(originT.position);
            Vector3 to = p - originT.position;

            Vector3 forward = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized;
            Vector3 toFlat = Vector3.ProjectOnPlane(to, Vector3.up);

            float distFlat = toFlat.magnitude;
            if (distFlat < 0.0001f) continue;

            Vector3 dirFlat = toFlat / distFlat;
            float dot = Vector3.Dot(forward, dirFlat);

            if (dot < 0.2f) continue;

            var enemyHealth = collider.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(slashDamage);
            }
        }
    }

    void FireProjectile()
    {
        audioSource.PlayOneShot(shootSound);
        if (shootCooldownTimer > 0f) return;
        shootCooldownTimer = shootCooldown;

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
        beamEffectPrefab.Play();
        audioSource.PlayOneShot(shootSound);

        // tick-based so holding mouse does steady damage
        beamTickTimer -= Time.deltaTime;
        if (beamTickTimer > 0f) return;
        beamTickTimer = 1f / beamTickRate;

        Vector3 dir = GetAimDirection(out _);

        // Lightning beam uses raycast
        if (Physics.Raycast(muzzle.position, dir, out RaycastHit hitInfo, beamRange, aimMask)) // QueryTriggerInteraction.Ignore
        {
            AttackHit h = new AttackHit
            {
                damage = beamDamage,
                element = ElementType.Lightning
                // Stun effect. Chain lightning
            };

            var receiver = hitInfo.collider.GetComponentInParent<IHitReceiver>();
            if (receiver != null)
            {
                receiver.ReceiveHit(h, hitInfo.point, gameObject);
            }
            else
            {
                if (hitInfo.collider.TryGetComponent(out EnemyHealth enemy))
                {
                    enemy.TakeDamage(beamDamage);
                }
                if (hitInfo.collider.TryGetComponent(out EnemyStatus status))
                {
                    status.ApplyHit(h);
                }
            }
        }
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
                    damage = 35f,
                    element = ElementType.Fire,
                    dotDps = 10f,
                    dotDuration = 5f
                };

            case ElementType.Water:
                return new AttackHit
                {
                    damage = 25f,
                    element = ElementType.Water,
                    slowPercent = 0.5f,
                    slowDuration = 2.5f
                    // Knockback effect + Slow
                };

            case ElementType.Grass:
                return new AttackHit
                {
                    damage = 20f,
                    element = ElementType.Grass,
                    rootDuration = 2f
                    // poison, root, lifesteal
                };
        }

        return new AttackHit { damage = 15f, element = current };
    }

#if UNITY_EDITOR // Vissable Slash range 
    private void OnDrawGizmosSelected()
    {
        Transform originT = slashOrigin != null ? slashOrigin : transform;
        Vector3 center = originT.position + originT.forward * slashForwardOffset;
        Gizmos.DrawWireSphere(center, slashRadius);
    }
#endif
}