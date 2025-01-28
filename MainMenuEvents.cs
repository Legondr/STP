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

    public VisualElement settingsMenu;
    public Button backButton;

    public VisualElement modesMenu; // Modes menu
    public Button modesButton; // Modes button
    public Button modesBackButton; // Back button in modes menu
	public Button modesPlayButton; // Add this line at the top


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
        modesMenu = ui.Q<VisualElement>("ModesMenuPanel");

        playButton = ui.Q<Button>("Play-Button");
        settingsButton = ui.Q<Button>("Settings-Button");
        quitButton = ui.Q<Button>("Quit-Button");
        backButton = ui.Q<Button>("Back-Button");
        modesButton = ui.Q<Button>("Modes-Button");
        modesBackButton = ui.Q<Button>("ModesBack-Button");
		modesPlayButton = ui.Q<Button>("ModesPlay-Button");

        // Initialize visibility
        ShowMainMenu();

        // Set up button click events
        if (playButton != null) playButton.clicked += OnPlayButtonClicked;
        if (settingsButton != null) settingsButton.clicked += OnSettingsButtonClicked;
        if (quitButton != null) quitButton.clicked += OnQuitButtonClicked;
        if (backButton != null) backButton.clicked += OnBackButtonClicked;
        if (modesButton != null) modesButton.clicked += OnModesButtonClicked;
        if (modesBackButton != null) modesBackButton.clicked += OnModesBackButtonClicked;
		if (modesPlayButton != null) modesPlayButton.clicked += OnModesPlayButtonClicked;
		

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

    private void OnModesButtonClicked()
    {
        Debug.Log("Open modes menu.");
        ShowModesMenu();
    }

    private void OnModesBackButtonClicked()
    {
        Debug.Log("Back to main menu from modes.");
        ShowMainMenu();
    }

	private void OnModesPlayButtonClicked()
	{
		Debug.Log("Start game from modes");
		TogglePause();
        HideUI();
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

    private void ShowModesMenu()
    {
        if (modesMenu != null)
        {
            modesMenu.style.display = DisplayStyle.Flex; // Show modes menu
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

        if (modesMenu != null)
        {
            modesMenu.style.display = DisplayStyle.None; // Hide the modes menu
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
