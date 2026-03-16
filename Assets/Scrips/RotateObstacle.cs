using UnityEngine;

public class RotateObstacle : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    public float timeRemaning = 5f;
    private bool timerIsRunning = false;

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    public void StopTrap()
    {
        rotationSpeed = Vector3.zero;
        timerIsRunning = true;

        if (timerIsRunning)
        {
            if (timeRemaning < 0f)
            {
                timeRemaning -= Time.deltaTime;
                transform.Rotate(rotationSpeed * Time.deltaTime);
            }
        }
    }
}