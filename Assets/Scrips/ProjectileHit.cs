using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private LayerMask hitMask = ~0;
    [SerializeField] private bool destroyOnHit = true;

    public AttackHit hit; // set by shooter when spawned

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & hitMask) == 0) 
        {
            return;
        }

        var contact = collision.GetContact(0);
        TryApply(collision.collider, contact.point);

        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & hitMask) == 0)
        {
            return;
        }

        Vector3 p = other.ClosestPoint(transform.position);
        TryApply(other, p);

        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }

    void TryApply(Collider collider, Vector3 hitPoint)
    {
        IHitReceiver receiver = null;

        if (!collider.TryGetComponent<IHitReceiver>(out receiver))
        {
            receiver = collider.GetComponentInParent<IHitReceiver>();
        }

        if (receiver != null)
        {
            receiver.ReceiveHit(hit, hitPoint, gameObject);
        }
    }
}