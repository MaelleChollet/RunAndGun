using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Tooltip("Le panneau (GameObject) contenant le texte 'Game Over' et le bouton Rejouer.")]
    public GameObject gameOverPanel;

    [Tooltip("Texte affichant le nombre de cadeaux récupérés en fin de partie.")]
    public TextMeshProUGUI giftsCollectedText;

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (PlayerHealth.instant != null)
        {
            PlayerHealth.instant.OnPlayerDied += ShowGameOver;
        }
    }

    private void OnDestroy()
    {
        if (PlayerHealth.instant != null)
        {
            PlayerHealth.instant.OnPlayerDied -= ShowGameOver;
        }
    }

    private void ShowGameOver()
    {
        if (gameOverPanel != null && !gameOverPanel.activeSelf)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
            UpdateGiftsText();
        }
    }

    private void UpdateGiftsText()
    {
        if (giftsCollectedText != null && GiftManager.instance != null)
        {
            giftsCollectedText.text = $"{GiftManager.instance.CollectedGifts} / {GiftManager.instance.totalGifts}";
        }
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}