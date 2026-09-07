using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Tooltip("Le panneau (GameObject) contenant le texte 'Game Over' et le bouton Rejouer. Doit être désactivé au départ dans la scène.")]
    public GameObject gameOverPanel;

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
            Time.timeScale = 0f; // met le jeu en pause derrière l'écran Game Over
        }
    }

    public void Replay()
    {
        Time.timeScale = 1f; // sinon la scène rechargée reste en pause
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}