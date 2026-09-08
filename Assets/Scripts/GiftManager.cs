using System;
using UnityEngine;

public class GiftManager : MonoBehaviour
{
    public static GiftManager instance;

    [HideInInspector] public int totalGifts = 3;
    private int collectedGifts = 0;

    public event Action<int, int> OnGiftCollected; // (ramassés, total)
    public event Action OnAllGiftsCollected;

    private void Awake()
    {
        instance = this;
    }

    public void CollectGift()
    {
        collectedGifts++;
        OnGiftCollected?.Invoke(collectedGifts, totalGifts);

        if (collectedGifts >= totalGifts)
        {
            OnAllGiftsCollected?.Invoke();
        }
    }
}