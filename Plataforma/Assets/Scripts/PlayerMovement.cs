using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private const int MaxJumpCount = 2;

    [Header("Movimento")]
    [SerializeField, Min(0f)] private float moveSpeed = 6f;
    [SerializeField, Min(0f)] private float jumpVelocity = 3.8f;

    [Header("Chao")]
    [SerializeField] private Transform groundCheck;
    [SerializeField, Min(0f)] private float groundCheckRadius = 0.12f;
    [SerializeField] private LayerMask groundLayers = ~0;

    [Header("Escada")]
    [SerializeField] private Tilemap ladderTilemap;
    [SerializeField, Min(0f)] private float climbSpeed = 2.5f;
    [SerializeField] private RuntimeAnimatorController climbingAnimatorController;

    [Header("Ataque")]
    [SerializeField] private RuntimeAnimatorController attackAnimatorController;
    [SerializeField, Min(0.01f)] private float attackDuration = 0.5f;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private bool spriteFacesRight = true;
    [SerializeField] private string walkingParameter = "Andando";

    private Rigidbody2D body;
    private Collider2D bodyCollider;
    private readonly Collider2D[] groundHits = new Collider2D[8];
    private bool jumpRequested;
    private bool isClimbing;
    private bool isAttacking;
    private float defaultGravityScale;
    private float attackTimeRemaining;
    private RuntimeAnimatorController defaultAnimatorController;
    private int jumpsRemaining = MaxJumpCount;
    private int walkingParameterHash;
    private bool hasWalkingParameter;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
        defaultGravityScale = body.gravityScale;
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
            defaultAnimatorController = animator.runtimeAnimatorController;

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
        if (isAttacking)
        {
            attackTimeRemaining -= Time.deltaTime;
            if (attackTimeRemaining <= 0f)
            {
                FinishAttack();
            }
        }

        Keyboard keyboard = Keyboard.current;
        Gamepad gamepad = Gamepad.current;
        if ((keyboard != null && keyboard.spaceKey.wasPressedThisFrame) ||
            (gamepad != null && gamepad.buttonSouth.wasPressedThisFrame))
        {
            jumpRequested = true;
        }

        if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
        {
            StartAttack();
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = ReadHorizontalInput();
        float verticalInput = ReadVerticalInput();

        if (ladderTilemap != null &&
            IsTouchingLadder() &&
            Mathf.Abs(verticalInput) > 0.01f)
        {
            SetClimbing(true);
        }
        else if (isClimbing && !IsTouchingLadder())
        {
            SetClimbing(false);
        }

        bool jumpFromLadder = jumpRequested && isClimbing && jumpsRemaining > 0;
        if (jumpFromLadder)
        {
            SetClimbing(false);
        }

        if (!isClimbing && body.linearVelocity.y <= 0.01f && IsGrounded())
        {
            jumpsRemaining = MaxJumpCount;
        }

        if (isClimbing)
        {
            body.linearVelocity = new Vector2(horizontalInput * moveSpeed, verticalInput * climbSpeed);
        }
        else
        {
            body.linearVelocity = new Vector2(horizontalInput * moveSpeed, body.linearVelocity.y);
        }

        if (spriteRenderer != null && horizontalInput != 0f)
        {
            spriteRenderer.flipX = (horizontalInput > 0f) != spriteFacesRight;
        }

        if (isClimbing && !isAttacking && animator != null)
        {
            animator.speed = Mathf.Abs(verticalInput) > 0.01f ? 1f : 0f;
        }
        else if (!isAttacking && animator != null)
        {
            UpdateWalkingAnimation(Mathf.Abs(horizontalInput) > 0.01f);
        }

        if (jumpRequested && jumpsRemaining > 0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocity);
            jumpsRemaining--;
        }

        jumpRequested = false;
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

    private float ReadVerticalInput()
    {
        float input = 0f;
        Keyboard keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
            {
                input += 1f;
            }

            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
            {
                input -= 1f;
            }
        }

        Gamepad gamepad = Gamepad.current;
        if (gamepad != null && Mathf.Abs(gamepad.leftStick.y.ReadValue()) > Mathf.Abs(input))
        {
            input = gamepad.leftStick.y.ReadValue();
        }

        return Mathf.Clamp(input, -1f, 1f);
    }

    private void SetClimbing(bool climbing)
    {
        if (isClimbing == climbing)
        {
            return;
        }

        isClimbing = climbing;
        body.gravityScale = climbing ? 0f : defaultGravityScale;

        if (animator == null || isAttacking)
        {
            return;
        }

        ApplyMovementAnimator();
    }

    private void StartAttack()
    {
        if (isAttacking)
        {
            return;
        }

        if (animator == null || attackAnimatorController == null)
        {
            Debug.LogWarning("Atribua o Animator Controller da animacao Ataque ao PlayerMovement.", this);
            return;
        }

        isAttacking = true;
        attackTimeRemaining = attackDuration;
        animator.runtimeAnimatorController = attackAnimatorController;
        animator.speed = 1f;
        animator.Play(0, 0, 0f);
    }

    private void FinishAttack()
    {
        isAttacking = false;
        attackTimeRemaining = 0f;

        if (animator != null)
        {
            ApplyMovementAnimator();
            if (isClimbing)
            {
                animator.speed = 0f;
            }
        }
    }

    private void ApplyMovementAnimator()
    {
        RuntimeAnimatorController movementController = isClimbing
            ? climbingAnimatorController
            : defaultAnimatorController;

        if (movementController == null)
        {
            Debug.LogWarning(
                isClimbing
                    ? "Atribua o Animator Controller da animacao SubindoEscada."
                    : "O Animator Controller padrao do personagem nao foi definido.",
                this);
            return;
        }

        animator.runtimeAnimatorController = movementController;
        animator.speed = 1f;
    }

    private void UpdateWalkingAnimation(bool isMoving)
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

    private bool IsTouchingLadder()
    {
        if (ladderTilemap == null)
        {
            return false;
        }

        Bounds bounds = bodyCollider != null
            ? bodyCollider.bounds
            : new Bounds(transform.position, Vector3.one * 0.5f);

        Vector3Int minCell = ladderTilemap.WorldToCell(bounds.min);
        Vector3Int maxCell = ladderTilemap.WorldToCell(bounds.max);

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                if (ladderTilemap.HasTile(new Vector3Int(x, y, minCell.z)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsGrounded()
    {
        ContactFilter2D groundFilter = new ContactFilter2D();
        groundFilter.SetLayerMask(groundLayers);
        groundFilter.useTriggers = false;

        int hitCount;
        if (groundCheck != null)
        {
            hitCount = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundFilter,
                groundHits);
        }
        else if (bodyCollider != null)
        {
            Bounds bounds = bodyCollider.bounds;
            Vector2 checkPosition = new Vector2(
                bounds.center.x,
                bounds.min.y - groundCheckRadius * 0.5f);
            Vector2 checkSize = new Vector2(
                bounds.size.x * 0.8f,
                groundCheckRadius);

            hitCount = Physics2D.OverlapBox(
                checkPosition,
                checkSize,
                0f,
                groundFilter,
                groundHits);
        }
        else
        {
            return false;
        }

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D hit = groundHits[i];
            if (hit != null &&
                !hit.isTrigger &&
                hit.transform != transform &&
                !hit.transform.IsChildOf(transform))
            {
                return true;
            }
        }

        return false;
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
