using UnityEngine;

[DefaultExecutionOrder(200)]
public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector2 parallaxFactor = new Vector2(0.2f, 0.1f);

    private Vector3 initialPosition;
    private Vector3 initialCameraPosition;

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("ParallaxBackground precisa de uma camera para acompanhar.", this);
            enabled = false;
            return;
        }

        initialPosition = transform.position;
        initialCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 cameraDelta = cameraTransform.position - initialCameraPosition;
        transform.position = initialPosition + new Vector3(
            cameraDelta.x * parallaxFactor.x,
            cameraDelta.y * parallaxFactor.y,
            0f);
    }
}
