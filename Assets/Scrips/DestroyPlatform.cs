using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class DesproyPlatform : MonoBehaviour
{
    [SerializeField] private float destroyTimer = 2f;
    private bool timerIsRunning;

    private void OnTriggerEnter(Collider other)
    {
        timerIsRunning = true;
    }

    private void Update()
    {
        if (!timerIsRunning) return;

        destroyTimer -= Time.deltaTime;

        if (destroyTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
