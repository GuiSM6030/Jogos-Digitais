using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField, Min(0f)] private float moveSpeed = 6f;
    [SerializeField, Min(0f)] private float jumpVelocity = 12f;

    [Header("Chao")]
    [SerializeField] private Transform groundCheck;
    [SerializeField, Min(0f)] private float groundCheckRadius = 0.12f;
    [SerializeField] private LayerMask groundLayers = ~0;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private bool spriteFacesRight = true;
    [SerializeField] private string walkingParameter = "Andando";

    private Rigidbody2D body;
    private bool jumpRequested;
    private int walkingParameterHash;
    private bool hasWalkingParameter;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        walkingParameterHash = Animator.StringToHash(walkingParameter);

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator != null)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.nameHash == walkingParameterHash &&
                    parameter.type == AnimatorControllerParameterType.Bool)
                {
                    hasWalkingParameter = true;
                    break;
                }
            }

            if (!hasWalkingParameter)
            {
                Debug.LogWarning(
                    $"O Animator precisa de um parametro Bool chamado '{walkingParameter}'. " +
                    "Sem ele, a animacao atual sera pausada quando o personagem parar.",
                    this);
            }
        }
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        Gamepad gamepad = Gamepad.current;
        if ((keyboard != null &&
             (keyboard.spaceKey.wasPressedThisFrame ||
              keyboard.wKey.wasPressedThisFrame ||
              keyboard.upArrowKey.wasPressedThisFrame)) ||
            (gamepad != null && gamepad.buttonSouth.wasPressedThisFrame))
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = ReadHorizontalInput();
        body.linearVelocity = new Vector2(horizontalInput * moveSpeed, body.linearVelocity.y);

        if (spriteRenderer != null && horizontalInput != 0f)
        {
            spriteRenderer.flipX = (horizontalInput > 0f) != spriteFacesRight;
        }

        bool isMoving = Mathf.Abs(horizontalInput) > 0.01f;
        if (animator != null)
        {
            if (hasWalkingParameter)
            {
                animator.SetBool(walkingParameterHash, isMoving);
            }
            else
            {
                animator.speed = isMoving ? 1f : 0f;
            }
        }

        if (jumpRequested)
        {
            if (IsGrounded())
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocity);
            }

            jumpRequested = false;
        }
    }

    private float ReadHorizontalInput()
    {
        float input = 0f;
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            {
                input -= 1f;
            }

            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            {
                input += 1f;
            }
        }

        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            float stickInput = gamepad.leftStick.x.ReadValue();
            if (Mathf.Abs(stickInput) > Mathf.Abs(input))
            {
                input = stickInput;
            }
        }

        return Mathf.Clamp(input, -1f, 1f);
    }

    private bool IsGrounded()
    {
        return groundCheck != null &&
               Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers) != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
