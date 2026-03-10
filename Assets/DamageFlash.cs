using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.3f;
    public float maxAlpha = 0.6f;

    private float flashTimer = 0f;

    private void Update()
    {
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;

            float normalized = flashTimer / flashDuration;
            float alpha = Mathf.Lerp(0f, maxAlpha, normalized);

            Color c = flashImage.color;
            c.a = alpha;
            flashImage.color = c;
        }
        else
        {
            // Ensure it stays fully invisible when not flashing
            Color c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
        }
    }

    public void TriggerFlash()
    {
        flashTimer = flashDuration;
    }
}
