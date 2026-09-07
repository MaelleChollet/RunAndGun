using System;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public event Action<Collider2D> OnPlayerEnter;
    public event Action<Collider2D> OnPlayerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerExit?.Invoke(other);
        }
    }

    private void OnDrawGizmosSelected()
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, col.radius);
        }
    }
}