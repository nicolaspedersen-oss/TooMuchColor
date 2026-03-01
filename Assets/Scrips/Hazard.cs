using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private float damagePerSecond = 25f;

    private PlayerHealth playerHealth;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage); // Checks for player component PlayerHealth

            playerHealth = other.GetComponent<PlayerHealth>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (other.GetComponent<PlayerHealth>() == playerHealth)
            playerHealth = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerHealth == null) return;
        if (!other.CompareTag("Player")) return;

        playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
    }
}

