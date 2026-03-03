using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Beam : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public float maxBeamDistance = 100f;
    public Transform beamOrigin;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        // Ensure the LineRenderer has exactly two points: start and end
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        UpdateBeam();
    }

    void UpdateBeam()
    {
        // Set the start position of the beam
        lineRenderer.SetPosition(0, beamOrigin.position);

        RaycastHit hit;
        // Cast a ray forward from the origin
        if (Physics.Raycast(beamOrigin.position, beamOrigin.forward, out hit, maxBeamDistance))
        {
            // If the ray hits something, set the end position to the hit point
            lineRenderer.SetPosition(1, hit.point);
            // Optional: attach a particle system here to create sparks at the impact point
        }
        else
        {
            // If the ray doesn't hit anything, set the end position to the max distance
            lineRenderer.SetPosition(1, beamOrigin.position + beamOrigin.forward * maxBeamDistance);
        }
    }
}