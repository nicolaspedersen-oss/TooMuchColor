using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupObject;

    private void Awake()
    {
        if (popupObject != null)
            popupObject.SetActive(false);
    }

    public void Show()
    {
        if (popupObject != null)
            popupObject.SetActive(true);
    }

    public void Hide()
    {
        if (popupObject != null)
            popupObject.SetActive(false);
    }

    private void Update()
    {
        if (popupObject.activeSelf && Input.GetKeyDown(KeyCode.E))
            Hide();
    }

}
