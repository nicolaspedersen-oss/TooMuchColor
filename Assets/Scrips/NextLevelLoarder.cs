using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!string.IsNullOrWhiteSpace(sceneName)) // load assigned level if sceneName is assigned in the inspector
        {
            SceneManager.LoadScene(sceneName);
        }
        else // if sceneName is not assigned in the inspector, load next level in the buildIndex
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (nextIndex >= SceneManager.sceneCountInBuildSettings)
                nextIndex = 0;

            SceneManager.LoadScene(nextIndex);
        }
    }
}