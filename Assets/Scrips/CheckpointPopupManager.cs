using System.Collections;
using TMPro;
using UnityEngine;

public class CheckpointPopupManager : MonoBehaviour
{
    [SerializeField] private GameObject popupObject;
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private string message = "Checkpoint reached!";
    [SerializeField] private float showSeconds = 1.5f;

    private Coroutine routine;

    private void Start()
    {
        popupObject.SetActive(false);
        CheckpointManager.Instance.OnCheckpointReached += Show;
    }

    private void OnDestroy()
    {
        if (CheckpointManager.Instance != null)
            CheckpointManager.Instance.OnCheckpointReached -= Show;
    }

    public void Show()
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Routine());
    }

    public IEnumerator Routine()
    {
        popupObject.SetActive(true);
        popupText.text = message;

        yield return new WaitForSeconds(showSeconds);

        popupObject.SetActive(false);
        routine = null;
    }
}
