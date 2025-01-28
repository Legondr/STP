using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerOne : Snake
{
    private void Start()
    {
        base.Start();
        InitializeSnake(1,KeyCode.W,KeyCode.S,KeyCode.A, KeyCode.D, Color.green, Vector2Int.right);
        
    }
    private void Update()
    {
        base.Update(); // Call the base method for common input handling

        // PlayerOne-specific input logic (e.g. custom movement controls)
        HandlePlayerInput();
    }
    
    
    
}