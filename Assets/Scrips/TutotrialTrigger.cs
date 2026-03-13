using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private TutorialPopup tutorialPopup;
    [SerializeField] private bool hideOnExit = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            tutorialPopup.Show();
    }

    private void OnTriggerExit(Collider other)
    {
        if (hideOnExit && other.CompareTag("Player"))
            tutorialPopup.Hide();
    }
}
