using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainTitle : MonoBehaviour
{
    public static event Action OnGameReady;

    [SerializeField] SceneTransitions _scenTransitions;
    [SerializeField] GameProgressSO _progress;
    [SerializeField]  GameObject _startButton;

    void Awake()
    {
        VolumeControl.OnPauseStateChanged += OnPauseStateChanged;
    }

    void OnDestroy()
    {
        VolumeControl.OnPauseStateChanged -= OnPauseStateChanged;
    }

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_startButton);
        OnGameReady?.Invoke();
    }

    void OnPauseStateChanged(bool state)
    {
        if(!state)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_startButton);
        }
    }

    public void StartGame()
    {
        _progress.Reset();
        _scenTransitions.RequestNextLevel();
    }

}
