using TMPro;
using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int playerLives = 3;
    //[SerializeField] private GameObject popupObject;
    [SerializeField] private GameObject loseScreen;

    private PlayerRespawn respawn;
    private int livesRemaining;
    private bool isCursorVisable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        respawn = GetComponent<PlayerRespawn>();

        livesRemaining = playerLives;

        if (loseScreen != null)
        {
            loseScreen.SetActive(false);
        }
    }

    public void LoseLife()
    {
        livesRemaining--;

        if (livesRemaining > 0)
        {
            respawn.Respawn();
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (!loseScreen) return;
        if (loseScreen != null)
        {
            loseScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            isCursorVisable = Cursor.visible = true;
        }

        Time.timeScale = 0;
    }

    public int LivesRemaning => livesRemaining;
}
