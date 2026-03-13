using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class DesproyPlatform : MonoBehaviour
{
    [SerializeField] private float destroyTimer = 2f;

    private float timer;
    private bool timerIsRunning;

    private void Awake()
    {
        timer = destroyTimer;
    }

    private void OnEnable()
    {
        timer = destroyTimer;
        timerIsRunning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timerIsRunning = true;
        }
    }

    private void Update()
    {
        if (!timerIsRunning) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
