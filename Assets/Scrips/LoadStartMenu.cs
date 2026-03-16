using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStartMenue : MonoBehaviour
{
    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftShift)) && Input.GetKeyDown(KeyCode.Escape))
        {
            LoadStartLevel();
        }
    }

    public void LoadStartLevel()
    {
        SceneManager.LoadScene("StartMenu");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}