using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;

    [Header("Limites du niveau")]
    public float minX = -9f;
    public float maxX = 19f;
    public float maxY = 3f;

    [SerializeField]
    private bool isGrounded = false;

    private float faceFlip = 1f;
    private Vector3 scaleOrigin = Vector3.one;

    private float horizontal = 0f;
    private bool running = false;

    private Rigidbody2D rb;
    private Animator animator;
    
    
    public static PlayerController instant;

    [Header("Shooting")]
    [Space(0)]
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform bulletSpawnPoint;


    private void Awake()
    {
        instant = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        scaleOrigin = transform.localScale;
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        rb.linearVelocityX = horizontal * speed;

        if (horizontal == 0f)
        {
            running = false;
        }
        else
        {
            running = true;
            if (horizontal < 0)
            {
                faceFlip = -1f;
            }
            else if (horizontal > 0)
            {
                faceFlip = 1f;
            }

        }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce);
            isGrounded = false;
        }

        animator.SetBool("running", running);

        // Shooting
        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        transform.localScale = new Vector3(scaleOrigin.x * faceFlip, scaleOrigin.y, scaleOrigin.z);

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Min(clampedPosition.y, maxY); // seulement une limite haute
        transform.position = clampedPosition;
    }

    private Vector2 GetShootDirection()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 direction = (Vector2)(mouseWorldPos - bulletSpawnPoint.position);
        return direction.normalized;
    }

    private void Shoot()
    {
        if (bullet == null || bulletSpawnPoint == null)
        {
            Debug.Log("Bullet or spawn point is not assigned.");
            return;
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        faceFlip = (mouseWorldPos.x < transform.position.x) ? -1f : 1f;

        Vector2 shootDirection = GetShootDirection();
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

        GameObject bulletObj = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.Euler(0f, 0f, angle));
        bulletObj.GetComponent<BulletBehavior>().SetDirection(shootDirection);
    }   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

   
}
