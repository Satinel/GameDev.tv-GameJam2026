using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static event Action<int> OnAnyCollectableCollected;

    [SerializeField] int _value;
    [SerializeField] Animator _animator;

    float _timer;
    bool _hasAppeared;

    static readonly int APPEAR_HASH = Animator.StringToHash("Appear"), SPIN_HASH = Animator.StringToHash("Spin");

    void Update()
    {
        if(!_hasAppeared) { return; }

        _timer += Time.deltaTime;

        if(_timer >= 0)
        {
            _timer = UnityEngine.Random.Range(-600f, -120f);
            _animator.SetTrigger(SPIN_HASH);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            OnAnyCollectableCollected?.Invoke(_value);
            Destroy(gameObject);
        }
    }

    public void SetAppearTrigger()
    {
        _animator.SetTrigger(APPEAR_HASH);
        _timer = UnityEngine.Random.Range(-600f, -30f);
        _hasAppeared = true;
    }
}
