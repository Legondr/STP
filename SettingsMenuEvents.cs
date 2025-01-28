using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public VisualElement settingsMenuUI; // Reference to the settings menu UI
    public SliderInt speedSlider; // Slider for adjusting the game speed
    public Slider volumeSlider; // Slider for adjusting the volume
    public Toggle moveThroughWallsToggle; // Checkbox for enabling/disabling moving through walls
    public AudioMixer audioMixer; // Reference to the AudioMixer for controlling volume

    private Snake snakeScript; // Reference to the Snake script
   

    private void Awake()
    {
        settingsMenuUI = GetComponent<UIDocument>().rootVisualElement;

        // Find the elements in the settings menu
        speedSlider = settingsMenuUI.Q<SliderInt>("Speed-Slider");
        volumeSlider = settingsMenuUI.Q<Slider>("Volume-Slider");
        moveThroughWallsToggle = settingsMenuUI.Q<Toggle>("MoveThroughWalls-CheckBox");

        // Find the Snake script in the scene
        snakeScript = FindObjectOfType<Snake>();
        if (snakeScript == null)
        {
            Debug.LogError("Snake script not found in the scene.");
        }

        // Set default values for UI elements
        if (speedSlider != null)
        {
            speedSlider.value = 20; // Default speed value
            speedSlider.RegisterValueChangedCallback(evt => OnSpeedChanged(evt.newValue));
        }
        else
        {
            Debug.LogError("Speed Slider not found!");
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = 0.5f; // Default volume (50%)
            volumeSlider.RegisterValueChangedCallback(evt => OnVolumeChanged(evt.newValue));
        }
        else
        {
            Debug.LogError("Volume Slider not found!");
        }

        if (moveThroughWallsToggle != null)
        {
            moveThroughWallsToggle.value = snakeScript.moveThroughWalls; // Set based on Snake script
            moveThroughWallsToggle.RegisterValueChangedCallback(evt => OnMoveThroughWallsChanged(evt.newValue));
        }
        else
        {
            Debug.LogError("MoveThroughWalls Toggle not found!");
        }
    }

    private void OnSpeedChanged(int newSpeed)
    {
        Debug.Log($"Game speed changed to: {newSpeed}");
        if (snakeScript != null)
        {
            snakeScript.speed = newSpeed;
        }
    }

    private void OnVolumeChanged(float newVolume)
    {
        Debug.Log($"Volume changed to: {newVolume}");
        if (audioMixer != null)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(newVolume) * 20); // Convert to decibel scale
        }
        else
        {
            Debug.LogWarning("AudioMixer not assigned. Cannot adjust volume.");
        }
    }

    private void OnMoveThroughWallsChanged(bool isAllowed)
    {
        Debug.Log($"Move through walls toggled: {isAllowed}");
        if (snakeScript != null)
        {
            snakeScript.SetMoveThroughWalls(isAllowed);
        }
    }
}
