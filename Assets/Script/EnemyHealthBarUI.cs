using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    public Image fillImage;
    public EnemyHealth enemyHealth;

    private void Start()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(enemyHealth.CurrentHealth, enemyHealth.maxHealth);
        }
    }

    private void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnHealthChanged -= UpdateHealthBar;
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