using UnityEngine;

public class BonnFireCheckpoint : MonoBehaviour
{
    private Checkpoint bonnFire;

    void Start()
    {
        bonnFire = GetComponent<Checkpoint>();
    }

    void Update()
    {
        
    }

    public void GetCheckpoint()
    {
        bonnFire.CheckPoint();
    }
}
