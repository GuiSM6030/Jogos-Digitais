using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerControls : MonoBehaviour
{
    public float speed = 10f;
    public float boundX = 5.5f;
    public float boundY = 4f;

    private Rigidbody2D rb2d;
    private Vector2 movement;
    private Camera mainCamera;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera not found. PlayerControls will skip screen-to-world conversion.");
        }
    }

    void Update()
    {
        if (mainCamera == null)
            return;

        Vector3 worldPos;

#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        // Novo Input System
        if (Mouse.current == null)
            return;
        Vector2 screenPos = Mouse.current.position.ReadValue();
        float zDistance = -mainCamera.transform.position.z; // distancia até o plano z=0
        worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, zDistance));
#else
        // Input Manager (legado) ou compatibilidade habilitada
        Vector3 screenPosLegacy = Input.mousePosition;
        float zDistance = -mainCamera.transform.position.z; // distancia até o plano z=0
        worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPosLegacy.x, screenPosLegacy.y, zDistance));
#endif

        worldPos.z = 0f;

        float clampedX = Mathf.Clamp(worldPos.x, -boundX, boundX);
        float clampedY = Mathf.Clamp(worldPos.y, -boundY, boundY);

        movement = new Vector2(clampedX, clampedY);
    }

    void FixedUpdate()
    {
        if (rb2d == null) return;
        rb2d.MovePosition(Vector2.Lerp(rb2d.position, movement, speed * Time.fixedDeltaTime));
    }
}