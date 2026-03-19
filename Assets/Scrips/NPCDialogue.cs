using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject textPrompt;
    [SerializeField] private GameObject dialogueMenu;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Dialogue lines")]
    [TextArea(2, 4)]
    [SerializeField] private List<string> lines = new List<string>();

    private bool playerInRange;
    private bool dialogueOpen;
    private int lineIndex;

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

        if (!dialogueOpen)
        {
            StartDialogue();
        }
        else
        {
            NextLine();
        }
    }   

    private void StartDialogue()
    {
        if (dialogueMenu == null || dialogueText == null) return;
        if (lines == null || lines.Count == 0) return;

        dialogueOpen = true;
        lineIndex = 0;

        dialogueMenu.SetActive(true);
        dialogueText.text = lines[lineIndex];

        if (textPrompt  != null)
        {
            textPrompt.SetActive(false);
        }
    }

    private void NextLine()
    {
        lineIndex++;
        
        if (lineIndex >= lines.Count)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = lines[lineIndex];
    }

    private void EndDialogue()
    {
        dialogueOpen = false;
        lineIndex = 0;

        if (dialogueMenu  != null)
        {
            dialogueMenu.SetActive(false);
        }

        if (playerInRange && textPrompt != null)
        {
            textPrompt.SetActive(true);
        }
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
