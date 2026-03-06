using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startPromptPanel;
    [SerializeField] private GameObject mainMenuPanel;

    private bool isMenuOpen = false;
    private bool levelStarted = false;
    private bool isCursorVisable;

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
                //Debug.Log("Level Started");
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {   
        Cursor.lockState = CursorLockMode.None;

        isMenuOpen = !isMenuOpen;
        mainMenuPanel.SetActive(isMenuOpen);

        // Pause or resume game
        if (isMenuOpen )
        {
            Time.timeScale = 0f;
            isCursorVisable = Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            isCursorVisable = Cursor.visible = false;
        }
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void StartLevel()
    {
        levelStarted = true;
        Time.timeScale = 1f;
        startPromptPanel.SetActive(false);
        //Debug.Log("Start prompt false");
    }

    public void ResumeGame()
    {   
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