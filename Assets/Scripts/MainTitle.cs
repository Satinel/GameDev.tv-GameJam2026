using UnityEngine;
using UnityEngine.EventSystems;

public class MainTitle : MonoBehaviour
{
    [SerializeField] SceneTransitions _scenTransitions;
    [SerializeField] GameProgressSO _progress;
    [SerializeField]  GameObject _startButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_startButton);
    }

    public void StartGame()
    {
        _progress.Reset();
        _scenTransitions.RequestNextLevel();
    }
}
