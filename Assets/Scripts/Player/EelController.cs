using UnityEngine;

public class EelController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1f;

    Vector3 _moveValue = Vector3.zero;

    void Awake()
    {
        InputManager.OnMoveAction += GetMoveValue;
    }

    void OnDestroy()
    {
        InputManager.OnMoveAction -= GetMoveValue;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position += _moveSpeed * Time.deltaTime * _moveValue;
    }

    void GetMoveValue(Vector2 value)
    {
        _moveValue = value;
    }
}
