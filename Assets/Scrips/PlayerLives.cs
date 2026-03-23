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
            hearts[i].SetActive(i < livesRemaining); // If i is less than livesRemaining, heart is on. Otherwise off.
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

    
    /*
    public void LoseLife()
    {
        livesRemaining--;

        
        if (hearts != null && hearts.Count > 0)                                             // Disable one heart if we have hearts assigned.
        {
            int indexToDisable = Mathf.Clamp(livesRemaining, 0, hearts.Count - 1);          // Turn off hearts from the end: 2, 1, 0

            if (indexToDisable < hearts.Count && hearts[indexToDisable] != null)            // Only disable when we actually lost a life and the heart exists
            {
                hearts[indexToDisable].SetActive(false);
            }
        }

        if (livesRemaining > 0)
        {
            respawn.Respawn();
        }
        else
        {
            GameOver();
        }
    }
    */

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

    /*
    private void HartsLeft()
    {
        if (hearts.Count > 0)
        {
            heartIndex--;

            if (hearts.Count > 0)
            {
                heartsRemaining = hearts.Count;
            }
        }

        for (int i = 0; i < 2;)
        {

        }
        if (heartIndex == hearts.Count - 1)
        {

        }
        if (heartIndex == hearts.Count - 2)
        {

        }
        if (heartIndex == hearts.Count - 3)
        {

        }
        if (livesRemaining > 0)
        {
            respawn.Respawn();
        }
        else
        {
            GameOver();
        }
    }
    */
}
