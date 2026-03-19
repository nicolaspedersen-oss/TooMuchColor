using UnityEngine;

using System.Collections;
using UnityEngine;

public class RotateDiamonds : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 100, 0);
    [SerializeField] private GameObject crystal;
    private Vector3 defaultRotationSpeed;
    private Coroutine stopRoutine;

    private void Awake()
    {
        Rotate();
        crystal.SetActive(false);
    }

    private void Update()

    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
    public void Rotate()
    {
       defaultRotationSpeed = rotationSpeed;
        crystal.SetActive(true);
    }
}

