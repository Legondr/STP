using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    private static ScoreCounter Instance; // Singleton for easy access

    public Text playerOneScoreText;
    public Text playerTwoScoreText; 
    public Text playerOneHighScoreText;
    public Text playerTwoHighScoreText;
    public Text winnerMessageText;
    

    public int playerOneScore = 0;
    public int playerTwoScore = 0;
    
    public int playerOneHighScore = 0;
    public int playerTwoHighScore = 0;

    public bool isTwoPlayerMode = false; // Toggle for 2-player mode
    public ScoreCounter scoreCounter;

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

    private void Start()
    {
        winnerMessageText.enabled = false;
        UpdateUI();
        scoreCounter = FindObjectOfType<ScoreCounter>();  // Get the ScoreCounter instance
        if (scoreCounter == null)
        {
            Debug.LogError("ScoreCounter not found in the scene!");
        }
    }
    
    public void SetTwoPlayerMode(bool isTwoPlayers)
    {
        winnerMessageText.enabled = true;
        isTwoPlayerMode = isTwoPlayers;
        UpdateUI();
    }

    public void AddScore(int playerID)
    {
        if (playerID == 1)
        {
            playerOneScore += 1;
            Debug.Log("Player 1 Score: " + playerOneScore);
        }
        else if (playerID == 2)
        {
            playerTwoScore += 1;
            Debug.Log("Player 2 Score: " + playerTwoScore);
        }
        if (playerOneScore > playerOneHighScore)
        {
            playerOneHighScore = playerOneScore;
        }

        if (playerTwoScore > playerTwoHighScore)
        {
            playerTwoHighScore = playerTwoScore;
        }

        UpdateUI();
    }

    // Enable or disable Player 2's score UI
    public void SetPlayerTwoScoreVisibility(bool isVisible)
    {
        if (playerTwoScoreText != null)
        {
            playerTwoScoreText.gameObject.SetActive(isVisible);
        }
    }
    
    // Reset scores
    public void ResetScores()
    {
        playerOneScore = 0;
        playerTwoScore = 0;
        UpdateUI();
        
        Time.timeScale = 0f; // Pause the game
        Debug.Log("Game Paused: Press any key to restart.");
    }
    private void UpdateUI()
    {
        if (isTwoPlayerMode)
        {
            playerOneScoreText.text = "Player1' Score: " + playerOneScore;
            playerOneHighScoreText.text = "Highscore: " + playerOneHighScore;
            playerTwoScoreText.text = "Player2' Score: " + playerTwoScore;
            playerTwoHighScoreText.text = "Highscore: " + playerTwoHighScore;

            playerTwoScoreText.gameObject.SetActive(true);
            playerTwoHighScoreText.gameObject.SetActive(true);
        }
        else
        {
            playerOneScoreText.text = "Score: " + playerOneScore;
            playerOneHighScoreText.text = "High Score: " + playerOneHighScore;

            playerTwoScoreText.gameObject.SetActive(false);
            playerTwoHighScoreText.gameObject.SetActive(false);
        }
    }
    
    public void DisplayWinner(string winnerMessage)
    {
        if (winnerMessageText != null)
        {
            winnerMessageText.text = winnerMessage;  // Set the text to show the winner
            winnerMessageText.enabled = true;  // Make sure the text is visible
        }
        else
        {
            Debug.LogError("Winner text is not assigned in the inspector!");
        }
    }

    
}
