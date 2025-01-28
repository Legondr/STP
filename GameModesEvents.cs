using UnityEngine;
using UnityEngine.UIElements;

public class GameModesEvents : MonoBehaviour
{
    private VisualElement root;
    private RadioButtonGroup modes;

    // Enum for game modes (example)
    public enum GameMode
    {
        Normal,
        Classic,
        OneVsOne
    }

    private GameMode currentGameMode;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        modes = root.Q<RadioButtonGroup>("Modes-RadioGroup");

        if (modes != null)
        {
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
        }
    }

    // Method to change the game mode
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

            default:
                Debug.Log("Unknown Game Mode");
                break;
        }
    }
	private void NormalGameMode()
	{
	
	}

	private void ClassicGameMode()
	{
	
	}

	private void OneVsOneGameMode()
	{
	
	}
}
