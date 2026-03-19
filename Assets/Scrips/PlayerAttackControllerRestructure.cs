using UnityEngine;

public class PlayerAttackController_Beginner : MonoBehaviour
{
    // -----------------------------
    // 1) References (things in scene)
    // -----------------------------
    [Header("References")]
    [SerializeField] private Transform muzzle;       // where shots come from
    [SerializeField] private Camera playerCamera;    // what we aim with
    [SerializeField] private AudioSource audioSource;

    // -----------------------------
    // 2) Element selection + visuals
    // -----------------------------
    [Header("Element Visuals")]
    [SerializeField] private GameObject[] brushes;       // show 1 brush at a time
    [SerializeField] private GameObject[] slashEffects;  // show 1 slash effect at a time

    // -----------------------------
    // 3) Aiming
    // -----------------------------
    [Header("Aiming")]
    [SerializeField] private float maxAimDistance = 200f;
    [SerializeField] private LayerMask aimMask = ~0;
    [SerializeField] private LayerMask obstructionMask = ~0;

    // -----------------------------
    // 4) Projectile shooting
    // -----------------------------
    [Header("Projectiles")]
    [SerializeField] private GameObject fireProjectilePrefab;
    [SerializeField] private GameObject waterProjectilePrefab;
    [SerializeField] private GameObject grassProjectilePrefab;
    [SerializeField] private float muzzleVelocity = 200f;
    [SerializeField] private bool usesGravity = false;
    [SerializeField] private float shootCooldown = 0.2f;
    [SerializeField] private AudioClip shootSound;

    // -----------------------------
    // 5) Lightning beam
    // -----------------------------
    [Header("Lightning Beam")]
    [SerializeField] private float beamRange = 40f;
    [SerializeField] private float beamDamage = 15f;
    [SerializeField] private float beamTickRate = 12f; // times per second
    [SerializeField] private ParticleSystem beamEffect; // scene ParticleSystem

    // -----------------------------
    // 6) Slash (secondary attack)
    // -----------------------------
    [Header("Slash")]
    [SerializeField] private float slashDamage = 18f;
    [SerializeField] private float slashRadius = 1.0f;
    [SerializeField] private float slashCooldown = 0.35f;
    [SerializeField] private LayerMask enemyMask = ~0;
    [SerializeField] private Transform slashOrigin;
    [SerializeField] private float slashForwardOffset = 1.2f;

    // -----------------------------
    // Timers
    // -----------------------------
    private float shootTimer;
    private float slashTimer;
    private float beamTickTimer;

    // Current element
    private ElementType currentElement = ElementType.Fire;

    void Start()
    {
        SetElement(ElementType.Fire);
    }

    void Update()
    {
        // decrease timers
        shootTimer -= Time.deltaTime;
        slashTimer -= Time.deltaTime;
        beamTickTimer -= Time.deltaTime;

        HandleElementHotkeys();
        HandleAttacks();
    }

    // -----------------------------
    // INPUT
    // -----------------------------

    void HandleElementHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetElement(ElementType.Fire);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetElement(ElementType.Water);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetElement(ElementType.Lightning);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetElement(ElementType.Grass);
    }

    void HandleAttacks()
    {
        // Left mouse = primary attack
        if (Input.GetMouseButton(0))
        {
            if (currentElement == ElementType.Lightning)
                TryFireBeam();
            else
                TryFireProjectile();
        }
        else
        {
            // stop beam visuals when not holding mouse
            if (beamEffect != null && beamEffect.isPlaying)
                beamEffect.Stop();
        }

        // F = slash
        if (Input.GetKeyDown(KeyCode.F))
        {
            TrySlash();
        }
    }

    // -----------------------------
    // ELEMENT
    // -----------------------------

    void SetElement(ElementType newElement)
    {
        currentElement = newElement;

        // Switch brush + slash effect by element
        int index = ElementToIndex(newElement);
        ShowOnlyThis(brushes, index);
        ShowOnlyThis(slashEffects, index);
    }

    int ElementToIndex(ElementType element)
    {
        // You can change this mapping easily
        if (element == ElementType.Fire) return 0;
        if (element == ElementType.Water) return 1;
        if (element == ElementType.Lightning) return 2;
        if (element == ElementType.Grass) return 3;

        return 0;
    }

    void ShowOnlyThis(GameObject[] objects, int activeIndex)
    {
        if (objects == null) return;

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
                objects[i].SetActive(i == activeIndex);
        }
    }

    // -----------------------------
    // PROJECTILE ATTACK
    // -----------------------------

    void TryFireProjectile()
    {
        // cooldown check first (prevents sound spam)
        if (shootTimer > 0f) return;
        shootTimer = shootCooldown;

        PlayShootSound();

        Vector3 direction = GetAimDirection();
        GameObject prefab = GetProjectilePrefabForCurrentElement();

        if (prefab == null || muzzle == null) return;

        GameObject projectile = Instantiate(prefab, muzzle.position, Quaternion.LookRotation(direction));

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = usesGravity;
            rb.linearVelocity = direction * muzzleVelocity; // (if linearVelocity doesn't exist, use rb.velocity)
        }

        // OPTIONAL: pass element hit info to projectile script
        ProjectileHit ph = projectile.GetComponent<ProjectileHit>();
        if (ph != null)
        {
            ph.hit = BuildHitForCurrentElement();
        }
    }

    GameObject GetProjectilePrefabForCurrentElement()
    {
        if (currentElement == ElementType.Fire) return fireProjectilePrefab;
        if (currentElement == ElementType.Water) return waterProjectilePrefab;
        if (currentElement == ElementType.Grass) return grassProjectilePrefab;

        return fireProjectilePrefab;
    }

    // -----------------------------
    // BEAM ATTACK
    // -----------------------------

    void TryFireBeam()
    {
        // beam visuals should play while holding
        if (beamEffect != null && !beamEffect.isPlaying)
            beamEffect.Play();

        // tick check (how often we deal damage)
        if (beamTickTimer > 0f) return;
        beamTickTimer = 1f / beamTickRate;

        PlayShootSound(); // optional; many games use a loop sound instead

        Vector3 direction = GetAimDirection();

        Ray ray = new Ray(muzzle.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, beamRange, aimMask, QueryTriggerInteraction.Ignore))
        {
            // simplest damage method: try EnemyHealth
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(beamDamage);
            }

            // OPTIONAL: status effects
            EnemyStatus status = hit.collider.GetComponentInParent<EnemyStatus>();
            if (status != null)
            {
                status.ApplyHit(BuildHitForCurrentElement());
            }
        }
    }

    // -----------------------------
    // SLASH ATTACK
    // -----------------------------

    void TrySlash()
    {
        if (slashTimer > 0f) return;
        slashTimer = slashCooldown;

        // play current slash effect particle system
        PlayActiveSlashEffect();

        Transform origin = (slashOrigin != null) ? slashOrigin : transform;

        Vector3 center = origin.position + origin.forward * slashForwardOffset;

        Collider[] hits = Physics.OverlapSphere(center, slashRadius, enemyMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyHealth enemy = hits[i].GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(slashDamage);
            }
        }
    }

    void PlayActiveSlashEffect()
    {
        if (slashEffects == null) return;

        // find the active effect and play its particle system
        for (int i = 0; i < slashEffects.Length; i++)
        {
            if (slashEffects[i] == null) continue;
            if (!slashEffects[i].activeInHierarchy) continue;

            ParticleSystem ps = slashEffects[i].GetComponent<ParticleSystem>();
            if (ps == null) ps = slashEffects[i].GetComponentInChildren<ParticleSystem>();

            if (ps != null)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ps.Play(true);
            }
            break;
        }
    }

    // -----------------------------
    // AIMING (camera -> aim point -> muzzle obstruction)
    // -----------------------------

    Vector3 GetAimDirection()
    {
        if (playerCamera == null || muzzle == null) return transform.forward;

        // ray from center of screen
        Ray camRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 aimPoint;

        RaycastHit camHit;
        if (Physics.Raycast(camRay, out camHit, maxAimDistance, aimMask, QueryTriggerInteraction.Ignore))
            aimPoint = camHit.point;
        else
            aimPoint = camRay.GetPoint(maxAimDistance);

        // direction from muzzle to aim point
        Vector3 dir = (aimPoint - muzzle.position).normalized;

        // if muzzle is blocked (wall in front of gun), shorten
        float distanceToAim = Vector3.Distance(muzzle.position, aimPoint);

        RaycastHit muzzleHit;
        if (Physics.Raycast(muzzle.position, dir, out muzzleHit, distanceToAim, obstructionMask, QueryTriggerInteraction.Ignore))
        {
            dir = (muzzleHit.point - muzzle.position).normalized;
        }

        return dir;
    }

    // -----------------------------
    // HIT DATA (element stats)
    // -----------------------------

    AttackHit BuildHitForCurrentElement()
    {
        if (currentElement == ElementType.Fire)
        {
            return new AttackHit { damage = 35f, element = ElementType.Fire, dotDps = 10f, dotDuration = 5f };
        }
        if (currentElement == ElementType.Water)
        {
            return new AttackHit { damage = 25f, element = ElementType.Water, slowPercent = 0.5f, slowDuration = 2.5f };
        }
        if (currentElement == ElementType.Grass)
        {
            return new AttackHit { damage = 20f, element = ElementType.Grass, rootDuration = 2f };
        }
        if (currentElement == ElementType.Lightning)
        {
            return new AttackHit { damage = beamDamage, element = ElementType.Lightning };
        }

        return new AttackHit { damage = 10f, element = currentElement };
    }

    void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Transform origin = (slashOrigin != null) ? slashOrigin : transform;
        Vector3 center = origin.position + origin.forward * slashForwardOffset;
        Gizmos.DrawWireSphere(center, slashRadius);
    }
#endif
}