using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tigey : MonoBehaviour
{
    public static event Action OnShopOpened;

    [SerializeField] TextAsset _tigeyIntro;
    [SerializeField] StoryTeller _storyTeller;
    [SerializeField] Canvas _shopCanvas, _defeatCanvas;
    [SerializeField] EnemySpawner _yemmepSpawner;
    [SerializeField] GameObject _retryButton;
    [SerializeField] GameProgressSO _gameProgress;

    bool _isTigeyStoryActive;

    void Awake()
    {
        StoryTeller.OnStoryCanvasClosed += CheckTigeyStory;
        VolumeControl.OnPauseStateChanged += OnPauseStateChanged;
    }

    void OnDestroy()
    {
        StoryTeller.OnStoryCanvasClosed -= CheckTigeyStory;
        VolumeControl.OnPauseStateChanged -= OnPauseStateChanged;
    }

    void Start()
    {
        if(_yemmepSpawner && _gameProgress.YemmepEaten)
        {
            _yemmepSpawner.gameObject.SetActive(true);
        }
    }

    public void OpenShop()
    {
        if(!_gameProgress.TigeyMet)
        {
            _isTigeyStoryActive = true;
            _storyTeller.StartSideStory(_tigeyIntro);
            _gameProgress.SetTigeyMet();
            return;
        }

        EventSystem.current.SetSelectedGameObject(null);
        _defeatCanvas.enabled = false;
        _shopCanvas.enabled = true;
        OnShopOpened?.Invoke();
        EventSystem.current.SetSelectedGameObject(_retryButton);
    }

    void OnPauseStateChanged(bool isPaused)
    {
        if(!isPaused)
        {
            if(_shopCanvas.isActiveAndEnabled)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(_retryButton);
            }
        }
    }

    void CheckTigeyStory()
    {
        if(_isTigeyStoryActive)
        {
            _isTigeyStoryActive = false;

            EventSystem.current.SetSelectedGameObject(null);
            _defeatCanvas.enabled = false;
            _shopCanvas.enabled = true;
            OnShopOpened?.Invoke();
            EventSystem.current.SetSelectedGameObject(_retryButton);
        }
    }
}
