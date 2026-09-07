using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float attackRange = 0.8f;
    public float zoneRange = 1.5f; // distance max autorisée depuis le point de départ

    [Header("Combat")]
    public float attackCooldown = 1.5f;
    public int damage = 1;

    [SerializeField]
    private DetectionZone detectionZone;

    private Transform player;
    private bool playerDetected = false;
    private float lastAttackTime = -999f;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector3 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    private void OnEnable()
    {
        detectionZone.OnPlayerEnter += HandlePlayerEnter;
        detectionZone.OnPlayerExit += HandlePlayerExit;
    }

    private void OnDisable()
    {
        detectionZone.OnPlayerEnter -= HandlePlayerEnter;
        detectionZone.OnPlayerExit -= HandlePlayerExit;
    }

    private void HandlePlayerEnter(Collider2D other)
    {
        playerDetected = true;
        player = other.transform;
    }

    private void HandlePlayerExit(Collider2D other)
    {
        playerDetected = false;
        rb.linearVelocityX = 0f;
        if (animator != null) animator.SetBool("running", false);
    }

    private void Update()
    {
        if (!playerDetected || player == null)
        {
            rb.linearVelocityX = 0f;
            if (animator != null) animator.SetBool("running", false);
            return;
        }

        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null && ph.IsDead)
        {
            rb.linearVelocityX = 0f;
            if (animator != null) animator.SetBool("running", false);
            return;
        }

        float distanceToPlayer = Mathf.Abs(player.position.x - transform.position.x);
        float offsetFromStart = transform.position.x - startPosition.x;

        if (distanceToPlayer > attackRange)
        {
            float direction = (player.position.x > transform.position.x) ? 1f : -1f;

            bool wouldExceedZone = (direction > 0f && offsetFromStart >= zoneRange)
                                 || (direction < 0f && offsetFromStart <= -zoneRange);

            if (wouldExceedZone)
            {
                rb.linearVelocityX = 0f;
                if (animator != null) animator.SetBool("running", false);
            }
            else
            {
                rb.linearVelocityX = direction * speed;
                FlipTowards(direction);
                if (animator != null) animator.SetBool("running", true);
            }
        }
        else
        {
            rb.linearVelocityX = 0f;
            if (animator != null) animator.SetBool("running", false);

            float dirToPlayer = (player.position.x > transform.position.x) ? 1f : -1f;
            FlipTowards(dirToPlayer);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                if (animator != null) animator.SetTrigger("attack");
            }
        }
    }

    private void FlipTowards(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    // Appelée par un Animation Event, au moment précis du coup dans le clip Enemy_Attack
    public void DealDamageToPlayer()
    {
        if (player == null) return;

        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = Application.isPlaying ? startPosition : transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(center + Vector3.left * zoneRange, center + Vector3.right * zoneRange);
    }
}