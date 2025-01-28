using UnityEngine;

public class GamePauseHandler : MonoBehaviour
{
    void Update()
    {
        if (Time.timeScale == 0f && Input.anyKeyDown) // If game is paused & any key is pressed
        {
            ResumeGame();
        }
    }

    void ResumeGame()
    {
        Debug.Log("Game Resumed!");
        Time.timeScale = 1f; // Resume game
    }
}