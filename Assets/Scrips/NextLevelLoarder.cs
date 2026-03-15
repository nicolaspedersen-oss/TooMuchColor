using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    [SerializeField] private string sceneName;

    [Header("Spawn Launch")]
    [SerializeField] private float nextLevelVerticalVelocity = 8f; // set in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Tell the next scene what vertical velocity to apply
        SpawnSettings.NextSpawnVerticalVelocity = nextLevelVerticalVelocity;

        if (!string.IsNullOrWhiteSpace(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextIndex >= SceneManager.sceneCountInBuildSettings)
                nextIndex = 0;

            SceneManager.LoadScene(nextIndex);
        }
    }
}