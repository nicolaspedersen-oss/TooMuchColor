using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TimedSceneLoadEvent : MonoBehaviour
{
    [Header("Load Settings")]
    [SerializeField] private string sceneName;
    [SerializeField] private float delaySeconds = 2f;

    [Header("Optional Events")]
    public UnityEvent onCountdownStarted;
    public UnityEvent onCountdownFinished; // fired right before load

    private Coroutine _routine;

    // Call this from InteractionEvent.onInteract
    public void StartCountdownAndLoad()
    {
        if (_routine != null) StopCoroutine(_routine);
        _routine = StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        onCountdownStarted?.Invoke();

        if (delaySeconds > 0f)
            yield return new WaitForSeconds(delaySeconds);

        onCountdownFinished?.Invoke();

        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError($"{nameof(TimedSceneLoadEvent)}: sceneName is empty.");
            yield break;
        }

        SceneManager.LoadScene(sceneName);
    }
}