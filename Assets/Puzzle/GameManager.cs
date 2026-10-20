using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels (Optional)")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompleteLevel()
    {
        if (isGameOver) return;

        Debug.Log("YOU WIN! Level Cleared.");

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        // ตัวอย่าง: โหลดฉากถัดไปหลังจากชนะ 2 วินาที (ปลดล็อกเมื่อมี Scene ถัดไป)
        // Invoke(nameof(LoadNextScene), 2f);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("GAME OVER! Player Died.");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // รีสตาร์ตฉากปัจจุบันหลังจากแพ้ 2 วินาที
        Invoke(nameof(RestartScene), 2f);
    }

    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Game Cleared!");
        }
    }
}