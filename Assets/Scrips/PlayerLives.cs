using TMPro;
using UnityEngine;

public class PlayerLives : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int playerLives = 3;
    //[SerializeField] private GameObject popupObject;
    [SerializeField] private GameObject loseScreen;

    private PlayerRespawn respawn;
    private PlayerHealth playerDie;
    private int livesRemaining;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        respawn = GetComponent<PlayerRespawn>();
        playerDie = GetComponent<PlayerHealth>();

        livesRemaining = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LivesLost()
    {
        livesRemaining--;
        if (livesRemaining > 0)
        {
            respawn.Respawn();
        }
        else
        {
            playerDie.Die();

            //popupObject.SetActive(true);
            loseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
