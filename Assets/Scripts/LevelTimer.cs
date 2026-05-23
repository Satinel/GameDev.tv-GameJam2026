using System;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public static event Action<float> OnTimerStarted;
    public static event Action<float> ReportCurrentTime;
    public static event Action OnTimerCompleted;

    [SerializeField] float _totalDuration;

    bool _isTimerStarted;
    float _timer;

    void Awake()
    {
        LevelManager.OnLevelStarted += SetTimerStarted;
        PemmingController.OnDefeat += StopTimer;
    }

    void OnDestroy()
    {
        LevelManager.OnLevelStarted -= SetTimerStarted;
        PemmingController.OnDefeat -= StopTimer;
    }

    void Update()
    {
        if(!_isTimerStarted) { return; }

        _timer += Time.deltaTime;
        ReportCurrentTime?.Invoke(_timer);

        if(_timer >= _totalDuration)
        {
            OnTimerCompleted?.Invoke();
            _isTimerStarted = false;
            _timer = 0;
        }
    }

    void SetTimerStarted()
    {
        _isTimerStarted = true;
        OnTimerStarted?.Invoke(_totalDuration);
    }

    void StopTimer()
    {
        _isTimerStarted = false;
    }
}
