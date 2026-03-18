using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : MonoBehaviour
{
    [Header("Prompt")]
    [SerializeField] private string promptText = "Press E";
    [SerializeField] private GameObject textPrompt;

    [Header("Event")]
    public UnityEvent onInteract;

    private bool playerInRange;

    private void Start()
    {
        if (!textPrompt) return;
        textPrompt.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        {
            playerInRange = true;
            if (!textPrompt) return;
            textPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        {
            playerInRange= false;
            if (!textPrompt) return;
            textPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            onInteract?.Invoke();
        }
    }
}
