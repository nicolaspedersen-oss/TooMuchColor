using UnityEngine;
using UnityEngine.Events; // 1. Include the namespace

public class EventTriggerExample : MonoBehaviour
{
    // 2. Declare a public UnityEvent (appears in the Inspector)
    public UnityEvent onTriggerEnterEvent;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is the player, for example
        if (other.CompareTag("Player"))
        {
            // 3. Invoke the event, notifying all registered listeners
            onTriggerEnterEvent.Invoke();
        }
    }
}
