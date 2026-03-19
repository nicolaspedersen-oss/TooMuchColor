using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startPromptPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private NavMeshSurface surface;

    [Header("Default Spawn Launch (if not set by trigger)")]
    [SerializeField] private float defaultSpawnVerticalVelocity = 0f;

    private bool isMenuOpen = false;
    private bool levelStarted = false;
    private bool isCursorVisable;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (!levelStarted && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (!Input.GetKeyDown(KeyCode.Escape))
            {
                StartLevel();
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        

        isMenuOpen = !isMenuOpen;
        if (!mainMenuPanel) return;

        mainMenuPanel.SetActive(isMenuOpen);

        if (isMenuOpen)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            isCursorVisable = Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            isCursorVisable = Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void LoadLevel(string sceneName)
    {
        levelStarted = false;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!surface) return;

        surface.gameObject.SetActive(true);
        surface.enabled = true;
       
        var sp = GameObject.FindWithTag("StartPoint");
        if (sp == null) return;

        var p = GameObject.FindWithTag("Player");
        if (p == null) return;

        // Run the teleport + launch after the scene has fully settled (next frame)
        StartCoroutine(SpawnAndLaunchNextFrame(p, sp));
    }

    private System.Collections.IEnumerator SpawnAndLaunchNextFrame(GameObject player, GameObject startPoint)
    {
        // Make sure we’re not paused (deltaTime must be > 0 to actually move)
        Time.timeScale = 1f;

        // Wait one frame so CharacterController + PlayerMovement Start() have run
        yield return null;

        // Find controller anywhere in hierarchy
        var controller =
            player.GetComponent<CharacterController>() ??
            player.GetComponentInChildren<CharacterController>() ??
            player.GetComponentInParent<CharacterController>();

        // Slight lift to avoid being embedded/ground-snapped
        Vector3 spawnPos = startPoint.transform.position + Vector3.up * 0.05f;
        Quaternion spawnRot = startPoint.transform.rotation;

        if (controller != null) controller.enabled = false;
        player.transform.SetPositionAndRotation(spawnPos, spawnRot);
        if (controller != null) controller.enabled = true;

        // Wait one more frame after enabling controller (helps with grounded state)
        yield return null;

        // Find movement anywhere in hierarchy and launch
        var movement =
            player.GetComponent<PlayerMovement>() ??
            player.GetComponentInChildren<PlayerMovement>() ??
            player.GetComponentInParent<PlayerMovement>();

        if (movement != null)
        {
            movement.Launch(defaultSpawnVerticalVelocity);
        }
    }

    private void StartLevel()
    {
        if (!startPromptPanel) return;

        levelStarted = true;
        Time.timeScale = 1f;
        startPromptPanel.SetActive(false);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeGame()
    {
        isMenuOpen = false;
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game...");
    }
}