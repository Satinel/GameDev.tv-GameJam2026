using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField] GameProgressSO _progressRef;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] Animator _animator;

    static readonly int CUTSCENE_HASH = Animator.StringToHash("Begin");

    void Awake()
    {
        if(_progressRef.CutsceneViewed)
        {
            EndCutsceneAnimationEvent();
        }
    }

    void Start()
    {
        if(!_progressRef.CutsceneViewed)
        {
            _progressRef.SetCutsceneViewed();
            _animator.SetTrigger(CUTSCENE_HASH);
        }
    }

    public void EndCutsceneAnimationEvent()
    {
        _levelManager.enabled = true;
        gameObject.SetActive(false);
    }
}
