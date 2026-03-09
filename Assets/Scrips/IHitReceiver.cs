using UnityEngine;

public interface IHitReceiver
{
    void ReceiveHit(AttackHit hit, Vector3 hitPoint, GameObject instigator);
}