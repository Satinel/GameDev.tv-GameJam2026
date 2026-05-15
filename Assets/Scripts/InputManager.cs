using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnMoveAction;

    InputAction _moveAction;

    void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        OnMoveAction?.Invoke(_moveAction.ReadValue<Vector2>());
    }
}
