using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GiftPickup : MonoBehaviour
{
    [Header("Flottement")]
    public float floatHeight = 0.3f;
    public float floatSpeed = 2f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + new Vector3(0f, offsetY, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GiftManager.instance != null)
            {
                GiftManager.instance.CollectGift();
            }
            Destroy(gameObject);
        }
    }
}