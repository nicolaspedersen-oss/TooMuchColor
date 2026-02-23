using UnityEngine;

public class CrosshairOnGUI : MonoBehaviour
{
    [SerializeField] private Texture2D crosshairTexture;
    [SerializeField] private float size = 16f;

    private void OnGUI()
    {
        if (crosshairTexture == null) return;

        float x = (Screen.width - size) * 0.5f;
        float y = (Screen.height - size) * 0.5f;

        GUI.DrawTexture(new Rect(x, y, size, size), crosshairTexture);
    }
}