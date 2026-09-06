using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    public float minX = -2f;
    public float maxX = 30f;
    public float minY = -10f;
    public float maxY = 15f;

    private void Update()
    {
        Vector3 pos = transform.position;

        if (pos.x < minX || pos.x > maxX || pos.y < minY || pos.y > maxY)
        {
            Destroy(gameObject);
        }
    }
}