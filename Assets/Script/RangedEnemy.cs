using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Combat")]
    public float attackCooldown = 2f;
    public int damage = 1;
    public float bulletSpeed = 4f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Zone")]
    public float zoneRange = 3f; // distance max autorisée depuis le point de départ (si tu veux qu'il puisse légèrement se repositionner ; sinon laisse-le immobile)

    [SerializeField]
    private DetectionZone detectionZone;

    private Transform player;
    private bool playerDetected = false;
    private float lastAttackTime = -999f;
    private Animator animator; // optionnel, peut rester null si pas d'Animator

    private void Awake()
    {
        animator = GetComponent<Animator>(); // ne plante pas si absent, juste null
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
    }

    private void Update()
    {
        if (!playerDetected || player == null) return;

        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null && ph.IsDead) return; // le joueur est mort, on arrête tout


        // Orientation vers le joueur (flip du sprite)
        float direction = (player.position.x > transform.position.x) ? 1f : -1f;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Shoot();
            lastAttackTime = Time.time;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Bullet prefab ou FirePoint non assigné sur " + gameObject.name);
            return;
        }

        if (animator != null)
        {
            animator.SetTrigger("shoot"); // ignoré si pas d'Animator ou pas de paramètre "shoot"
        }

        Vector2 dir = (player.position - firePoint.position);
        dir.Normalize();

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0f, 0f, angle));
        BulletBehavior bb = bulletObj.GetComponent<BulletBehavior>();
        if (bb != null)
        {
            bb.speed = bulletSpeed;
            bb.damage = damage;
            bb.owner = BulletBehavior.Owner.Enemy;
            bb.SetDirection(dir);
        }
    }

    private void OnDrawGizmosSelected()
    {
        DetectionZone dz = GetComponentInChildren<DetectionZone>();
        if (dz != null)
        {
            CircleCollider2D col = dz.GetComponent<CircleCollider2D>();
            if (col != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(dz.transform.position, col.radius);
            }
        }
    }
}