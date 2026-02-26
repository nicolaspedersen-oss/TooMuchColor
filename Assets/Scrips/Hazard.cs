using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private float damage = 25f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage); // Checks for player component PlayerHealth
        }
    }
}