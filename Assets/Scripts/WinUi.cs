using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    [Tooltip("Le panneau Victoire. Doit être désactivé au départ dans la scène.")]
    public GameObject winPanel;

    private void Start()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (GiftManager.instance != null)
        {
            GiftManager.instance.OnAllGiftsCollected += ShowWin;
        }
    }

    private void OnDestroy()
    {
        if (GiftManager.instance != null)
        {
            GiftManager.instance.OnAllGiftsCollected -= ShowWin;
        }
    }

    private void ShowWin()
    {
        if (winPanel != null && !winPanel.activeSelf)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}