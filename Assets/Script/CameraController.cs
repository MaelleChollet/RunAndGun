using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Limites horizontales")]
    public float minX = 0f;
    public float maxX = 12f;

    [Header("Limites verticales")]
    public float minY = -1f;
    public float maxY = 3f;

    [Header("Décalage vertical (optionnel)")]
    public float verticalOffset = 0f;

    private PlayerController playerController;

    private void Start()
    {
        playerController = PlayerController.instant;
    }

    private void Update()
    {
        var playerPosition = playerController.transform.position;
        var cameraPosition = this.transform.position;

        cameraPosition.x = Mathf.Clamp(playerPosition.x, minX, maxX);
        cameraPosition.y = Mathf.Clamp(playerPosition.y + verticalOffset, minY, maxY);

        transform.position = cameraPosition;
    }
}