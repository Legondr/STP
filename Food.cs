using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class Food : MonoBehaviour
{
    private GameModesEvents gameModesEvents;
    public Collider2D gridArea;
    private Snake snake;
    private bool isMoving = false;
    private Coroutine moveCoroutine;

    private Vector2Int currentDirection;
    private Vector2Int currentGridPosition;

    private void Awake()
    {
        snake = FindObjectOfType<Snake>();
        gameModesEvents = FindObjectOfType<GameModesEvents>();
    }

    private void Start()
    {
        RandomizePosition();

        // Check if Moving Fruit mode is enabled
        if (gameModesEvents.currentGameMode == GameModesEvents.GameMode.MovingFruit)
        {
            isMoving = true;
            StartCoroutine(MoveFood());
        }
    }

    public void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;

        // Pick a random position inside the bounds
        // Round the values to ensure it aligns with the grid
        int x = Mathf.RoundToInt(Random.Range(bounds.min.x, bounds.max.x));
        int y = Mathf.RoundToInt(Random.Range(bounds.min.y, bounds.max.y));

        // Prevent the food from spawning on the snake
        while (snake.Occupies(x, y))
        {
            x++;

            if (x > bounds.max.x)
            {
                x = Mathf.RoundToInt(bounds.min.x);
                y++;

                if (y > bounds.max.y)
                {
                    y = Mathf.RoundToInt(bounds.min.y);
                }
            }
        }

        transform.position = new Vector2(x, y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RandomizePosition();

        if (isMoving)
        {
            StartMoving(); // Restart coroutine from new position
        }

        if (gameModesEvents.currentGameMode == GameModesEvents.GameMode.Classic)
        {
            snake.speed += 0.1f;
            Debug.Log("Increasing snake speed to: " + snake.speed);
        }
    }

    private IEnumerator MoveFood()
    {
        // Initialize position and direction
        currentGridPosition = Vector2Int.RoundToInt(transform.position);
        ChooseNewDirection();

        while (isMoving)
        {
            yield return new WaitForSeconds(0.25f);
            Vector2Int nextPos = currentGridPosition + currentDirection;

            if (IsPositionValid(nextPos))
            {
                currentGridPosition = nextPos;
            }
            else
            {
                ChooseNewDirection();
                continue;
            }

            transform.position = new Vector2(currentGridPosition.x, currentGridPosition.y);
        }
    }

    private void ChooseNewDirection()
    {
        List<Vector2Int> directions = new List<Vector2Int>
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        // Shuffle directions
        for (int i = 0; i < directions.Count; i++)
        {
            Vector2Int temp = directions[i];
            int randomIndex = Random.Range(i, directions.Count);
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }

        foreach (var dir in directions)
        {
            if (IsPositionValid(currentGridPosition + dir))
            {
                currentDirection = dir;
                return;
            }
        }

        // If no direction is valid (boxed in), stay still
        currentDirection = Vector2Int.zero;
    }

    private bool IsPositionValid(Vector2Int pos)
    {
        Bounds bounds = gridArea.bounds;

        // Check bounds
        if (pos.x < Mathf.RoundToInt(bounds.min.x) || pos.x > Mathf.RoundToInt(bounds.max.x) ||
            pos.y < Mathf.RoundToInt(bounds.min.y) || pos.y > Mathf.RoundToInt(bounds.max.y))
            return false;

        // Check if snake occupies this position
        return !snake.Occupies(pos.x, pos.y);
    }
    
    public void StartMoving()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
    
        isMoving = true;
        moveCoroutine = StartCoroutine(MoveFood());
    }

    public void StopMoving()
    {
        isMoving = false;
    }


}