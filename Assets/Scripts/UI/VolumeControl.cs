using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer _audioMixer;
    [SerializeField] GameObject _mainMenuButton;
    [SerializeField] Canvas _audioCanvas;
    [SerializeField] Slider _mainVolumeSlider;
    [SerializeField] Slider _musicVolumeSlider;
    [SerializeField] Slider _sfxVolumeSlider;
    [SerializeField] Toggle _mainMuteToggle, _musicMuteToggle, _sfxMuteToggle;
    // [SerializeField] OptionsMenu _optionsMenu;

    InputAction _toggleOptions;

    // void Awake()
    // {
    //     _toggleOptions = InputSystem.actions.FindAction("Options"); // TODO Implement this stuff
    //     _toggleOptions.performed += ToggleAudioCanvas;
    // }

    // void OnDestroy()
    // {
    //     _toggleOptions.performed -= ToggleAudioCanvas;
    // }

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

    // public void IncreaseMainVolumeLevel()
    // {
    //     SetMainVolumeLevel(_mainVolumeSlider.value + 10f);
    // }

    // public void DecreaseMainVolumeLevel()
    // {
    //     SetMainVolumeLevel(_mainVolumeSlider.value - 10f);
    // }

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

    // public void IncreaseMusicVolumeLevel()
    // {
    //     SetMusicVolume(_musicVolumeSlider.value + 0.1f);
    // }

    // public void DecreaseMusicVolumeLevel()
    // {
    //     SetMusicVolume(_musicVolumeSlider.value - 0.1f);
    // }

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

    // public void IncreaseSFXVolumeLevel()
    // {
    //     SetSFXVolume(_sfxVolumeSlider.value + 0.1f);
    // }

    // public void DecreaseSFXVolumeLevel()
    // {
    //     SetSFXVolume(_sfxVolumeSlider.value - 0.1f);
    // }

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

    void ToggleAudioCanvas(InputAction.CallbackContext _)
    {
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
        _audioCanvas.enabled = false;
        Time.timeScale = 1;

        // _optionsMenu.EnableOptionsCanvas();
    }

    void EnableAudioCanvas() // TODO? Setting Time.timeScale here (and in DisableAudioCanvas) isn't ideal
    {
        Time.timeScale = 0;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_mainMenuButton);        
    }
}
