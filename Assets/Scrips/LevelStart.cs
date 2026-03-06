using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelStart : MonoBehaviour
{
    [SerializeField] private GameObject startPromptPanel;

    private bool levelStarted = false;

    void Start()
    {
        Time.timeScale = 0f; // Pause the game
    }

    void Update()
    {
        if (!levelStarted && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            // Only start if the key pressed is NOT Escape
            if (!Input.GetKeyDown(KeyCode.Escape))
            {
                StartLevel();
            }
        }
    }

    private void StartLevel()
    {
        levelStarted = true;
        Time.timeScale = 1f;
        startPromptPanel.SetActive(false);
    }
}