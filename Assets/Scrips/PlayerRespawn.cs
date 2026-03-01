using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerRespawn : MonoBehaviour
{
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        var mgr = CheckpointManager.Instance;

        if (mgr == null || mgr.CurrentSpawnPoint == null) return;
        {
            // Turn off controller so we can move freely
            characterController.enabled = false;
        }

        Transform sp = mgr.CurrentSpawnPoint;
        transform.SetPositionAndRotation(sp.position, sp.rotation);

        // Turn it back on
        characterController.enabled = true;
    }
}