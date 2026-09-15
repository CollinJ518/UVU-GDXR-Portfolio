using UnityEngine;
using UnityEngine.UI; // Remove this if you're not using UI Text placeholders yet

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Settings")]
    public int maxLives = 2;
    private int currentLives;

    [Header("Timer")]
    private float survivalTime = 0f;
    private bool gameActive = true;

    [Header("UI Placeholders (optional, assign in Inspector)")]
    public Text livesText;
    public Text timerText;
    public GameObject gameOverPanel;

    void Awake()
    {
        // Simple singleton so other scripts can call GameManager.Instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentLives = maxLives;
        UpdateUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (!gameActive) return;

        survivalTime += Time.deltaTime;
        UpdateUI();
    }

    // Called by PlayerController when the player is hit
    public void PlayerHit()
    {
        if (!gameActive) return;

        currentLives--;
        UpdateUI();

        if (currentLives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameActive = false;
        Debug.Log("Game Over! Survived: " + survivalTime.ToString("F2") + " seconds");

        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        // Stop the spawner too
        ObjectSpawner spawner = FindObjectOfType<ObjectSpawner>();
        if (spawner != null) spawner.StopSpawning();
    }

    public bool IsGameActive()
    {
        return gameActive;
    }

    public float GetSurvivalTime()
    {
        return survivalTime;
    }

    void UpdateUI()
    {
        if (livesText != null) livesText.text = "Lives: " + currentLives;
        if (timerText != null) timerText.text = "Time: " + survivalTime.ToString("F1") + "s";
    }
}
