using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter instance;
    public Text scoreText;
    public Text highScoreText;

    public int score = 0;

    public int highScore = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + score.ToString();
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    public void AddScore()
    {
		if (Snake.instance.suppressScore) return;

        score++;
        scoreText.text = "Score: " + score.ToString();
        // HighScore expression
        if (score > highScore)
        {
            highScore = score;
			highScoreText.text = "High Score: " + highScore.ToString();
            Debug.Log("High score changed to: " + highScore);
        }
        
    }
}