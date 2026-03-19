using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerRespawn : MonoBehaviour
{
    private CharacterController characterController;

    public GameObject[] platformInstance;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
            RespawnPlatforms();
        }

        for (int i = 5; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                JumpToCheckpoint(i);
        }
    }

    private void JumpToCheckpoint(int index)
    {
        var mgr = CheckpointManager.Instance;
        if (mgr == null) return;

        mgr.SetCheckpointByIndex(index);
        Respawn();
    }

    public void Respawn()
    {
        var mgr = CheckpointManager.Instance;
        if (mgr == null || mgr.CurrentSpawnPoint == null) return;

        // Turn off controller
        characterController.enabled = false;

        Transform sp = mgr.CurrentSpawnPoint;
        transform.SetPositionAndRotation(sp.position, sp.rotation);

        // Turn it back on
        characterController.enabled = true;
    }

    public void RespawnPlatforms()
    {
        if (platformInstance != null)
        {
            foreach (var p in platformInstance)
                if (p != null) p.SetActive(true);
        }
    }
}