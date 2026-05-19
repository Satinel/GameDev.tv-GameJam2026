using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnMoveAction;
    public static event Action<bool> OnActAction;
    public static event Action OnJumpAction, OnOptionsAction;

    InputAction _moveAction, _actAction, _jumpAction, _optionsAction;

    void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _actAction = InputSystem.actions.FindAction("Act");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _optionsAction = InputSystem.actions.FindAction("Options");
    }

    // Update is called once per frame
    void Update()
    {
        OnMoveAction?.Invoke(_moveAction.ReadValue<Vector2>());

        OnActAction?.Invoke(_actAction.IsPressed());

        if(_jumpAction.WasPerformedThisFrame())
        {
            OnJumpAction?.Invoke();
        }

        if(_optionsAction.WasPerformedThisFrame())
        {
            OnOptionsAction?.Invoke();
        }
    }
}
