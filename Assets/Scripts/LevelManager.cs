using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static event Action OnLevelStarted, OnLevelFinished;

    [SerializeField] Canvas _startCanvas, _winCanvas;
    [SerializeField] GameObject _startButton, _nextLevelButton;

    void OnEnable()
    {
        Goal.OnGoalAchieved += Goal_OnGoalAchieved;
    }

    void OnDisable()
    {
        Goal.OnGoalAchieved -= Goal_OnGoalAchieved;
    }

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_startButton);
    }

    void Goal_OnGoalAchieved()
    {
        OnLevelFinished?.Invoke();
        _winCanvas.enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_nextLevelButton);
    }

    public void StartLevel()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _startCanvas.enabled = false;
        OnLevelStarted?.Invoke();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
