using UnityEngine;
using UnityEngine.SceneManagement;

//[InfoHeaderClass("Drag me into the scene. I can load new scenes")]
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    /*
    [TextArea(1, 10)]
    [SerializeField]
    private string helpInfo = "Drag me into the scene. I can load new scenes";
    */

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
         
        //DontDestroyOnLoad(gameObject); // persists between scenes
    }

    public void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Only works for executables.
    public void QuitGame()
    {
        Application.Quit();
    }
}
