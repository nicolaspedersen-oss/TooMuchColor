using UnityEngine;

using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> enemySpawnLocations = new List<Transform>();
    [SerializeField] private GameObject enemyPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnEnemies()
        {
        for (int i = 0; i < enemySpawnLocations.Count; i++)
        {
            Instantiate(enemyPrefab, enemySpawnLocations[i].position, enemySpawnLocations[i].rotation);
        }
    }
}
