using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public static event Action<bool> OnPauseStateChanged;
    public static event Action OnUnpausedWithStoryActive;
    public static event Action OnEnableAudioCanvas, OnDisableAudioCanvas;
    public static event Action OnRequestRestart;

    public AudioMixer _audioMixer;
    [SerializeField] GameObject _mainMenuButton, _quitPromptParent, _cancelQuitButton, _quitButton;
    [SerializeField] Canvas _audioCanvas;
    [SerializeField] Slider _mainVolumeSlider;
    [SerializeField] Slider _musicVolumeSlider;
    [SerializeField] Slider _sfxVolumeSlider;
    [SerializeField] Toggle _mainMuteToggle, _musicMuteToggle, _sfxMuteToggle;

    bool _isLevelReady, _isStoryActive;

    void Awake()
    {
#if UNITY_WEBGL
        _quitButton.SetActive(false);
#endif
    }

    void OnEnable()
    {
        InputManager.OnOptionsAction += ToggleAudioCanvas;

        LevelManager.OnLevelReady += OnLevelReady;

        StoryTeller.OnStoryStarted += OnStoryStarted;
        StoryTeller.OnStoryCanvasClosed += OnStoryClosed;
    }

    void OnDisable()
    {
        InputManager.OnOptionsAction -= ToggleAudioCanvas;

        LevelManager.OnLevelReady -= OnLevelReady;

        StoryTeller.OnStoryStarted -= OnStoryStarted;
        StoryTeller.OnStoryCanvasClosed -= OnStoryClosed;
    }

    void Start()
    {
        _mainVolumeSlider.value = PlayerPrefs.GetFloat("MainVolume", 1);
        _musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        _sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);

        _mainMuteToggle.isOn = PlayerPrefs.GetInt("MainMuted", 0) == 1;
        ToggleMuteMainVolume();

        _musicMuteToggle.isOn = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        ToggleMuteMusicVolume();

        _sfxMuteToggle.isOn = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        ToggleMuteSFXVolume();
    }

    public void SetMainVolumeLevel(float sliderValue)
    {
        PlayerPrefs.SetFloat("MainVolume", sliderValue);

        if(_mainMuteToggle.isOn) { return; }

        _audioMixer.SetFloat("MainVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void ToggleMuteMainVolume()
    {
        if(_mainMuteToggle.isOn)
        {
            PlayerPrefs.SetInt("MainMuted", 1);
            _audioMixer.SetFloat("MainVolume", Mathf.Log10(0.0001f) * 20);  // 0.0001f works but 0 doesn't because Log10 stuff I don't understand
        }
        else
        {
            PlayerPrefs.SetInt("MainMuted", 0);
            SetMainVolumeLevel(_mainVolumeSlider.value);
        }
    }

    public void SetMusicVolume(float sliderValue)
    {
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);

        if(_musicMuteToggle.isOn) { return; }

        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void ToggleMuteMusicVolume()
    {
        if(_musicMuteToggle.isOn)
        {
            PlayerPrefs.SetInt("MusicMuted", 1);
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(0.0001f) * 20);  // 0.0001f works but 0 doesn't because Log10 stuff I don't understand
        }
        else
        {
            PlayerPrefs.SetInt("MusicMuted", 0);
            SetMusicVolume(_musicVolumeSlider.value);
        }
    }

    public void SetSFXVolume(float sliderValue)
    {
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);

        if(_sfxMuteToggle.isOn) { return; }

        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void ToggleMuteSFXVolume()
    {
        if(_sfxMuteToggle.isOn)
        {
            PlayerPrefs.SetInt("SFXMuted", 1);
            _audioMixer.SetFloat("SFXVolume", Mathf.Log10(0.0001f) * 20);  // 0.0001f works but 0 doesn't because Log10 stuff I don't understand
        }
        else
        {
            PlayerPrefs.SetInt("SFXMuted", 0);
            SetSFXVolume(_sfxVolumeSlider.value);
        }
    }

    void ToggleAudioCanvas()
    {
        if(!_isLevelReady) { return; }

        _audioCanvas.enabled = !_audioCanvas.enabled;
        if(_audioCanvas.enabled)
        {
            EnableAudioCanvas();
        }
        else
        {
            DisableAudioCanvas();
        }
    }

    public void DisableAudioCanvas()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _audioCanvas.enabled = false;
        UnpauseGame();
        OnDisableAudioCanvas?.Invoke();
    }

    void EnableAudioCanvas() // TODO? Setting Time.timeScale here (and in DisableAudioCanvas) isn't ideal
    {
        OnEnableAudioCanvas?.Invoke();
        PauseGame();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_mainMenuButton);
    }

    void OnStoryStarted()
    {
        _isStoryActive = true;
        PauseGame();
    }

    void OnStoryClosed()
    {
        _isStoryActive = false;
        UnpauseGame();
    }

    void PauseGame()
    {
        OnPauseStateChanged?.Invoke(true);
        Time.timeScale = 0;
    }

    void UnpauseGame()
    {
        if(_isStoryActive)
        {
            OnUnpausedWithStoryActive?.Invoke();
            return;
        }

        Time.timeScale = 1;
        OnPauseStateChanged?.Invoke(false);
    }

    void OnLevelReady()
    {
        _isLevelReady = true;
    }

    public void RequestRestart()
    {
        OnRequestRestart?.Invoke();
        _audioCanvas.enabled = false;
        Time.timeScale = 1;
    }

    public void PromptQuit()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _quitPromptParent.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_cancelQuitButton);
    }

    public void CancelQuit()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _quitPromptParent.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_mainMenuButton);
    }

    public void ConfirmQuitGame()
    {
        Application.Quit();
    }
}
