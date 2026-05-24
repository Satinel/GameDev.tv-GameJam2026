using System;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public static event Action OnPemmeyEaten;

    [SerializeField] GameProgressSO _progressRef;
    [SerializeField] LevelManager _levelManager;
    [SerializeField] Animator _animator, _stuntPemmingAnimator, _stuntEelAnimator;
    [SerializeField] GameObject _realPemming, _realBubbles;
    [SerializeField] Canvas _healthCanvas;

    static readonly int CUTSCENE_HASH = Animator.StringToHash("Begin");
    static readonly int PEMMINIG_MOVE_HASH = Animator.StringToHash("IsMoving");
    static readonly int EEL_MOVE_HASH = Animator.StringToHash("Moving");

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
            _stuntPemmingAnimator.SetBool(PEMMINIG_MOVE_HASH, true);
            _stuntEelAnimator.SetBool(EEL_MOVE_HASH, true);
        }
    }

    public void PemmeyEatenAnimationEvent()
    {
        _stuntPemmingAnimator.SetBool(PEMMINIG_MOVE_HASH, false);
        OnPemmeyEaten?.Invoke();
    }

    public void EnableBubblesAnimationEvent()
    {
        _realBubbles.SetActive(true);
    }

    public void EndCutsceneAnimationEvent()
    {
        _realBubbles.SetActive(true);
        _realPemming.SetActive(true);
        _healthCanvas.enabled = true;
        _levelManager.enabled = true;
        gameObject.SetActive(false);
    }
}
