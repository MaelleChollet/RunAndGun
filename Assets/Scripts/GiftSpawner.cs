using System.Collections.Generic;
using UnityEngine;

public class GiftSpawner : MonoBehaviour
{
    [Tooltip("Le prefab du cadeau à faire apparaître.")]
    public GameObject giftPrefab;

    [Tooltip("Tous les points où un cadeau PEUT apparaître.")]
    public Transform[] spawnPoints;

    [Tooltip("Combien de cadeaux doivent apparaître au total, l'un après l'autre.")]
    public int giftsToSpawn = 3;

    private List<Transform> availablePoints;
    private int giftsSpawnedSoFar = 0;

    private void Start()
    {
        if (giftPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("GiftSpawner : prefab ou points de spawn manquants.");
            return;
        }

        availablePoints = new List<Transform>(spawnPoints);
        giftsToSpawn = Mathf.Min(giftsToSpawn, availablePoints.Count);

        if (GiftManager.instance != null)
        {
            GiftManager.instance.totalGifts = giftsToSpawn;
            GiftManager.instance.OnGiftCollected += HandleGiftCollected;
        }

        SpawnNextGift(); // seulement le premier cadeau au démarrage
    }

    private void OnDestroy()
    {
        if (GiftManager.instance != null)
        {
            GiftManager.instance.OnGiftCollected -= HandleGiftCollected;
        }
    }

    private void HandleGiftCollected(int collected, int total)
    {
        if (collected < total)
        {
            SpawnNextGift();
        }
    }

    private void SpawnNextGift()
    {
        if (giftsSpawnedSoFar >= giftsToSpawn || availablePoints.Count == 0) return;

        int randomIndex = Random.Range(0, availablePoints.Count);
        Transform chosenPoint = availablePoints[randomIndex];

        availablePoints.RemoveAll(p => p.position == chosenPoint.position);

        Instantiate(giftPrefab, chosenPoint.position, Quaternion.identity);
        giftsSpawnedSoFar++;
    }
}