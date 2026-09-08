using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinUI : MonoBehaviour
{
    [Tooltip("Le panneau Victoire.")]
    public GameObject winPanel;

    [Tooltip("Texte affichant le nombre de cadeaux récupérés en fin de partie.")]
    public TextMeshProUGUI giftsCollectedText;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}