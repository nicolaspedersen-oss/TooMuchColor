using UnityEngine;
using System.Collections.Generic;

public class PlayerLives : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int playerLives = 3;

    [SerializeField] private GameObject loseScreen;

    [SerializeField] private List<GameObject> hearts = new List<GameObject>();

    private PlayerRespawn respawn;
    private bool isCursorVisable;
    private int livesRemaining;
    private int heartsRemaining;
    private int heartIndex = 3;

    void Awake()
    {
        respawn = GetComponent<PlayerRespawn>();

        livesRemaining = playerLives;
        heartsRemaining = hearts.Count;

        if (loseScreen != null)
        {
            loseScreen.SetActive(false);
        }
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < livesRemaining);
        }
    }

    public void LoseLife()
    {
        livesRemaining--;

        UpdateHeartsUI();

        if (livesRemaining > 0)
            respawn.Respawn();
        else
            GameOver();
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
