using UnityEngine;

public class Chaser : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1.5f, _distractionRate = 5f, _distractionRecovery = 1.5f, _distractionChance = 0.65f;
    [SerializeField] SpriteRenderer _spriteRenderer;
    Transform _player;

    float _timer;
    bool _isDistracted;

    void Start()
    {
        _player = FindFirstObjectByType<PemmingController>().transform;
    }


    void Update()
    {
        if(!_player) { return; }

        _timer += Time.deltaTime;

        if(_timer >= _distractionRate)
        {

            if(_isDistracted)
            {
                _isDistracted = false;
                _timer -= _distractionRate;
            }
            else
            {
                _isDistracted = Random.value > _distractionChance;
                _timer -= _distractionRecovery;
            }
        }

        if(_isDistracted)
        {
            transform.right = transform.position - _player.position;
            _spriteRenderer.flipY = transform.position.x < _player.position.x;
        }
        else
        {
            transform.right = -(transform.position - _player.position);
            _spriteRenderer.flipY = transform.position.x > _player.position.x;
        }

        transform.position += _moveSpeed * Time.deltaTime * transform.right;
    }
}
