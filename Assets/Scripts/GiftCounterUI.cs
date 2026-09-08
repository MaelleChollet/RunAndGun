using UnityEngine;
using TMPro;

public class GiftCounterUI : MonoBehaviour
{
    [Tooltip("Le texte TextMeshPro qui affiche le nombre de cadeaux ramassés.")]
    public TextMeshProUGUI counterText;

    private void Start()
    {
        int total = GiftManager.instance != null ? GiftManager.instance.totalGifts : 3;
        UpdateText(0, total);

        if (GiftManager.instance != null)
        {
            GiftManager.instance.OnGiftCollected += UpdateText;
        }
    }

    private void OnDestroy()
    {
        if (GiftManager.instance != null)
        {
            GiftManager.instance.OnGiftCollected -= UpdateText;
        }
    }

    private void UpdateText(int collected, int total)
    {
        if (counterText != null)
        {
            counterText.text = $"{collected} / {total}";
        }
    }
}