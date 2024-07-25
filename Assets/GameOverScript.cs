using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public GameObject gameOverPanel; // The panel containing the buttons
    public Button retryButton;       // Retry button
    public Button homeButton;        // Home button

    private bool isGameOver = false;

    void Start()
    {
        // Ensure the Game Over panel is not visible at the start
        gameOverPanel.SetActive(false);

        // Assign button click listeners
        retryButton.onClick.AddListener(RestartGame);
        homeButton.onClick.AddListener(GoToHome);
    }

    void Update()
    {
        // Placeholder for player death detection
        if (!isGameOver && PlayerIsDead()) // Replace with actual condition for player's death
        {
            GameOver();
        }
    }

    bool PlayerIsDead()
    {
        // Implement your own logic to check if the player is dead
        // This is a placeholder function
        return false; // Replace with actual condition
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0; // Pause the game
        gameOverPanel.SetActive(true); // Show the Game Over panel
    }

    void RestartGame()
    {
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the current scene
    }

    void GoToHome()
    {
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene("Home"); // Load the Home scene
    }
}
