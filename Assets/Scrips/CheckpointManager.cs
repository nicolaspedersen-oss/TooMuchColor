using System;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    public Transform defaultSpawnPoint;
    public Transform CurrentSpawnPoint { get; private set; }

    public event Action OnCheckpointReached;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CurrentSpawnPoint = defaultSpawnPoint;
    }

    public void SetCheckpoint(Transform newSpawn)
    {
        if (newSpawn == null) return;

        if (CurrentSpawnPoint == newSpawn) return;

        CurrentSpawnPoint = newSpawn;

        OnCheckpointReached?.Invoke();
    }
}