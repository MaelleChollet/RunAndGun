using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public enum Owner { Player, Enemy }

    public float speed = 5f;
    public int damage = 1;
    public Owner owner = Owner.Player;

    private Vector2 direction = Vector2.right;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Start()
    {
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bullet (" + owner + ") a touché : " + other.gameObject.name + " avec le tag " + other.tag + " sur le layer " + LayerMask.LayerToName(other.gameObject.layer));

        if (owner == Owner.Player && other.CompareTag("Enemy"))
        {
            EnemyHealth eh = other.GetComponent<EnemyHealth>();
            if (eh != null) eh.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (owner == Owner.Enemy && other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}