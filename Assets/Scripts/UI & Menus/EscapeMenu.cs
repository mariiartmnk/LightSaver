using UnityEngine;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuPanel;
    public GameObject optionsPanel;
    public GameObject controlsPanel;
    public GameObject backgroundOverlay;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private bool isMenuOpen = false;
    void Start()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (backgroundOverlay != null) backgroundOverlay.SetActive(false);
    }

    void Update()
    {
        if(PauseController.IsGamePaused) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (pauseMenuPanel == null) return;

        isMenuOpen = !isMenuOpen;
        pauseMenuPanel.SetActive(isMenuOpen);

        if (backgroundOverlay != null) 
            backgroundOverlay.SetActive(isMenuOpen);

        if (isMenuOpen)
        {
            if (SceneLoader.Instance != null) SceneLoader.Instance.PauseGame();
            if (optionsPanel != null) optionsPanel.SetActive(false);
            if (controlsPanel != null) controlsPanel.SetActive(false);
        }
        else
        {
            if (SceneLoader.Instance != null) SceneLoader.Instance.ResumeGame();
        }
    }

    public void Resume()
    {
        ToggleMenu();
    }

    public void OpenOptions()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    public void OpenControls()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    public void ExitGame()
    {
        SceneLoader.Instance.QuitGame();
    }

    public void SetMusicVolume(float value)
    {
        if (MenuAudioManager.Instance != null) 
            MenuAudioManager.Instance.SetMusicVolume(value);
        
        if (AudioManager.Instance != null) 
            AudioManager.Instance.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        if (MenuAudioManager.Instance != null) 
            MenuAudioManager.Instance.SetSFXVolume(value);
            
        if (AudioManager.Instance != null) 
            AudioManager.Instance.SetSFXVolume(value);
    }
}
