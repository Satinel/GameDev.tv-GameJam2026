using UnityEngine;

public class GameEnder : MonoBehaviour
{
    [SerializeField] StoryTeller _storyTeller;
    [SerializeField] TextAsset _penultimateStory, _gameEndStory;
    [SerializeField] Animator _animator;

    bool _hasStartedPenultimateStory;
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

    void EndGame()
    {
        if(!_hasStartedPenultimateStory)
        {
            _hasStartedPenultimateStory = true;
            _storyTeller.BeginStory(_penultimateStory);
        }
    }

    void ActivateObject()
    {
        if(_hasStartedPenultimateStory)
        {
            _animator.SetTrigger(END_HASH);
        }
    }

    public void TellGameEndStoryAnimationEvent()
    {
        _storyTeller.BeginStory(_gameEndStory);
    }
}
