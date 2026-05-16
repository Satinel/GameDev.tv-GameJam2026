using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnMoveAction;
    public static event Action<bool> OnActAction;

    InputAction _moveAction, _actAction;

    void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _actAction = InputSystem.actions.FindAction("Act");
    }

    // Update is called once per frame
    void Update()
    {
        OnMoveAction?.Invoke(_moveAction.ReadValue<Vector2>());

        OnActAction?.Invoke(_actAction.IsPressed());
    }
}
