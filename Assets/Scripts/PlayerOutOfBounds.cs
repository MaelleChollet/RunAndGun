using UnityEngine;

public class PlayerOutOfBounds : MonoBehaviour
{
    [Tooltip("Limites en coordonnées monde. Ajuste-les pour qu'elles correspondent aux bords de ta caméra/niveau.")]
    public float minY = -10f;

    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        Vector3 pos = transform.position;

        if (pos.y < minY)
        {
            if (playerHealth != null)
            {
                playerHealth.Kill();
            }
        }
    }
}