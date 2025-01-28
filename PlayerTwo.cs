using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerTwo : Snake
{
    private void Start()
    {
        base.Start();
        InitializeSnake(2, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, Color.cyan, Vector2Int.left);
        
    }
    private void Update()
    {
        base.Update(); // Call the base method for common input handling

        // PlayerOne-specific input logic (e.g. custom movement controls)
        HandlePlayerInput();
    }
    

   
}