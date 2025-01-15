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
        score = 0;
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    // Update is called once per frame
    public void AddScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
    }
}