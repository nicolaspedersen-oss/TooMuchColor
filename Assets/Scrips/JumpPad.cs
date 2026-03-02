using UnityEngine;
using System.Collections;

public class JumpPad : MonoBehaviour
{
    public float bounceHeight = 8f;      // How high the player goes
    public float bounceSpeed = 25f;      // How fast the bounce happens

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();

            if (controller != null)
            {
                StartCoroutine(Bounce(controller));
            }
        }
    }

    IEnumerator Bounce(CharacterController controller)
    {
        float startY = controller.transform.position.y;
        float targetY = startY + bounceHeight;

        controller.enabled = false; // Temporarily disable movement script

        while (controller.transform.position.y < targetY)
        {
            controller.transform.position += Vector3.up * bounceSpeed * Time.deltaTime;
            yield return null;
        }

        controller.enabled = true; // Enable again so gravity continues
    }
}