using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private GameObject textPrompt;
    [SerializeField] private GameObject dialogueMenu;

    public List<GameObject> dialogueTextList = new List<GameObject>();

    private bool playerInRange;

    void Start()
    {
        if (!textPrompt)
        textPrompt.SetActive(false);

        if (!dialogueMenu)
        dialogueMenu.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;
        if (!dialogueMenu) return;

        dialogueMenu.SetActive(!dialogueMenu.activeSelf);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
  
        playerInRange = true;
        if (!textPrompt) return;
        textPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        if (!textPrompt) return;
        textPrompt.SetActive(false);

        if (dialogueMenu != null)
        {
            dialogueMenu.SetActive(false);
        }
    }
}
