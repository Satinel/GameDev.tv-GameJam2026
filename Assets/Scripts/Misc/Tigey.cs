using UnityEngine;
using UnityEngine.EventSystems;

public class Tigey : MonoBehaviour
{
    [SerializeField] TextAsset _tigeyIntro;
    [SerializeField] StoryTeller _storyTeller;
    [SerializeField] Canvas _shopCanvas, _defeatCanvas;
    [SerializeField] GameProgressSO _gameProgress;
    [SerializeField] EnemySpawner _yemmepSpawner;
    [SerializeField] GameObject _retryButton;

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
        if(_yemmepSpawner && _gameProgress.YemmepEaten)
        {
            _yemmepSpawner.gameObject.SetActive(true);
        }
    }

    public void OpenShop()
    {
        if(!_gameProgress.TigeyMet)
        {
            _storyTeller.StartSideStory(_tigeyIntro);
            _gameProgress.SetTigeyMet();
            return;
        }

        EventSystem.current.SetSelectedGameObject(null);
        _defeatCanvas.enabled = false;
        _shopCanvas.enabled = true;
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
}
