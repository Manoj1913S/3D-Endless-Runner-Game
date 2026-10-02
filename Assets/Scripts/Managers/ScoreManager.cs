using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // Reference to GameOverManager so we can check
    // whether the game has already ended.
    [SerializeField] GameOverManager gameOverManager;

    // Reference to the UI Text component that displays the score.
    [SerializeField] TMP_Text scoreText;

    // Stores the player's current score.
    int score = 0;


    public void IncreaseScore(int scoreAmount)
    {
        // If the game is already over, stop this method immediately.
        // 'return' exits the method, so score will NOT increase.
        if (gameOverManager.GameOver) return;

        // Add the received scoreAmount to the current score.
        // Example: score = 100, scoreAmount = 100 → score = 200
        score += scoreAmount;

        // Convert the integer score into a string because
        // TMP_Text.text requires a string value.
        // Example: 200 → "200"
        scoreText.text = score.ToString();

    }

}
