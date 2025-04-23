using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameModesEvents : MonoBehaviour
{
    private RadioButtonGroup modes;

    public GameObject snakePrefab; // Assign in Inspector
    private Snake snake;
    private Snake playerOne;
    private Snake playerTwo;
    private GameObject playerTwoObj; // Store Player 2 instance
    private ScoreCounter scoreCounter;

    public enum GameMode
    {
        Normal,
        Classic,
        OneVsOne,
        Invisible,
        Mirror,
        MovingFruit
    }

    public GameMode currentGameMode;

    void Start()
    {
        var mainMenu = GameObject.Find("MainMenu");
        var root = mainMenu.GetComponent<UIDocument>().rootVisualElement;
        modes = root.Q<RadioButtonGroup>("Modes-RadioGroup");

        if (modes == null)
        {
            throw new Exception("Modes-RadioGroup not found");
        }

        Debug.Log($"Initial Selected Mode Index: {modes.value}");
        currentGameMode = (GameMode)modes.value;

        modes.RegisterValueChangedCallback(evt =>
        {
            Debug.Log($"Selected Mode Index: {evt.newValue}");

            // Update game mode based on selected index
            currentGameMode = (GameMode)evt.newValue;

            // Handle game mode change
            ChangeGameMode(currentGameMode);
        });

        // Call ChangeGameMode initially
        ChangeGameMode(currentGameMode);

        scoreCounter = FindObjectOfType<ScoreCounter>(); // Find the ScoreCounter script
        if (scoreCounter == null)
        {
            Debug.LogError("ScoreCounter not found in the scene! Make sure it exists.");
        }
    }

    private void DestroyPlayerTwo()
    {
        if (playerTwo != null)
        {
            foreach (Transform segment in playerTwo.GetSegments())
            {
                Destroy(segment.gameObject); // Destroy all segments
            }
        }

        Destroy(playerTwoObj); // Destroy Player Two completely
        playerTwo = null;
        playerTwoObj = null;
        scoreCounter.SetTwoPlayerMode(false);
    }


    private void ChangeGameMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.Normal:
                Debug.Log("Game Mode: Normal");
                NormalGameMode();
                break;

            case GameMode.Classic:
                Debug.Log("Game Mode: Classic");
                ClassicGameMode();
                break;

            case GameMode.OneVsOne:
                Debug.Log("Game Mode: 1vs1");
                OneVsOneGameMode();
                break;

            case GameMode.Invisible:
                Debug.Log("Game Mode: Invisible");
                InvisibleGameMode();
                break;

            case GameMode.Mirror:
                Debug.Log("Game Mode: Mirror");
                MirrorGameMode();
                break;

            case GameMode.MovingFruit:
                Debug.Log("Game Mode: MovingFruit");
                MovingFruitGameMode();
                break;

            default:
                Debug.Log("Unknown Game Mode");
                break;
        }
    }

    private void NormalGameMode()
    {
        StopAllModesModifications();
        scoreCounter.winnerMessageText.enabled = false;
        Debug.Log("Starting Normal Game Mode");
        snake.speed = 15f;

        if (playerTwoObj != null)
        {
            DestroyPlayerTwo(); // Fully remove Player Two
        }
    }

    private void ClassicGameMode()
    {
        StopAllModesModifications();
        scoreCounter.winnerMessageText.enabled = false;
        Debug.Log("Starting Classic Game Mode");
        snake.speed = 15;

        if (playerTwoObj != null)
        {
            DestroyPlayerTwo(); // Fully remove Player Two
        }
    }

    private void OneVsOneGameMode()
    {
        StopAllModesModifications();
        Debug.Log("Starting 1vs1 Game Mode");

        // Find Player One
        playerOne = FindObjectOfType<PlayerOne>();
        if (playerOne == null)
        {
            Debug.LogError("PlayerOne not found in the scene!");
            return;
        }

        // Instantiate or Reactivate Player Two
        if (playerTwoObj == null)
        {
            playerTwoObj = Instantiate(snakePrefab, new Vector3(-2, 0, 0), Quaternion.identity); //here
            playerTwoObj.name = "PlayerTwoSnake";
            playerTwo = playerTwoObj.AddComponent<PlayerTwo>();
            playerTwoObj.tag = "Player";


            // Assign Segment Prefab from PlayerOne to PlayerTwo
            playerTwo.segmentPrefab = playerOne.segmentPrefab;

            // Ensure SpriteRenderer exists
            SpriteRenderer sr = playerTwoObj.GetComponent<SpriteRenderer>();
            if (sr == null)
            {
                sr = playerTwoObj.AddComponent<SpriteRenderer>(); // Add if missing
            }

            // Assign a sprite
            sr.sprite = playerOne.GetComponent<SpriteRenderer>().sprite; // Use the same sprite
            sr.color = Color.cyan; // Different color for PlayerTwo
        }

        // Add Rigidbody2D if missing
        Rigidbody2D rb = playerTwoObj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = playerTwoObj.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic; // Use Kinematic or Dynamic if needed
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        // Add Collider2D if missing
        BoxCollider2D col = playerTwoObj.GetComponent<BoxCollider2D>();
        if (col == null)
        {
            col = playerTwoObj.AddComponent<BoxCollider2D>();
        }
        else
        {
            playerTwoObj.SetActive(true); // Reactivate if it exists
        }

        col.isTrigger = true; // Set to false if you want solid collisions

        // Reset scale before setting size
        playerTwoObj.transform.localScale = Vector3.one;

        // Force Collider size
        col.size = new Vector2(0.75f, 0.75f);
        col.offset = Vector2.zero;

        // Force Unity to update the collider immediately
        col.enabled = false;
        col.enabled = true;

        scoreCounter.SetTwoPlayerMode(true);

        Debug.Log("1 vs 1 Mode: PlayerTwo added!");

        RespawnPlayers();
    }

    public void RespawnPlayers()
    {
        if (currentGameMode == GameMode.OneVsOne)
        {
            Debug.Log("Respawning Players...");

            // Clear old body segments (if applicable)
            playerOne.ResetSnake();
            playerTwo.ResetSnake();

            // Set Player One's position and facing direction
            playerOne.transform.position = new Vector3(2, 0, 0); // Left side
            playerOne.SetDirection(Vector2Int.right); // Facing right
            playerOne.input = Vector2Int.right; // Ensure movement starts correctly

            // Set Player Two's position and facing direction
            playerTwoObj.transform.position = new Vector3(-2, 0, 0); // Right side
            playerTwo.SetDirection(Vector2Int.left); // Facing left
            playerTwo.input = Vector2Int.left; // Ensure movement starts correctly


            // Restart game
            Time.timeScale = 0f; // Pause the game until key is pressed
            Debug.Log("Game Paused: Press any key to restart.");
        }
    }

    public void CheckWinner()
    {
        string winner = "";
        if (currentGameMode == GameMode.OneVsOne)
        {
            if (playerOne.IsAlive() && !playerTwo.IsAlive())
            {
                winner = "Player One Wins!";
                Debug.Log("Player One Wins!");
            }
            else if (!playerOne.IsAlive() && playerTwo.IsAlive())
            {
                winner = "Player Two Wins!";
                Debug.Log("Player Two Wins!");
            }
            else if (!playerOne.IsAlive() && !playerTwo.IsAlive())
            {
                winner = "Tie!";
                Debug.Log("It's a Tie!");
            }
            else if (scoreCounter.playerOneScore == 10)
            {
                winner = "Player One Wins!";
                Debug.Log("Player One Wins!");
            }
            else if (scoreCounter.playerTwoScore == 10)
            {
                winner = "Player Two Wins!";
                Debug.Log("Player Two Wins!");
            }

            scoreCounter.DisplayWinner(winner);
        }
    }

    private void InvisibleGameMode()
    {
        StopAllModesModifications();
        Debug.Log("Starting Invisible Game Mode");

        // Find Player One
        playerOne = FindObjectOfType<PlayerOne>();
        if (playerOne == null)
        {
            Debug.LogError("PlayerOne not found in the scene!");
        }

        if (playerTwoObj != null)
        {
            DestroyPlayerTwo(); // Fully remove Player Two
        }

        playerOne.StartInvisibleMode();
    }

    private void MirrorGameMode()
    {
        StopAllModesModifications();
        // Find Player One
        playerOne = FindObjectOfType<PlayerOne>();
        if (playerOne == null)
        {
            Debug.LogError("PlayerOne not found in the scene!");
        }

        if (playerTwoObj != null)
        {
            DestroyPlayerTwo(); // Fully remove Player Two
        }
    }

    private void MovingFruitGameMode()
    {
        Debug.Log("Starting Moving Fruit Game Mode");

        // Remove Player Two if active
        if (playerTwoObj != null)
        {
            DestroyPlayerTwo();
        }

        // Optionally reset or trigger movement on food
        Food food = FindObjectOfType<Food>();
        if (food != null)
        {
            food.RandomizePosition(); // Optional: re-place the food

            // If you made the MoveFood coroutine public:
            food.StartMoving(); // <-- You can make this helper method inside Food
        }
        else
        {
            Debug.LogWarning("Food not found in scene.");
        }
    }

    private void StopAllModesModifications()
    {
        Food food = FindObjectOfType<Food>();
        if (food != null)
        {
            food.StopMoving();
        }

        playerOne = FindObjectOfType<PlayerOne>();
        if (playerOne != null)
        {
            playerOne.ResetSnake(); // reset segments and state
        }

        if (playerTwoObj != null)
        {
            DestroyPlayerTwo();
        }
        
        Time.timeScale = 1f;
        
        scoreCounter.winnerMessageText.enabled = false;
    }
}