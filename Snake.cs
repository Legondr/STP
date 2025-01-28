using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class Snake : MonoBehaviour
{
    public Transform segmentPrefab;
    public Vector2Int direction;
    
    public float speed = 15f;
    public float speedMultiplier = 1f;
    public int initialSize = 4;
    public bool moveThroughWalls = false;
    public bool isAlive = true;
    public int playerID; // Unique ID for each snake

    private readonly List<Transform> segments = new List<Transform>();
    public Vector2Int input;
    private float nextUpdate;
    
    private ScoreCounter scoreCounter;
    private GameModesEvents gameModesEvents;
    public bool suppressScore = false;
    
    private SpriteRenderer spriteRenderer;
    protected KeyCode upKey, downKey, leftKey, rightKey; // Key bindings

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void InitializeSnake(int id, KeyCode up, KeyCode down, KeyCode left, KeyCode right, Color snakeColor, Vector2Int newDirection)
    {
        playerID = id;
        upKey = up;
        downKey = down;
        leftKey = left;
        rightKey = right;
        spriteRenderer.color = snakeColor; // Set color to differentiate players
        direction = newDirection;
    }

    protected void Start()
    {
        scoreCounter = FindObjectOfType<ScoreCounter>();
        if (scoreCounter == null)
        {
            Debug.LogError("ScoreCounter not found!");
        }				
        gameModesEvents = FindObjectOfType<GameModesEvents>();
        if (gameModesEvents == null)
        {
            Debug.LogError("GameModesEvents not found!");  
        }
        ResetState();
    }

    protected void Update()
    {
        HandlePlayerInput();
    }

    protected void FixedUpdate()
    {
        if (Time.time < nextUpdate) 
            return;

        if (input != Vector2Int.zero)
            direction = input;

        for (int i = segments.Count - 1; i > 0; i--) 
            segments[i].position = segments[i - 1].position;

        int x = Mathf.RoundToInt(transform.position.x) + direction.x;
        int y = Mathf.RoundToInt(transform.position.y) + direction.y;
        transform.position = new Vector2(x, y);

        nextUpdate = Time.time + (1f / (speed * speedMultiplier));
    }
    
    public bool Occupies(int x, int y)
    {
        foreach (Transform segment in segments)
        {
            if (Mathf.RoundToInt(segment.position.x) == x && Mathf.RoundToInt(segment.position.y) == y) 
            {
                return true;
            }
        }
        return false;
    }
    
    public void SetMoveThroughWalls(bool value)
    {
        moveThroughWalls = value;
        Debug.Log($"MoveThroughWalls is now: {moveThroughWalls}");
    }

    private void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = segments[segments.Count - 1].position;
        
        // Set the color of the segment to match the snake color (yellow for the second snake)
        SpriteRenderer segmentRenderer = segment.GetComponent<SpriteRenderer>();
        if (segmentRenderer != null)
        {
            segmentRenderer.color = spriteRenderer.color; // Match the snake's color
        }
        
        segments.Add(segment);

        if (!suppressScore)
        {
            scoreCounter.AddScore(playerID);
        }
    }
    

    private void ResetState()
    {
        isAlive = true;
        suppressScore = true;
        transform.position = Vector3.zero;

        for (int i = 1; i < segments.Count; i++) 
        {
            Destroy(segments[i].gameObject);
        }

        segments.Clear();
        segments.Add(transform);

        for (int i = 0; i < initialSize - 1; i++) 
        {
            Grow();
        }

        suppressScore = false;
        scoreCounter.ResetScores();
    }

    protected void HandlePlayerInput()
    {
        if (direction.x != 0f)
        {
            if (Input.GetKeyDown(upKey))
                input = Vector2Int.up;
            else if (Input.GetKeyDown(downKey))
                input = Vector2Int.down;
        }
        else if (direction.y != 0f)
        {
            if (Input.GetKeyDown(leftKey))
                input = Vector2Int.left;
            else if (Input.GetKeyDown(rightKey))
                input = Vector2Int.right;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Grow();
        }
        else if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Player"))
        {
            isAlive = false;
            gameModesEvents.CheckWinner(); // Call the winner check
            gameModesEvents.RespawnPlayers();

            if (gameModesEvents.currentGameMode != GameModesEvents.GameMode.OneVsOne)
            {
                ResetState();
            }

        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            if (moveThroughWalls) 
            {
                Traverse(other.transform);
            } 
            else 
            {
                isAlive = false;
                gameModesEvents.CheckWinner(); // Call the winner check
                gameModesEvents.RespawnPlayers();
                if (gameModesEvents.currentGameMode != GameModesEvents.GameMode.OneVsOne)
                {
                    ResetState();
                }
            }
        }
    }

    private void Traverse(Transform wall)
    {
        Vector3 position = transform.position;

        if (direction.x != 0f) 
        {
            position.x = Mathf.RoundToInt(-wall.position.x + direction.x);
        } 
        else if (direction.y != 0f) 
        {
            position.y = Mathf.RoundToInt(-wall.position.y + direction.y);
        }
        transform.position = position;
    }
    
    public void ResetSnake()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("BodySegment"))
            {
                Destroy(child.gameObject);
            }
        }
        SetDirection(direction);
        input = direction;
        isAlive = true;
    }
    public void SetDirection(Vector2Int newDirection)
    {
        if (newDirection != Vector2.zero) // Prevent setting an invalid direction
        {
            direction = newDirection;
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }
    
}
