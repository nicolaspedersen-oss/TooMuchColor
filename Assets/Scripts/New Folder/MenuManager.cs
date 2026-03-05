using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;

    private bool isMenuOpen = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        mainMenuPanel.SetActive(isMenuOpen);

        // Pause or resume game
        Time.timeScale = isMenuOpen ? 0f : 1f;
    }

    public void ResumeGame()
    {   
        Debug.Log("Resume button clicked!");
        isMenuOpen = false;
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game...");
    }
}