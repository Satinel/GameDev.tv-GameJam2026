using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static event Action OnLevelStarted, OnLevelFinished;

    [SerializeField] GameObject _startUI, _successUI;

    void OnEnable()
    {
        Goal.OnGoalAchieved += Goal_OnGoalAchieved;
    }

    void OnDisable()
    {
        Goal.OnGoalAchieved -= Goal_OnGoalAchieved;
    }

    void Goal_OnGoalAchieved()
    {
        _successUI.SetActive(true);
        OnLevelFinished?.Invoke();
    }

    public void StartLevel()
    {
        _startUI.SetActive(false);
        OnLevelStarted?.Invoke();
    }
}
