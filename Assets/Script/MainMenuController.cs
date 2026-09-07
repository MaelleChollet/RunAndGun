using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Tooltip("Nom exact de la scène du premier niveau (doit correspondre au nom du fichier de scène).")]
    [SerializeField] private string firstLevelSceneName = "SampleScene";

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(firstLevelSceneName);
    }

}