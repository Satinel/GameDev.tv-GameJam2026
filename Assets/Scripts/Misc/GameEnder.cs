using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameEnder : MonoBehaviour
{
    public static event Action OnFinalPlayed;

    [SerializeField] StoryTeller _storyTeller;
    [SerializeField] TextAsset _penultimateStory, _gameEndStory;
    [SerializeField] Animator _animator;
    [SerializeField] Canvas _endScreen;
    [SerializeField] GameObject _finalScene, _mainMenuButton;
    [SerializeField] PemmingController _pemmingController;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] IntReferenceSO _totalFishEaten, _totalFishZapped;
    [SerializeField] TextMeshProUGUI _fishText, _zapText;

    bool _hasStartedPenultimateStory, _hasStartedLastStory;
    static readonly int END_HASH = Animator.StringToHash("End");

    void Awake()
    {
        LevelManager.OnLevelFinished += EndGame;
        StoryTeller.OnStoryCanvasClosed += ActivateObject;
    }

    void OnDestroy()
    {
        LevelManager.OnLevelFinished -= EndGame;
        StoryTeller.OnStoryCanvasClosed -= ActivateObject;
    }

    void Start()
    {
        _fishText.text = $"Total Fish Eaten: {_totalFishEaten.Value}";
        _zapText.text = $"Total Fish Zapped: {_totalFishZapped.Value}";
    }

    void EndGame()
    {
        if(!_hasStartedPenultimateStory)
        {
            EventSystem.current.SetSelectedGameObject(null);
            _pemmingController.gameObject.SetActive(false);
            _hasStartedPenultimateStory = true;
            _storyTeller.StartSideStory(_penultimateStory);
        }
    }

    void ActivateObject()
    {
        if(_hasStartedPenultimateStory && !_hasStartedLastStory)
        {
            _levelManager.enabled = false;
            _hasStartedLastStory = true;
            _animator.SetTrigger(END_HASH);
        }
        else if(_hasStartedLastStory)
        {
            _endScreen.enabled = true;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_mainMenuButton);
        }
    }

    public void TellGameEndStoryAnimationEvent()
    {
        _finalScene.SetActive(true);
        OnFinalPlayed?.Invoke();
        _storyTeller.StartSideStory(_gameEndStory);
    }
}
