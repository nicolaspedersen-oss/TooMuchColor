using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [Header("Default")]
    public Transform defaultSpawnPoint;

    [Header("Playtest")]
    public List<Transform> checkpoints = new List<Transform>();

    public Transform CurrentSpawnPoint { get; private set; }
    public int playtestSpawnIndex = -1;
    public event Action OnCheckpointReached;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Choose initial spawn
        CurrentSpawnPoint = defaultSpawnPoint;

        // If you selected a playtest index, override for this play session
        if (playtestSpawnIndex >= 0 &&
            playtestSpawnIndex < checkpoints.Count &&
            checkpoints[playtestSpawnIndex] != null)
        {
            CurrentSpawnPoint = checkpoints[playtestSpawnIndex];
        }
    }

    public void SetCheckpoint(Transform newSpawn)
    {
        if (newSpawn == null) return;
        if (CurrentSpawnPoint == newSpawn) return;

        CurrentSpawnPoint = newSpawn;
        OnCheckpointReached?.Invoke();
    }

    public void SetCheckpointByIndex(int index)
    {
        if (index < 0 || index >= checkpoints.Count) return;
        SetCheckpoint(checkpoints[index]);
    }
}