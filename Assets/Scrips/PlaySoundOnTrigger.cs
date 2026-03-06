using UnityEngine;
public class PlaySoundOnTrigger : MonoBehaviour
{
    public AudioClip pickUpSound; // Use the inspector to drag a sound onto this field

    void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(pickUpSound, transform.position);
        Destroy(gameObject);
    }
}