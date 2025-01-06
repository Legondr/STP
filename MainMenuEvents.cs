using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    public VisualElement ui;

    public Button playButton;
    public Button settingsButton;
    public Button quitButton;

    public VisualElement settingsMenu; // Reference to the settings menu
    public Button backButton; // Button to go back to the main menu from settings

    private bool isPaused = false;

    private void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
        PauseGame(); // Ensure the game starts paused
    }

    private void OnEnable()
    {
        // Assign main menu and settings menu elements
        settingsMenu = ui.Q<VisualElement>("SettingsMenuPanel");
        playButton = ui.Q<Button>("Play-Button");
        settingsButton = ui.Q<Button>("Settings-Button");
        quitButton = ui.Q<Button>("Quit-Button");
        backButton = ui.Q<Button>("Back-Button");

        // Initialize visibility
        ShowMainMenu();

        // Set up button click events
        if (playButton != null) playButton.clicked += OnPlayButtonClicked;
        else Debug.LogError("Play Button not found!");

        if (settingsButton != null) settingsButton.clicked += OnSettingsButtonClicked;
        else Debug.LogError("Settings Button not found!");

        if (quitButton != null) quitButton.clicked += OnQuitButtonClicked;
        else Debug.LogError("Quit Button not found!");

        if (backButton != null) backButton.clicked += OnBackButtonClicked;
        else Debug.LogError("Back Button not found!");
    }

    private void OnQuitButtonClicked()
    {
        Debug.Log("Quitting the game.");
        Application.Quit();
    }

    private void OnSettingsButtonClicked()
    {
        Debug.Log("Open settings.");
        ShowSettingsMenu();
    }

    private void OnPlayButtonClicked()
    {
        TogglePause();
        HideUI();
    }

    private void OnBackButtonClicked()
    {
        Debug.Log("Back to main menu.");
        ShowMainMenu();
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
            ShowUI();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Stops game time
        isPaused = true;
        Debug.Log("Game Paused");
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1f; // Resumes game time
        isPaused = false;
        Debug.Log("Game Unpaused");
    }

    private void HideUI()
    {
        if (ui != null)
        {
            ui.style.display = DisplayStyle.None; // Hide the main UI
        }
    }

    private void ShowUI()
    {
        if (ui != null)
        {
            ui.style.display = DisplayStyle.Flex; // Show the main UI
        }
    }

    private void ShowSettingsMenu()
    {
        if (settingsMenu != null)
        {
            settingsMenu.style.display = DisplayStyle.Flex; // Show settings menu
        }
        
    }

    private void ShowMainMenu()
    {
        if (ui != null)
        {
            ui.style.display = DisplayStyle.Flex; // Show the main menu
        }

        if (settingsMenu != null)
        {
            settingsMenu.style.display = DisplayStyle.None; // Hide the settings menu
        }
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
