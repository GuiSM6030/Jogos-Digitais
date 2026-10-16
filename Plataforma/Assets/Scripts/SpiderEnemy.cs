using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class SpiderEnemy : MonoBehaviour
{
    [SerializeField, Min(0f)] private float moveSpeed = 1.25f;
    [SerializeField, Min(0.1f)] private float attackRange = 1f;
    [SerializeField, Min(0.1f)] private float attackVerticalRange = 1f;
    [SerializeField, Min(0.1f)] private float attackInterval = 1f;
    [SerializeField, Min(0.01f)] private float attackAnimationDuration = 0.4f;
    [SerializeField, Min(0.01f)] private float deathAnimationDuration = 0.6f;

    private Rigidbody2D body;
    private BoxCollider2D bodyCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private PlayerHealth playerHealth;
    private RuntimeAnimatorController walkingAnimatorController;
    private RuntimeAnimatorController attackAnimatorController;
    private RuntimeAnimatorController deathAnimatorController;
    private float nextAttackTime;
    private float attackAnimationTimeRemaining;
    private bool isAttacking;
    private bool isDead;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        body.gravityScale = 1f;
        body.constraints |= RigidbodyConstraints2D.FreezeRotation;

        if (spriteRenderer != null)
        {
            Vector3 scale = transform.lossyScale;
            Bounds spriteBounds = spriteRenderer.bounds;
            bodyCollider.size = new Vector2(
                spriteBounds.size.x / Mathf.Max(Mathf.Abs(scale.x), 0.01f),
                spriteBounds.size.y / Mathf.Max(Mathf.Abs(scale.y), 0.01f));
        }

        if (animator != null)
        {
            walkingAnimatorController = animator.runtimeAnimatorController;
        }
    }

    private void Update()
    {
        if (!isAttacking || isDead)
        {
            return;
        }

        attackAnimationTimeRemaining -= Time.deltaTime;
        if (attackAnimationTimeRemaining <= 0f)
        {
            isAttacking = false;
            if (animator != null && walkingAnimatorController != null)
            {
                animator.runtimeAnimatorController = walkingAnimatorController;
                animator.speed = 1f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDead || playerHealth == null)
        {
            return;
        }

        float horizontalDistance = playerHealth.transform.position.x - transform.position.x;
        float verticalDistance = Mathf.Abs(playerHealth.transform.position.y - transform.position.y);
        if (Mathf.Abs(horizontalDistance) <= attackRange && verticalDistance <= attackVerticalRange)
        {
            body.linearVelocity = new Vector2(0f, body.linearVelocity.y);
            if (Time.time >= nextAttackTime)
            {
                AttackPlayer();
            }

            return;
        }

        if (isAttacking)
        {
            body.linearVelocity = new Vector2(0f, body.linearVelocity.y);
            return;
        }

        float direction = Mathf.Sign(horizontalDistance);
        body.linearVelocity = new Vector2(direction * moveSpeed, body.linearVelocity.y);
        if (spriteRenderer != null && direction != 0f)
        {
            spriteRenderer.flipX = direction < 0f;
        }
    }

    public void Configure(
        RuntimeAnimatorController attackController,
        RuntimeAnimatorController deathController,
        PlayerHealth targetPlayer)
    {
        attackAnimatorController = attackController;
        deathAnimatorController = deathController;
        if (targetPlayer != null)
        {
            playerHealth = targetPlayer;
        }
    }

    public void TakeHit()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        GameScore.AddPoints(5);
        body.linearVelocity = Vector2.zero;
        body.gravityScale = 0f;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        bodyCollider.enabled = false;

        if (animator != null && deathAnimatorController != null)
        {
            animator.runtimeAnimatorController = deathAnimatorController;
            animator.speed = 1f;
            animator.Play(0, 0, 0f);
        }

        Destroy(gameObject, deathAnimationDuration);
    }

    private void AttackPlayer()
    {
        nextAttackTime = Time.time + attackInterval;
        isAttacking = true;
        attackAnimationTimeRemaining = attackAnimationDuration;
        playerHealth.TakeDamage(1);

        if (animator != null && attackAnimatorController != null)
        {
            animator.runtimeAnimatorController = attackAnimatorController;
            animator.speed = 1f;
            animator.Play(0, 0, 0f);
        }
    }
}