using UnityEngine;
using UnityEngine.SceneManagement;

public class HitDetector : MonoBehaviour
{
    [SerializeField] private float duration = 2f;
    private float timer;
    private bool isRunning = false;
    private Color originalColor;

    void Update()
    {
        if (isRunning == true)
        {
            HitDetection();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ball")
        {
            originalColor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material.color = Color.red;
            timer = duration;
            isRunning = true;
        }
    }
    private void HitDetection()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            GetComponent<Renderer>().material.color = originalColor;
            isRunning = false;
        }
    }
}