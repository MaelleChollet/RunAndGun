using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image fillImage;

    private void Start()
    {
        if (PlayerHealth.instant != null)
        {
            PlayerHealth.instant.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(PlayerHealth.instant.CurrentHealth, PlayerHealth.instant.maxHealth);
        }
    }

    private void OnDestroy()
    {
        if (PlayerHealth.instant != null)
        {
            PlayerHealth.instant.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(int current, int max)
    {
        if (fillImage == null) return;

        float ratio = (float)current / max;
        fillImage.fillAmount = ratio;

        if (ratio > 0.5f) fillImage.color = Color.green;
        else if (ratio > 0.25f) fillImage.color = new Color(1f, 0.6f, 0f); // orange
        else fillImage.color = Color.red;
    }


}