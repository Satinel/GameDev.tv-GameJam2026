using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressTracker : MonoBehaviour
{
    [SerializeField] int _storyIndex;
    [SerializeField] GameProgressSO _gameProgress;

    void Awake()
    {
        StoryTeller.OnStoryCompleted += SetStoryCompleted;
        Enemy.OnYemmepBit += SetYemmepEaten;
    }

    void OnDestroy()
    {
        StoryTeller.OnStoryCompleted -= SetStoryCompleted;
        Enemy.OnYemmepBit -= SetYemmepEaten;
    }

    void Start()
    {
        _storyIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void SetStoryCompleted()
    {
        _gameProgress.SetStoryViewedByBuildIndex(_storyIndex);
    }

    void SetYemmepEaten()
    {
        _gameProgress.SetYemmepEaten();
    }

}
